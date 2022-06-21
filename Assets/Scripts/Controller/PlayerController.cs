using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStat playerstat;

    public float _jumpPower; //�÷��̾� ���� �Ŀ�

    private Rigidbody2D rigid; //�÷��̾�  rigid body
    private Animator animator; //�÷��̾� �ִϸ��̼�
    private PossessionController possession; // �÷��̾� ����

    private bool isjumping; // ��ũ��Ʈ ���� ���� ���� ����
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

        playerstat = GetComponent<PlayerStat>();
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
        if(rigid.velocity.y < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Floor"));

            if (hit.collider != null)
            {
                if (hit.distance < 0.2f)
                {
                    isjumping = false;
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }

    // collider�� �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ispossession)
            Debug.Log("OnCollisionEnter2D");

        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
            Debug.Log($"{collision.gameObject.name}");
     }

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
        ispossession = false;
        if (Input.GetKey(KeyCode.A)) // ���� �̵�
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(-1, 1, 1); //���� �ٶ󺸴� ����
            transform.Translate(Vector3.left * Time.deltaTime * playerstat.MoveSpeed);  //���� * �ӵ�
        }
        if (Input.GetKey(KeyCode.D)) //������ �̵�
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(1, 1, 1); //������ �ٶ󺸴� ����
            transform.Translate(Vector3.right * Time.deltaTime * playerstat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.Space)) //����
        {
            if (!isjumping) // �������°� �ƴϾ��� ��
            {
                //��ũ��Ʈ ���� �������� ��ȯ
                isjumping = true;
                // ���� ���� ��
                rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            }

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
        //�÷��̾� �⺻ ����
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
                    possession.Possession(possession.GetClickedObject());

                    animator.SetTrigger("isDie");
                    Managers.Input.KeyAction -= OnKeyBoard;
                    Managers.Input.NonKeyAction -= NonKeyBoard;
                    Managers.Input.MouseAction -= OnMouseClicked;

                    gameObject.layer = (int)Define.Layer.Enemy;
                    gameObject.tag = "Untagged";

                    Destroy(gameObject, 3f);
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    animator.SetTrigger("isAttack");
            }
        }
            
    }

    
}
