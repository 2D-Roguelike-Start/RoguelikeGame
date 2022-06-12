using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object // 리소스의 경로를 받아오는데 타입은 Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null) //리소스의 생성 (경로, 부모객체의 위치정보)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null)
        {
            Debug.Log($"Failed to Load Prefab : {path}");

            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        //(Clone)이름 삭제
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            //잘라내줌 0 ~ index
            go.name = go.name.Substring(0, index);

        return go;
    }

    public void Destroy(GameObject go) // 리소스의 제거
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
