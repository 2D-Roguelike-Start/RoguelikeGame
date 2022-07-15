using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Tutorial : MonoBehaviour
{
    [SerializeField]
    private float scanRange = 3;

    public bool android = false;
    bool detect = false;
    float height;
    float distance;
    string Name = "여행자";
    string[] Talk = { "어..안녕하세요!", "말을 알아들을 수 있나..?", "어쨋든.. 이곳은 위험합니다. 돌아가세요..!" };
    static int talkcnt = 0;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Managers.Input.KeyAction -= OnTalk;
        Managers.Input.KeyAction += OnTalk;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        height = Mathf.Abs(gameObject.transform.position.y - player.transform.position.y);
        distance = Mathf.Abs(gameObject.transform.position.x - player.transform.position.x);
    }

    void OnTalk()
    {
        if(Input.GetKeyDown(KeyCode.F) || android)
        {
            Debug.Log("npc talk");
            if (distance <= scanRange && height <= 1)
            {
                Managers.Input.TouchAction -= player.GetAddComponent<PlayerController>().OnTouch;
                //Managers.Input.KeyAction -= player.GetAddComponent<PlayerController>().OnKeyBoard;
                //Managers.Input.MouseAction -= player.GetAddComponent<PlayerController>().OnMouseClicked;
                if (Talk.Length == talkcnt)
                {
                    Managers.Input.TouchAction += player.GetAddComponent<PlayerController>().OnTouch;
                    //Managers.Input.KeyAction += player.GetAddComponent<PlayerController>().OnKeyBoard;
                    //Managers.Input.MouseAction += player.GetAddComponent<PlayerController>().OnMouseClicked;
                    talkcnt = 0;
                    Managers.UI.CloseAllPopupUI();
                }
                else
                {
                    UI_TalkPopup talk = Managers.UI.ShowPopupUI<UI_TalkPopup>();
                    talk.Name = Name;
                    talk.Talk = Talk[talkcnt];
                    talkcnt++;
                    android = false;
                }
            }
        }
    }
}
