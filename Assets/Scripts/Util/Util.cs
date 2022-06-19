using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    //component가 없으면 찾던가해서 추가하는기능
    public static T GetAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    //게임 오브젝트로 조건에 맞는걸 찾음
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform =  FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }

    // 자식 객체에서 맞는걸 찾음 , recursive 재귀적으로 찾을것인가?? (자식, 자식의 자식)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                //조건에 맞는 직속 자식의 Transform 값을 반환
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                //이름을 입력하지 않았을때 T 타입만 맞으면 반환
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;


            }
        }
        return null;
    }
}
