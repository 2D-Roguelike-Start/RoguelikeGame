using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // �Ŵ��� �ν��Ͻ� ���ϼ� ����
    static Managers Instance { get { init(); return s_instance; } } //���� Instance�� ����

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerExtended _scene = new SceneManagerExtended();
    UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerExtended Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }

    void Start()
    {
        //�ʱ�ȭ
        init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            // ���� �� ����
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            //���� ����
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
        
    }
}
