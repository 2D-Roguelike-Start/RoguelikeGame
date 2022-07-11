using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControllerShort : CreatureController
{
    //���� �پ��ִ� �ٰŸ� ���� ��Ʈ�ѷ�
    Stat stat;
    Rigidbody2D _rigid;
    ActionController action;

    [SerializeField]
    float _scanRange = 5;
    [SerializeField]
    float _attackRange = 1.7f;
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
    bool isflying = false;
    bool isattack = false;
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
        if (gameObject.tag == "Flying_Long") { isflying = true; _scanRange = 3f; } //CancelInvoke(); }
        stat = gameObject.GetComponent<Stat>();
        _rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        action = GetComponentInChildren<ActionController>();

        stat.Level = 1;
        stat.MaxHp = 10;
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
        frontcheck = Physics2D.Raycast(frontVec + new Vector2(0, 1), movementFlag == 1 ? Vector3.right :Vector3.left, 0.2f, LayerMask.GetMask("Floor"));
    }

    #region Idle
    protected override void UpdateIdle()
    {
        if(!isflying)
        {
            XYcheck(); // �� �����Ӹ��� �Ÿ� ���
            VectorAndRay();
        }
        anim.SetInteger(animationState, (int)Define.CreatureState.Idle);

        if (stat.Hp <= 0)
        {
            if (action.equipWeapon && !isflying)
                action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
            action.PossessionTimerOn(); //Ÿ�̸� ON
            State = Define.CreatureState.Die;
            return;
        }

        if(!isflying)
        {
            if (distance <= _scanRange && height < 1) // X �Ÿ��� _scanRange�̳��̰� �������̰� 1 �̸��϶�
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
        }
        else
        {
            if(distance <= _scanRange && height <= 5f)
            {
                State = Define.CreatureState.Attack;
                return;
            }
            else
            {
                State = Define.CreatureState.Moving;
                return;
            }
        }
    }
    #endregion
    #region Moving
    protected override void UpdateMoving()
    {
        XYcheck();
        if (!isflying)
        {
            VectorAndRay();
        }
        anim.SetInteger(animationState, (int)Define.CreatureState.Moving);

        if (stat.Hp <= 0)
        {
            if(!isflying)
            {
                if (AI) AI = false;
                CancelInvoke();
                if (action.equipWeapon)
                    action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
            }
            action.PossessionTimerOn(); //Ÿ�̸� ON
            State = Define.CreatureState.Die;
            return;
        }
        if(!isflying)
        {
            if (AI)
            {
                if (distance <= _scanRange && height < 1)
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

                if (distance <= _attackRange && height < 1)
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
        }
        else
        {
            if (distance <= _attackRange)
            {
                State = Define.CreatureState.Attack;
                return;
            }
            else
            {
                if (transform.position.x < _lockTarget.transform.position.x) movementFlag = 1;
                else movementFlag = -1;
                MoveFlag(movementFlag);
            }
        }
    }
    #endregion

    #region Attack
    protected override void UpdateAttack()
    {
        XYcheck();
        if (!isflying)
        { 
            anim.SetInteger(animationState, (int)Define.CreatureState.Attack);

            // ���ݹ������� distance�� �־����� ���ÿ� Attack �ִϸ��̼��� 1���̻� ���� �� ����
            if (_attackRange < distance && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
                gameObject.GetComponentInChildren<ActionController>().equipWeapon == false)
            {

                anim.SetInteger(animationState, (int)Define.CreatureState.Moving);
                State = Define.CreatureState.Moving;
                return;
            }
        }
        else
        {
            if (!isattack)
            {
                isattack = true;
                StartCoroutine("Shoot");
            }
            if (distance > _attackRange && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                State = Define.CreatureState.Moving;
        }
    }
    IEnumerator Shoot()
    {
        BatScan ShootPos = gameObject.GetComponentInChildren<BatScan>();
        GameObject[] Shoot = ShootPos.shootPos;
        int count = Shoot.Length;
        Managers.Sound.Play(Define.Sound.Effect, "Sound_BatAttack", UI_Setting_SoundPopup.EffectSound);
        for (int i = 0; i < count; i++)
        {
            switch (i)
            {
                case 0: Shoot[i].transform.rotation = Quaternion.AngleAxis(-45, Vector3.forward); break;
                case 1: Shoot[i].transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward); break;
                case 2: Shoot[i].transform.rotation = Quaternion.AngleAxis(-135, Vector3.forward); break;
                case 3: Shoot[i].transform.rotation = Quaternion.AngleAxis(45, Vector3.forward); break;
                case 4: Shoot[i].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward); break;
                case 5: Shoot[i].transform.rotation = Quaternion.AngleAxis(135, Vector3.forward); break;
            }
            GameObject go = Managers.Resource.Instantiate("Creature/Projectile/BatFireball");
            go.layer = (int)Define.Layer.Projectile_Enemy;
            go.transform.position = Shoot[i].transform.position;
            go.transform.rotation = Shoot[i].transform.rotation;
        }
        yield return new WaitForSeconds(2f);
        isattack = false;
    }
    #endregion

    #region Die
    protected override void UpdateDie()
    {
        if (isflying) _rigid.gravityScale = 1.8f;

        if (action.Timer.GetComponent<PossessionRadialProgress>().timeover)
        {
            Destroy(gameObject);
        }
    }
    #endregion
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Monster hit! : {collision.name}");
        if (State == Define.CreatureState.Die) return;

        if (stat.Hp <= 0)
        {
            DieSound();
            if(!isflying)
            {
                if (action.equipWeapon)
                    action.AttackColliderOnOff(); //Ȥ�ö� �����ִ� �ݶ��̴� OFF
            }
            action.PossessionTimerOn(); //Ÿ�̸� ON
            State = Define.CreatureState.Die;
        }

        if (State != Define.CreatureState.Die && State == Define.CreatureState.Attack)
        {
            Debug.Log("Monster Attack On");
            if (collision.gameObject.layer == (int)Define.Layer.Player)
            {
                if (PlayerStat.Hp > 0)
                    PlayerStat.Hp -= stat.Attack;
            }
        }
    }

    private void DieSound()
    {
        string[] names = System.Enum.GetNames(typeof(Define.Enemy_Short));
        string thisName = null;
        for (int i = 0; i < names.Length - 1; i++)
        {
            if (gameObject.name == names[i]) { thisName = names[i]; break; }
        }
        switch (thisName)
        {
            case "Slime_A":
                Managers.Sound.Play(Define.Sound.Effect, "Sound_SlimeDie", UI_Setting_SoundPopup.EffectSound);
                break;
            case "Bat_A":
                Managers.Sound.Play(Define.Sound.Effect, "Sound_BatDie", UI_Setting_SoundPopup.EffectSound);
                break;
                //case "Skeleton_C":
                //    Managers.Sound.Play(Define.Sound.Effect, "Sound_SkeletonDie", UI_Setting_SoundPopup.EffectSound);
                //    break;

        }
    }
}
