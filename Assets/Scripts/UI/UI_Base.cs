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

    //reflection �̿� // ���ϴ� ui �� ã�� ���� mapping
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //enum ���� string���� �ٲ���
        string[] names = Enum.GetNames(type);
        //names�� ���̸�ŭ objects �迭 ����
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        //Dictionaly key value �߰�
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // ������Ʈ�� �ƴ� ���� ������Ʈ�� ã�� ����
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);

            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to Bind{names[i]}");
        }
    }

    //���ϴ� ui�� �������� ����
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        //dictionary key �� �̿��� ����
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    //���ֻ���ϴ°͵� �޼���
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Scrollbar GetScroll(int idx) { return Get<Scrollbar>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }


    //�̺�Ʈ �߰� �޼���
    public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
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
