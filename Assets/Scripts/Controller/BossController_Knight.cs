using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Patterns_Knight
{
    Orc_Att,
    Throwing_Axe,
    Go_Wild,
    Earthquake,
    Follow,

    End
}

public class BossController_Knight : MonoBehaviour
{
    GameObject Target; //�÷��̾�
    Animator anim;
    string animationState = "AnimationState";
    EffectController effect;
    Stat stat;    
    GameObject AttCol; //�������� �ݶ��̴�
    Rigidbody2D rigid;
    float dir;
    Vector3 Movedir;

    //��ų����
    bool Counter_Check = false;
    float Save_Hp;


    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        anim = gameObject.GetComponentInChildren<Animator>();
        AttCol = transform.Find("AttackCollider").gameObject;
        effect = GetComponent<EffectController>();
        stat = gameObject.GetComponent<Stat>();
        rigid = this.gameObject.GetComponent<Rigidbody2D>();

        stat.Level = 1;
        stat.MaxHp = 200;
        stat.Hp = stat.MaxHp;
        stat.MoveSpeed = 2f;
        stat.Attack = 20;

        StartCoroutine("Boss_Knight_FlowerCut");
    }

    void Update()
    {
        ////�÷��̾���� ���� ����
        //float Angle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        //if ((Mathf.Abs(Angle) > 90 && transform.localScale.x > 0)
        //    || (Mathf.Abs(Angle) <= 90 && transform.localScale.x < 0))
        //    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //Debug.Log($"Boss Hp : {stat.Hp}");

        //ī���� Ű�� �ݻ絩
        if(Counter_Check && Save_Hp != stat.Hp)
        {
            PlayerStat.Hp -= Save_Hp - stat.Hp;
            Save_Hp = stat.Hp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player)
        {
            effect.EffectOn(collision.transform, "Blood");
            PlayerStat.Hp -= stat.Attack;

            AttCol.SetActive(false);
        }
    }

    void NextPattern()
    {
        if(true)
        {
            //StartCoroutine(Knight_Att());
            //StartCoroutine(FlowerCut());
            //StartCoroutine(Stab());
            //StartCoroutine(Counter());
        }
    }

    void LookPlayer()
    {
        float scale = transform.localScale.y;
        dir = Target.transform.position.x > transform.position.x ? scale : scale * (-1); //����
        Movedir = (Target.transform.position - transform.position).normalized * dir;
        transform.localScale = new Vector3(dir, scale, scale);
    }

    //1������ ��ų
    IEnumerator Boss_Knight_Att()
    {
        anim.SetTrigger("isAttack");
        AttCol.SetActive(true);

        yield return new WaitForSeconds(1);
        AttCol.SetActive(false);
    }

    IEnumerator Boss_Knight_FlowerCut() //6���� �̻��� x3
    {
        //������ ��ǥ�� x0~29, y0~12�ΰ� �������� �ۼ���.
        for(int j = 0; j < 3; ++j)
        {
            float[] XY = new float[2];
            XY[0] = Random.Range(0, 30);
            XY[1] = Random.Range(0, 13);
            for (int i = 0; i < 6; ++i)
            {
                GameObject go = Managers.Resource.Instantiate("Creature/Projectile/Rock Missile");
                go.transform.position = new Vector3(XY[0], XY[1], 0);
                go.transform.rotation = Quaternion.AngleAxis(60*i+90, Vector3.forward);
                go.transform.Translate(Vector3.right*1.5f);
                go.GetComponent<Missile>().Missile_launch_Time = 5f;

                GameObject Warning_Effect = Managers.Resource.Instantiate("Creature/Projectile/Warning_Effect_Pivot");
                Warning_Effect.transform.position = go.transform.position;
                Warning_Effect.transform.localScale = go.transform.localScale;
                Warning_Effect.transform.rotation = go.transform.rotation;
                Warning_Effect.GetComponent<WaringEffect>().Effect_Time = 1.5f;
            }
        }
        yield return new WaitForSeconds(1);
    }

    IEnumerator Boss_Knight_Stab() //���
    {
        GameObject go = Managers.Resource.Instantiate("Creature/Projectile/Rock Missile");
        go.transform.position = new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z);
        go.transform.localScale = new Vector3(3, 3,0);
        go.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        go.GetComponent<Missile>().Missile_launch_Time = 1.5f;

        GameObject Warning_Effect = Managers.Resource.Instantiate("Creature/Projectile/Warning_Effect_Pivot");
        Warning_Effect.transform.position = go.transform.position;
        Warning_Effect.transform.localScale = go.transform.localScale;
        Warning_Effect.transform.rotation = go.transform.rotation;
        Warning_Effect.GetComponent<WaringEffect>().Effect_Time = 1.5f;

        yield return new WaitForSeconds(1);
    }

    IEnumerator Boss_Knight_Counter() //���ӽð����� �ݻ�
    {
        Counter_Check = true;
        Save_Hp = stat.Hp;
        yield return new WaitForSeconds(3);
        Counter_Check = false;

        yield return new WaitForSeconds(1);
    }

    //2������ ��ų
    IEnumerator Boss_Knight_Rush() //���ӽð����� �ݻ�
    {
        LookPlayer();
        //anim.SetInteger(animationState, 0);
        Vector3 playerPosition = Target.transform.position;
        rigid.AddForce(Movedir * dir * 10000);
        AttCol.gameObject.SetActive(true);
        while (true) 
        {
            yield return new WaitForSeconds(0.1f);
            if (Mathf.Abs(transform.position.x - playerPosition.x) <= 1)
            {
                //.SetInteger(animationState, 0);
                rigid.velocity = new Vector2(0, 0);
                //onofftile.enabled = true;
                break;
            }
        }
        AttCol.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
    }
}
