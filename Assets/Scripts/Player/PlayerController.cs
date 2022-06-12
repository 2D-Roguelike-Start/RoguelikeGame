using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _speed = 5.0f; //�÷��̾� �̵��ӵ�
    public float _jumpPower; //�÷��̾� ���� �Ŀ�

    private Rigidbody2D rigid; //�÷��̾�  rigid body
    private Animator animator; //�÷��̾� �ִϸ��̼�

    private bool isjumping; // ��ũ��Ʈ ���� ���� ���� ����
    private string animationState = "AnimationState";

    //�÷��̾� ���µ�
    enum States
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
    }

    void Init() //�÷��̾� ������Ʈ ����κ�
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
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

    void Update()
    {
        
    } 

    // collider�� �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        // �÷��̾� �Ʒ��� BlockingLayer �����ϴ� Raycast hit����
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 0.8f, LayerMask.GetMask("BlockingLayer"));
        

        // 6�� ���̾� (Floor) �̰ų� hit�� null�� �ƴҶ�
        if (collision.gameObject.layer== 6 /*&& hit.collider != null*/)
        {
            Debug.Log("collider!!");
            animator.SetBool("isJumping", false);

            isjumping = false;
        }
     }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger : {collision.gameObject}");
    }

    //Ű���忡 ������ ������ �� ����
    void OnKeyBoard()
    {
        if (Input.GetKey(KeyCode.A)) // ���� �̵�
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(-1, 1, 1); //���� �ٶ󺸴� ����
            transform.Translate(Vector3.left * Time.deltaTime * _speed);  //���� * �ӵ�
        }
        if (Input.GetKey(KeyCode.D)) //������ �̵�
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(1, 1, 1); //������ �ٶ󺸴� ����
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }
        if (Input.GetKey(KeyCode.Space)) //����
        {
            animator.SetBool("isJumping", true); // �÷��̾� ���� ���·� ��ȯ

            if (isjumping == false) // �������°� �ƴϾ��� ��
            {
                // ���� ���� ��
                rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                
                //��ũ��Ʈ ���� �������� ��ȯ
                isjumping = true;
            }
            else return;
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
        if(mouse == Define.MouseEvent.Click && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            animator.SetTrigger("isAttack");
    }

    
}
