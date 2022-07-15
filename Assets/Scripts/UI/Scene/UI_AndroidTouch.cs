using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AndroidTouch : UI_Scene
{
    PlayerController Player;
    VariableJoystick joy;
    Toggle toggle;

    enum Images
    {
        HPBarBack,
        HPBar,
    }

    enum Buttons
    {
        AttackButton,
        JumpButton,
        Setting,
        ActiveButton,
    }

    enum Texts
    {
        HP,
    }

    enum Toggles
    {
        Possession,
    }


    private void Start()
    {
        joy = gameObject.GetComponentInChildren<VariableJoystick>();
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));

        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnClickAttackButton);
        GetButton((int)Buttons.JumpButton).gameObject.BindEvent(OnClickJumpButton);
        GetButton((int)Buttons.Setting).gameObject.BindEvent(OnClickSetting);
        GetButton((int)Buttons.ActiveButton).gameObject.BindEvent(OnClickActiveButton);
        //GetToggle((int)Toggles.Possession).gameObject.BindEvent(OnClickPossession);
        toggle = GetToggle((int)Toggles.Possession);

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_Tutorial", UI_Setting_SoundPopup.BgmSound);
    }

    void OnClickAttackButton()
    {
        Animator animator = Player.gameObject.GetComponentInChildren<Animator>();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            switch (Player.gameObject.name)
            {
                case "Slime_A":
                    Managers.Sound.Play(Define.Sound.Effect, "Sound_Slime_A_Hit", UI_Setting_SoundPopup.EffectSound);
                    break;
                case "Bat_A":
                    GameObject go = Managers.Resource.Instantiate("Creature/Projectile/BatFireball");
                    go.layer = (int)Define.Layer.Projectile_Player;

                    if (Player.gameObject.transform.localScale.x >= 0) go.transform.position = Player.gameObject.transform.position + new Vector3(1, 0, 0);
                    else { 
                        go.transform.position = Player.gameObject.transform.position + new Vector3(-1, 0, 0);
                        go.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    }

                    //Vector3 PlayerShotdir = (Camera.main.ScreenToWorldPoint(joy.gameObject.transform.position) + new Vector3(0, 0, 10)) - Player.gameObject.transform.position;
                    //float angle = Mathf.Atan2(PlayerShotdir.y, PlayerShotdir.x) * Mathf.Rad2Deg;

                    //go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    break;
            }
            animator.SetTrigger("isAttack");
        }
    }

    void OnClickJumpButton()
    {
        if(!Player.isflying)
        {
            if (!Player.animator.GetBool("isJumping"))
            {
                Player.jumpInfo = "JUMP";
                Player.SendMessage("OnKeyBoard");
                Player.moveInfo = null;
            }
        }
        else
        {
            Player.jumpInfo = "JUMPISFLYING";
            Player.SendMessage("OnKeyBoard");
            Player.moveInfo = null;
        }
        
    }

    void OnClickSetting()
    {
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    void OnClickActiveButton()
    {
        NPC_Tutorial npc = GameObject.Find("NPC_Tutorial").GetAddComponent<NPC_Tutorial>();
        npc.android = true;
        npc.SendMessage("OnTalk");
        npc.android = false;
    }

    //void OnClickPossession()
    //{
    //    Debug.Log("OnClickPossession");
    //    //²ô±â
    //    if (!toggle.isOn)
    //    {
    //        Debug.Log(Player.ispossession);
    //        //toggle.isOn = false;
    //        Player.ispossession = false;
    //    }
    //    //ÄÑ±â
    //    else
    //    {
    //        //toggle.isOn = true;
    //        Player.possInfo = "Possesion";
    //        Player.SendMessage("OnKeyBoard");
    //        Player.possInfo = "NotPossesion";
    //    }
    //}
    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        GetText((int)Texts.HP).text = $"{PlayerStat.Hp}";
        GetImage((int)Images.HPBar).fillAmount = Mathf.Lerp(GetImage((int)Images.HPBar).fillAmount, PlayerStat.Hp / PlayerStat.MaxHp, Time.deltaTime * 0.9f);

        if(PlayerStat.Hp > 0)
        {
            //¿À¸¥ÂÊ
            if (joy.Horizontal > 0)
            {
                Debug.Log("RIGHTMoving");
                Player.moveInfo = "RIGHT";
                Player.SendMessage("OnKeyBoard");
            }
            //¿ÞÂÊ
            else if (joy.Horizontal < 0)
            {
                Debug.Log("Moving");
                Player.moveInfo = "LEFT";
                Player.SendMessage("OnKeyBoard");

            }
            else Player.moveInfo = null;

            if (toggle.isOn)
            {
                Player.possInfo = "Possesion";
                Player.SendMessage("OnKeyBoard");
            }
            else
            {
                Player.possInfo = "NotPossesion";
                Player.ispossession = false;
            }
        }
    }
}