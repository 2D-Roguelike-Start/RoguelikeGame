using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;
    //UI관련 오더스택으로관리
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            // 없을 시 생성
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    //외부에서 UI가 켜질때 order 연산 관리기능
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    //Scene 띄우기
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        //이름을 안받았으면 T타입의 이름을 name에 저장
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T SceneUI = Util.GetAddComponent<T>(go);
        _sceneUI = SceneUI;

        //UI_Root 게임오브젝트 산하로 배치
        go.transform.SetParent(Root.transform);

        return SceneUI;
    }

    //서브 아이템 만들기(자식 ui)
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        //이름을 안받았으면 T타입의 이름을 name에 저장
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetAddComponent<T>(go);
    }
    //UI 띄우기
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        //이름을 안받았으면 T타입의 이름을 name에 저장
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        
        T popup = Util.GetAddComponent<T>(go);
        _popupStack.Push(popup);

        //UI_Root 게임오브젝트 산하로 배치
        go.transform.SetParent(Root.transform);

        return popup;
    }

    //누구를 삭제하는 것인가 실수 방지
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }
        ClosePopupUI();
    }

    //스택 삭제 일반 버전
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);

        popup = null;
        _order--;
    }

    //스택 비우기
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }


}
