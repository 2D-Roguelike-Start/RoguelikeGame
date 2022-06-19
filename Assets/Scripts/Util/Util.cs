using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    //component�� ������ ã�����ؼ� �߰��ϴ±��
    public static T GetAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    //���� ������Ʈ�� ���ǿ� �´°� ã��
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform =  FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }

    // �ڽ� ��ü���� �´°� ã�� , recursive ��������� ã�����ΰ�?? (�ڽ�, �ڽ��� �ڽ�)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                //���ǿ� �´� ���� �ڽ��� Transform ���� ��ȯ
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
                //�̸��� �Է����� �ʾ����� T Ÿ�Ը� ������ ��ȯ
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;


            }
        }
        return null;
    }
}
