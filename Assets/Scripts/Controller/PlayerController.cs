using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D boxCol2D;
    private Rigidbody2D rigid; //�÷��̾�  rigid body
    private Animator animator; //�÷��̾� �ִϸ��̼�
    private PossessionController possession; // �÷��̾� ����
    private ActionController action;
    public EffectController effect;

    private bool inputIdle = false;
    private bool inputRight = false;
    private bool inputLeft = false;
    private bool inputJump = false;
    private bool isdie = false;
    public bool ispossession; // ��ũ��Ʈ ���� ���� ���� ����
    private string animationState = "AnimationState";

    //�÷��̾� ���µ�
    enum States
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Skill = 3,
        Die = 4,
    }

    void Init() //�÷��̾� ������Ʈ ����κ�
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        possession = GetComponent<PossessionController>();
        action = GetComponentInChildren<ActionController>();
        effect = GameObject.Find("Effect").GetComponent<EffectController>();
    }

    void Start()
    {
        Init();
        action.PossessionTimerOff();
        //Input Manager �̿� 
        //Ű ����
        Managers.Input.KeyAction -= OnKeyBoard; // �̹� �۵��� �Ǽ� ����
        Managers.Input.KeyAction += OnKeyBoard;
        //Ű ���� ���� ����
        Managers.Input.NonKeyAction -= NonKeyBoard;
        Managers.Input.NonKeyAction += NonKeyBoard;
        //���콺 �巡�� , Ŭ�� ���� ( Define Ŭ���� ���� )
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        float fallSpeed = rigid.velocity.y;
        if(inputIdle)
        {
            inputIdle = false;
            rigid.velocity = new Vector2(0, fallSpeed);
        }
        if (inputRight)
        {
            inputRight = false;
            rigid.velocity = new Vector2(PlayerStat.MoveSpeed, fallSpeed);
        }
        if(inputLeft)
        {
            inputLeft = false;
            rigid.velocity = new Vector2(-PlayerStat.MoveSpeed, fallSpeed);
        }
        if (inputJump)
        {
            inputJump = false;
            rigid.AddForce(Vector2.up * PlayerStat.JumpPower, ForceMode2D.Impulse);
        }
        if(isdie)
        {
            isdie = false;
            rigid.velocity = new Vector2(0, fallSpeed);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Trigger : {collision.gameObject}");
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            if(collision.GetComponent<Stat>().Hp > 0)
            {
                if (transform.position.x <= collision.transform.position.x)
                {
                    collision.transform.position += new Vector3(0.3f, 0, 0);
                    effect.hit_closeRange.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                }
                else
                {
                    collision.transform.position += new Vector3(-0.3f, 0, 0);
                    effect.hit_closeRange.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                }

                effect.EffectOn(collision.transform);
                collision.GetComponentInParent<Stat>().Hp -= PlayerStat.Attack;
            }
        }
       
        if (PlayerStat.Hp <= 0)
        {
            isdie = true;
            animator.SetBool("isDie", true);
            Managers.Input.KeyAction -= OnKeyBoard;
            Managers.Input.NonKeyAction -= NonKeyBoard;
            Managers.Input.MouseAction -= OnMouseClicked;
            return;
        }
    }

    //Ű���忡 ������ ������ �� ����
    void OnKeyBoard()
    {
        RaycastHit2D raycasHit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, new Vector2(0, -0.3f), Color.red);
        if (raycasHit.collider != null)
            animator.SetBool("isJumping", false);
        else animator.SetBool("isJumping", true);

        if (Input.GetKey(KeyCode.A)) // ���� �̵�
        {
            inputLeft = true;
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(-1, 1, 1); //���� �ٶ󺸴� ����
        }
        if (Input.GetKey(KeyCode.D)) //������ �̵�
        {
            inputRight = true;
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(1, 1, 1); //������ �ٶ󺸴� ����
        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping")) //����
        {
            inputJump = true;
            animator.SetBool("isJumping", true); // �÷��̾� ���� ���·� ��ȯ
        }

        //���� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ispossession = true;
        }

    }

    //Ű���� Ű ������ ���°�� ����
    void NonKeyBoard()
    {
        RaycastHit2D raycasHit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, new Vector2(0, -0.3f), Color.red);
        if (raycasHit.collider != null) animator.SetBool("isJumping", false);
        else animator.SetBool("isJumping", true);

        //�÷��̾� �⺻ ����
        inputIdle = true;
        animator.SetInteger(animationState, (int)States.Idle);
    }

    //���콺�� (�巡�� , Ŭ��) ������ ��
    void OnMouseClicked(Define.MouseEvent mouse)
    {
        // Ŭ�������̰� ���� �÷��̾ Attack ���°� �ƴ� ��
        if(mouse == Define.MouseEvent.Click)
        {
            //if (possession.GetClickedObject().layer == (int)Define.Layer.NPC)
            //{
            //    return;
            //}
            //else
            //{
                //���� ������ ����
                if (ispossession)
                {
                    // ��ȯ�Ǵ� ������Ʈ�� ���̴�?
                    if (possession.GetClickedObject().layer == (int)Define.Layer.Enemy
                        && possession.GetClickedObject().GetComponent<Stat>().Hp <= 0)
                    {
                        PlayerStat.PossessionClicked = true;

                        float currentHp = PlayerStat.Hp;
                        Debug.Log($"current player Hp : {PlayerStat.Hp}");
                        possession.Possession(possession.GetClickedObject());

                        animator.SetBool("isDie", true);
                        Managers.Input.KeyAction -= OnKeyBoard;
                        Managers.Input.NonKeyAction -= NonKeyBoard;
                        Managers.Input.MouseAction -= OnMouseClicked;



                        gameObject.layer = (int)Define.Layer.Enemy;
                        gameObject.tag = "Untagged";

                        Destroy(gameObject, 5f);
                    }
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        animator.SetTrigger("isAttack");

                    }

                }
            //}
        }
            
    }

    
}
