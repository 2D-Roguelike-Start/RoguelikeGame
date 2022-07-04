using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Patterns_Bat
{
    Default_Att,
    AirCut,
    DarkBall,
    Blackout,

    End
}

public class BossController_Bat : MonoBehaviour
{
    GameObject Target; //플레이어
    Patterns_Bat Next_Pattern = Patterns_Bat.End;
    Stat stat;
    Rigidbody2D rigid;

    float dir;

    int Skile_Count = 1; //몇번째 차례의 공격인가1

    //에어컷
    public GameObject[] AirCut_WarningEffect;
    //암막
    bool Blackout_Check = false;

    private void Start()
    {
        Target = GameObject.FindWithTag("Player");
        stat = gameObject.GetComponent<Stat>();
        rigid = this.gameObject.GetComponent<Rigidbody2D>();

        stat.Level = 1;
        stat.MaxHp = 300;
        stat.Hp = 1;
        stat.MoveSpeed = 5;
        stat.Attack = 12;

        StartCoroutine("Default_Att");
        //StartCoroutine("DarkBall");
    }
    void Set_NextPattern()
    {
        //순서 : 평타/암막(평타)/랜덤(평타or다크볼)  
        Skile_Count++;
        if (Skile_Count > 3)
            Skile_Count -= 3; //3주기 순환이라 4번째 = 1번째

        switch (Skile_Count)
        {
            case 1:
                Next_Pattern = Patterns_Bat.Default_Att;
                break;
            case 2:
                if (!Blackout_Check && stat.Hp <= stat.MaxHp / 5)
                    Next_Pattern = Patterns_Bat.Blackout;
                else
                    Next_Pattern = Patterns_Bat.Default_Att;
                break;
            case 3:
                int N = UnityEngine.Random.Range(1, 11);//1~10중에 하나 반환
                if (N <= 3) 
                    Next_Pattern = Patterns_Bat.AirCut;
                else
                     Next_Pattern = Patterns_Bat.DarkBall;
                break;
        }
        NextPattern();
    }

    void NextPattern()
    {
        switch (Next_Pattern)
        {
            case Patterns_Bat.Default_Att: 
                StartCoroutine(Default_Att());
                break;
            case Patterns_Bat.AirCut:
                StartCoroutine(AirCut());
                break;
            case Patterns_Bat.DarkBall:
                StartCoroutine(DarkBall());
                break;
            case Patterns_Bat.Blackout:
                StartCoroutine(Blackout());
                break;
        }
    }

    private void Update()
    {
        //플레이어와의 각도 구함
        float Angle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        if ((Mathf.Abs(Angle) > 90 && transform.localScale.x > 0 )
            ||(Mathf.Abs(Angle) <= 90  && transform.localScale.x < 0))
            transform.localScale = new Vector3(transform.localScale.x* - 1, transform.localScale.y, transform.localScale.z);
        //Debug.Log($"Boss Hp : {stat.Hp}");
    }
    void LookDownPlayer()
    {
        float scale = transform.localScale.y;
        dir = Target.transform.position.x > transform.position.x ? scale * (-1) : scale; //방향
        transform.localScale = new Vector3(dir, scale, scale);
    }

    IEnumerator Default_Att()
    {
        //플레이어와의 각도 구함
        float Angle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        GameObject go = Managers.Resource.Instantiate("Creature/Projectile/Poison Missile");
        go.transform.position = transform.position;
        go.transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        //벽 통과는 지금 미사일 충돌 기준이 Floor 레이어 기준이니까 나중에 타일맵 2중으로 만들어서, 외벽이랑 내벽 구분함.

        yield return new WaitForSeconds(1);
        //if(Target.GetComponent<Stat>().Hp > 0)
            Set_NextPattern();
    }

    IEnumerator AirCut()
    {
        //박쥐의 x좌표는 맵 중앙에 고정되어 있음을 가정함.

        //화면 12.5*7
        //경고
        int[] Warning_Num = new int[4];
        for(int i = 0 ; i < 3 ; i += 2)
        {
            yield return new WaitForSeconds(0.5f);
            //알림
            Warning_Num[0 + i] = UnityEngine.Random.Range(0, 3);
            AirCut_WarningEffect[Warning_Num[0 + i]].SetActive(true);
            while (true)
            {
                Warning_Num[1 + i] = UnityEngine.Random.Range(0, 3);
                if (Warning_Num[0 + i] != Warning_Num[1 + i])
                    break;
            }
            AirCut_WarningEffect[Warning_Num[1 + i]].SetActive(true);
            yield return new WaitForSeconds(0.5f);

            //알림 off
            AirCut_WarningEffect[Warning_Num[0 + i]].SetActive(false);
            AirCut_WarningEffect[Warning_Num[1 + i]].SetActive(false);
        }

        // 공격 
        //(이펙트 찾으면 그때 다시. 지금은 틀만 만듦.)
        for (int i = 0; i < 3; i += 2)
        {
            yield return new WaitForSeconds(0.5f);
            AirCut_WarningEffect[Warning_Num[0 + i]].SetActive(true);
            AirCut_WarningEffect[Warning_Num[1 + i]].SetActive(true);
            yield return new WaitForSeconds(0.5f);

            //off
            AirCut_WarningEffect[Warning_Num[0 + i]].SetActive(false);
            AirCut_WarningEffect[Warning_Num[1 + i]].SetActive(false);
        }

        Set_NextPattern();
    }

    IEnumerator DarkBall()
    {
        float[] Ballx = new float[4]; 
        float[] Bally = new float[4]; 
        for(int i = 0; i < 4; ++i)
        {
            Ballx[i] = (Random.Range(0, 26) + Random.Range(0, 0.9999f) - 12.5f) / 2;
            Bally[i] = (Random.Range(0, 15) + Random.Range(0, 0.9999f) - 7) / 2;
            GameObject Obj = Managers.Resource.Instantiate("Creature/Projectile/Bat_DarkBall");
            Obj.transform.position = new Vector3 { x = transform.position.x + Ballx[i], y = transform.position.y + Bally[i], z = 0 };
            Obj.GetComponent<Bomb>().Type = Bomb_Type.DarkBall;
        }
        yield return new WaitForSeconds(2); //터지는거 기다림
        yield return new WaitForSeconds(1); //원래 대기시간 쿨타임
        Set_NextPattern(); 
    }
     
    IEnumerator Blackout()
    {
        Blackout_Check = true;
        Target.transform.Find("Bat_Blackout").gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Set_NextPattern();
    }
    void OnDestroy()
    {
        Target.transform.Find("Bat_Blackout").gameObject.SetActive(false);
    }
}