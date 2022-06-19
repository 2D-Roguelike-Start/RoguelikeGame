using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    //reflection 이용 // 원하는 ui 를 찾는 과정 mapping
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //enum 형을 string으로 바꿔줌
        string[] names = Enum.GetNames(type);
        //names의 길이만큼 objects 배열 생성
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        //Dictionaly key value 추가
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // 컴포넌트가 아닌 게임 오브젝트로 찾는 과정
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);

            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to Bind{names[i]}");
        }
    }

    //원하는 ui를 꺼내오는 과정
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        //dictionary key 값 이용해 추출
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    //자주사용하는것들 메서드
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    //이벤트 추가 메서드
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventController evt = Util.GetAddComponent<UI_EventController>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }

    }
}
