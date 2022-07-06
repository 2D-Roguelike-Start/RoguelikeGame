using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum Patterns_Orc
{
    Orc_Att,
    Throwing_Axe,
    Go_Wild,
    Earthquake,
    Follow,

    End
}

public class BossController_Orc : Stat
{
    GameObject Target; //플레이어
    Patterns_Orc Next_Pattern = Patterns_Orc.End;
    Animator anim;
    string animationState = "AnimationState";
    EffectController effect;

    int MoveCheck = 0;
    //지진
    int Earthquake_Count = 0;
    GameObject mainCamera;

    //도끼
    public GameObject Axe;
    float Axe_Angle = 0;
    float Axe_Speed = 2;
    
    //콜라이더
    GameObject AttCol;

    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        anim = gameObject.GetComponentInChildren<Animator>();
        AttCol = transform.Find("AttackCollider").gameObject;
        effect = GetComponent<EffectController>();
        //Axe = transform.Find("Rig Weapon").gameObject;

        Level = 1;
        MaxHp = 500;
        Hp = 10;
        MoveSpeed = 2;
        Attack = 12;

        StartCoroutine("Follow");
    }

    void Update()
    {
        //플레이어와의 각도 구함
        float Angle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        if ((Mathf.Abs(Angle) > 90 && transform.localScale.x > 0)
            || (Mathf.Abs(Angle) <= 90 && transform.localScale.x < 0))
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        if(Axe_Angle != 0 )
        {
            Axe.transform.Rotate(Vector3.forward * Axe_Speed);
            Axe.transform.position = new Vector3(Axe.transform.position.x + Mathf.Cos(Axe_Angle)* Axe_Speed, Axe.transform.position.y + Mathf.Sin(Axe_Angle)* Axe_Speed, Axe.transform.position.z);
        }

        if (MoveCheck != 0)
        {
            gameObject.transform.Translate(new Vector3(MoveCheck, 0, 0) * MoveSpeed * Time.deltaTime);
            if(MoveCheck > 0 && gameObject.transform.position.x > Target.transform.position.x
                || MoveCheck < 0 && gameObject.transform.position.x < Target.transform.position.x)
                MoveCheck *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player)
        {
            effect.EffectOn(collision.transform, "Blood");
            PlayerStat.Hp -= Attack;

            AttCol.SetActive(false);
        }
    }

    void HPCheck()
    {
        if (Hp < (MaxHp / 10) * 3 && Earthquake_Count != 3)
        {
            Earthquake_Count = 3;
            StartCoroutine(Go_Wild());
        }
        else if(Hp < (MaxHp / 10) * 5 && Earthquake_Count >= 1)
        {
            Earthquake_Count = 2;
            StartCoroutine(Earthquake());
        }
        else if (Hp < (MaxHp / 10) * 8 && Earthquake_Count == 0)
        {
            Earthquake_Count = 1;
            StartCoroutine(Earthquake());
        }
        else
            StartCoroutine(Follow());
    }

    IEnumerator Orc_Att()
    {
        anim.SetTrigger("isAttack");
        AttCol.SetActive(true);

        yield return new WaitForSeconds(2);
        AttCol.SetActive(false);
        StartCoroutine(Throwing_Axe());
    }

    IEnumerator Throwing_Axe()
    {
        Vector3 Save_Axe = Axe.transform.position;
        Quaternion Save_Axe_Rotation = Axe.transform.rotation;
        Axe.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        //플레이어와의 각도 구함
        Axe_Angle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg; 
        //update에서 angle 기준으로 도끼 던질거임.

        yield return new WaitForSeconds(2);
        Axe_Angle = 0;
        Axe.GetComponent<BoxCollider2D>().gameObject.SetActive(false);
        Axe.transform.position = Save_Axe;
        Axe.transform.rotation = Save_Axe_Rotation;
        HPCheck();
    }
    IEnumerator Go_Wild()
    {
        MoveSpeed *= 2;
        Attack *= 2;
        yield return new WaitForSeconds(1);
        StartCoroutine(Follow());
    }
    IEnumerator Earthquake()
    {
        float Save_Speed = PlayerStat.MoveSpeed;
        PlayerStat.MoveSpeed = (PlayerStat.MoveSpeed/10)*7;
        mainCamera.GetComponent<CameraShake>().Shake();
        StartCoroutine(Follow());
        yield return new WaitForSeconds(5);
        PlayerStat.MoveSpeed = Save_Speed;
    }

    //2초간 플레이어 주시잡고 따라감.
    IEnumerator Follow()
    {
        anim.SetInteger(animationState, 1); //animationState 1이면 이동 0이면 idle
        //이동
        if (Target.transform.position.x > transform.position.x)
            MoveCheck = 1;
        else
            MoveCheck = -1;

        yield return new WaitForSeconds(2);
        anim.SetInteger(animationState, 0);
        MoveCheck = 0;

        //이동 후 평타 거리 이내에 있다면 평타.
        if (Vector3.Distance(Target.transform.position, transform.position) < 6)
            StartCoroutine(Orc_Att());
        else
            StartCoroutine(Throwing_Axe());
        yield return null;
    }
}
