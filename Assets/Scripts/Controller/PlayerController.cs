using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStat playerstat;

    public float _jumpPower; //�÷��̾� ���� �Ŀ�

    private BoxCollider2D boxCol2D;
    private Rigidbody2D rigid; //�÷��̾�  rigid body
    private Animator animator; //�÷��̾� �ִϸ��̼�
    private PossessionController possession; // �÷��̾� ����

    private bool inputIdle = false;
    private bool inputRight = false;
    private bool inputLeft = false;
    private bool inputJump = false;
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
        _jumpPower = 100;

        boxCol2D = GetComponent<BoxCollider2D>();
        if (PossessionCount.PosCount == 0)
        {
            playerstat = GetComponent<PlayerStat>();
            playerstat.Hp = 100;
        }
        else
        {
            playerstat = GetComponent<PlayerStat>();
            Debug.Log($"first Start : current Hp : {PossessionCount.currentHp} and {playerstat.Hp}");
            playerstat.Hp = PossessionCount.currentHp;
            Debug.Log($"second Start : current Hp : {PossessionCount.currentHp} and {playerstat.Hp}");

        }
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        possession = GetComponent<PossessionController>();
    }

    void Start()
    {
        Init();
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
            rigid.velocity = new Vector2(playerstat.MoveSpeed, fallSpeed);
        }
        if(inputLeft)
        {
            inputLeft = false;
            rigid.velocity = new Vector2(-playerstat.MoveSpeed, fallSpeed);
        }
        if (inputJump)
        {
            inputJump = false;
            rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

    }

    // collider�� �������
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(ispossession)
    //        Debug.Log("OnCollisionEnter2D");

    //    if (collision.gameObject.layer == (int)Define.Layer.Enemy)
    //        Debug.Log($"{collision.gameObject.name}");
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger : {collision.gameObject}");
        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
            collision.GetComponentInParent<Stat>().Hp -= playerstat.Attack;

        if (playerstat.Hp <= 0)
        {
            animator.SetTrigger("isDie");
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

        ispossession = false;
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
        //���� �׽�Ʈ
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
            //���� ������ ����
            if (ispossession)
            {
                // ��ȯ�Ǵ� ������Ʈ�� ���̴�?
                if(possession.GetClickedObject().layer == (int)Define.Layer.Enemy)
                {
                    float currentHp = playerstat.Hp;
                    Debug.Log($"current player Hp : { playerstat.Hp}");
                    possession.Possession(possession.GetClickedObject());

                    animator.SetTrigger("isDie");
                    Managers.Input.KeyAction -= OnKeyBoard;
                    Managers.Input.NonKeyAction -= NonKeyBoard;
                    Managers.Input.MouseAction -= OnMouseClicked;

                    gameObject.layer = (int)Define.Layer.Enemy;
                    gameObject.tag = "Untagged";
                    PossessionCount.PosCount++;
                    PossessionCount.currentHp = playerstat.Hp;

                    Destroy(gameObject, 3f);
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    animator.SetTrigger("isAttack");
                    Debug.Log("Attack On!!");
                }
                    
            }
        }
            
    }

    
}
