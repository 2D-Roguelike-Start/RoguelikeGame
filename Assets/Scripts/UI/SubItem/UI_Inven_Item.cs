using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameTest,
    }

    string _name;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameTest).GetComponent<Text>().text = _name;

        GameObject go = Get<GameObject>((int)GameObjects.ItemIcon);
        BindEvent(go, (PointerEventData) => { Debug.Log($"아이템 클릭 : {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
