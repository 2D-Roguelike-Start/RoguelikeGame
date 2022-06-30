using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControllerLong : CreatureController
{
    //���� �پ��ִ� ���Ÿ� ���� ��Ʈ�ѷ�
    Stat stat;
    Rigidbody2D _rigid;
    ActionController action;

    [SerializeField]
    float _scanRange = 5;
    [SerializeField]
    float _attackRange = 8;
    [SerializeField]
    float distance;
    [SerializeField]
    float height;

    Vector2 frontVec;
    RaycastHit2D bottomcheck, frontcheck;

    Animator anim;
    //Idle ������ ����
    int movementFlag;
    bool AI = false;
    string animationState = "AnimationState";

    private void Awake()
    {
        movementFlag = 1;
        Invoke("Think", 2);
    }

    void Think()
    {
        movementFlag = Random.Range(-1, 2);
        Invoke("Think", 2);
    }

    protected override void Init()
    {
        stat = gameObject.GetComponent<Stat>();
        _rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        action = GetComponentInChildren<ActionController>();

        stat.Level = 1;
        stat.MaxHp = 100;
        stat.Hp = 10;
        stat.MoveSpeed = 2;
        stat.Attack = 10;
    }

    //ũ���Ŀ� �÷��̾� ���� X, Y ���� ���밪 ���
    void XYcheck()
    {
        distance = Mathf.Abs((_lockTarget.transform.position - transform.position).x);
        height = Mathf.Abs((_lockTarget.transform.position - transform.position).y);
    }

    // ������ ���� ���� ���, ���� ����ϴ� ����ĳ��Ʈ ���
    void VectorAndRay()
    {
        frontVec = new Vector2(_rigid.position.x + movementFlag, _rigid.position.y);
        bottomcheck = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        frontcheck = Physics2D.Raycast(frontVec + new Vector2(0, 1), movementFlag == 1 ? Vector3.right : Vector3.left, 1, LayerMask.GetMask("Floor"));
    }

    protected override void UpdateIdle()
    {
        XYcheck(); // �� �����Ӹ��� �Ÿ� ���
        VectorAndRay();
        anim.SetInteger(animationState, (int)Define.CreatureState.Idle);

        if (distance <= _scanRange && height < 2) // X �Ÿ��� _scanRange�̳��̰� �������̰� 1 �̸��϶�
        {
            Debug.Log("Find Player in Idle");
            if (distance < _attackRange)
            {
                State = Define.CreatureState.Moving;
                return;
            }
            Debug.DrawRay(frontVec, Vector3.down, Color.green);
            if (bottomcheck.collider == null)
            {
                CancelInvoke();
                return;
            }
            // + ��Ÿ�� �ϴ� ��� ���
            AI = false;
            State = Define.CreatureState.Moving;
            return;
        }
        else { AI = true; if (movementFlag != 0) State = Define.CreatureState.Moving; }

        if (stat.Hp <= 0)
        {
            if (action.equipWeapon)
                action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
            action.PossessionTimerOn(); //Ÿ�̸� ON
            State = Define.CreatureState.Die;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        XYcheck();
        VectorAndRay();
        anim.SetInteger(animationState, (int)Define.CreatureState.Moving);

        if (AI)
        {
            if (distance <= _scanRange && height < 2)
            {
                Debug.Log("Find Player in Moving");
                // + ��Ÿ�� �ϴ� ��� ���
                AI = false;
                //State = Define.CreatureState.Moving;
                return;
            }
            MoveFlag(movementFlag);

            Debug.DrawRay(frontVec, Vector3.down, Color.red);
            Debug.DrawRay(frontVec + new Vector2(0, 1), movementFlag == 1 ? Vector3.right : Vector3.left, Color.red);

            if (bottomcheck.collider == null || frontcheck.collider != null)
            {
                movementFlag = movementFlag * (-1);
                CancelInvoke();
                Invoke("Think", 2);
                return;
            }
        }
        else
        {
            CancelInvoke();
            if (transform.position.x < _lockTarget.transform.position.x) movementFlag = 1;
            else movementFlag = -1;

            MoveFlag(movementFlag);
            Debug.DrawRay(frontVec, Vector3.down, Color.green);

            if (distance <= _attackRange && height < 2)
            {
                State = Define.CreatureState.Attack;
                return;
            }

            if (bottomcheck.collider == null || frontcheck.collider != null || distance > _scanRange)
            {
                State = Define.CreatureState.Idle;
                Invoke("Think", 2);
                return;
            }
        }
        if (stat.Hp <= 0)
        {
            if (action.equipWeapon)
                action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
            action.PossessionTimerOn(); //Ÿ�̸� ON
            State = Define.CreatureState.Die;
            return;
        }
    }

    protected override void UpdateAttack()
    {
        XYcheck();
        anim.SetInteger(animationState, (int)Define.CreatureState.Attack);
        if (transform.position.x < _lockTarget.transform.position.x) transform.localScale = new Vector3(1, 1, 1); //���� �ٶ󺸴� ����
        else transform.localScale = new Vector3(-1, 1, 1); //������ �ٶ󺸴� ����
        //  Attack �ִϸ��̼��� 1���̻� ���� �� ����
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (_attackRange < distance || height > 2f)
            {
                anim.SetInteger(animationState, (int)Define.CreatureState.Moving);
                State = Define.CreatureState.Moving;
                return;
            }
            if (stat.Hp <= 0)
            {      
                action.PossessionTimerOn(); //Ÿ�̸� ON
                State = Define.CreatureState.Die;
                return;
            }
        }
    }

    protected override void UpdateDie()
    {
        if (action.Timer.GetComponent<PossessionRadialProgress>().timeover)
        {
            Destroy(gameObject);
        }
    }

    void MoveFlag(int movementFlag)
    {
        switch (movementFlag)
        {
            case -1:
                transform.localScale = new Vector3(-1, 1, 1); //���� �ٶ󺸴� ����
                transform.Translate(Vector3.left * Time.deltaTime * (stat.MoveSpeed));  //���� * �ӵ�
                break;
            case 1:
                transform.localScale = new Vector3(1, 1, 1); //������ �ٶ󺸴� ����
                transform.Translate(Vector3.right * Time.deltaTime * (stat.MoveSpeed));  //���� * �ӵ�
                break;
            default:
                anim.SetInteger(animationState, (int)Define.CreatureState.Idle);
                State = Define.CreatureState.Idle;
                break;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log($"Monster hit! : {collision.name}");
    //    if (State != Define.CreatureState.Die && State == Define.CreatureState.Attack)
    //    {
    //        Debug.Log("Monster Attack On");
    //        if (collision.gameObject.layer == (int)Define.Layer.Player)
    //        {
    //            if (PlayerStat.Hp > 0)
    //                PlayerStat.Hp -= stat.Attack;
    //        }

    //    }
    //    if (stat.Hp <= 0)
    //    {
    //        if (action.equipWeapon)
    //            action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
    //        action.PossessionTimerOn(); //Ÿ�̸� ON
    //        State = Define.CreatureState.Die;
    //    }
    //}
}
