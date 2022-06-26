using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;
    //UI���� �����������ΰ���
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            // ���� �� ����
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    //�ܺο��� UI�� ������ order ���� �������
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

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

    //Scene ����
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        //�̸��� �ȹ޾����� TŸ���� �̸��� name�� ����
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T SceneUI = Util.GetAddComponent<T>(go);
        _sceneUI = SceneUI;

        //UI_Root ���ӿ�����Ʈ ���Ϸ� ��ġ
        go.transform.SetParent(Root.transform);

        return SceneUI;
    }

    //���� ������ �����(�ڽ� ui)
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        //�̸��� �ȹ޾����� TŸ���� �̸��� name�� ����
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetAddComponent<T>(go);
    }
    //UI ����
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        //�̸��� �ȹ޾����� TŸ���� �̸��� name�� ����
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        
        T popup = Util.GetAddComponent<T>(go);
        _popupStack.Push(popup);

        //UI_Root ���ӿ�����Ʈ ���Ϸ� ��ġ
        go.transform.SetParent(Root.transform);

        return popup;
    }

    //������ �����ϴ� ���ΰ� �Ǽ� ����
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

    //���� ���� �Ϲ� ����
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);

        popup = null;
        _order--;
    }

    //���� ����
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
