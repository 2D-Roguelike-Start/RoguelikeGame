using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 매니저 인스턴스 유일성 보장
    static Managers Instance { get { Init(); return s_instance; } } //모든걸 Instance로 통제

    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerExtended _scene = new SceneManagerExtended();
    UIManager _ui = new UIManager();
    DataManager _data = new DataManager();
    SoundManager _sound = new SoundManager();
    TalkManager _talk = new TalkManager();

    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerExtended Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static DataManager Data { get { return Instance._data; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static TalkManager Talk { get { return Instance._talk; } }

    void Start()
    {
        //초기화
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            // 없을 시 생성
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            //삭제 방지
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._pool.init();
            s_instance._data.init();
            s_instance._sound.init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
