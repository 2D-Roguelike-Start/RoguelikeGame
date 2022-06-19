using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _speed = 5.0f; //플레이어 이동속도
    public float _jumpPower; //플레이어 점프 파워

    private Rigidbody2D rigid; //플레이어  rigid body
    private Animator animator; //플레이어 애니메이션

    private bool isjumping; // 스크립트 내부 점프 상태 제어
    private string animationState = "AnimationState";

    //플레이어 상태들
    enum States
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
    }

    void Init() //플레이어 컴포넌트 연결부분
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        Init();
        //Input Manager 이용 
        //키 감지
        Managers.Input.KeyAction -= OnKeyBoard; // 이미 작동된 실수 방지
        Managers.Input.KeyAction += OnKeyBoard;
        //키 없는 상태 감지
        Managers.Input.NonKeyAction -= NonKeyBoard;
        Managers.Input.NonKeyAction += NonKeyBoard;
        //마우스 드래그 , 클릭 감지 ( Define 클래스 참고 )
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
       
    }

    void Update()
    {
        
    } 

    // collider에 닿았을때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        // 플레이어 아래로 BlockingLayer 감지하는 Raycast hit정의
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 0.8f, LayerMask.GetMask("BlockingLayer"));
        

        // 6번 레이어 (Floor) 이거나 hit이 null이 아닐때
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

    //키보드에 뭔가가 들어왔을 때 실행
    void OnKeyBoard()
    {
        if (Input.GetKey(KeyCode.A)) // 왼쪽 이동
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(-1, 1, 1); //왼쪽 바라보는 방향
            transform.Translate(Vector3.left * Time.deltaTime * _speed);  //방향 * 속도
        }
        if (Input.GetKey(KeyCode.D)) //오른쪽 이동
        {
            animator.SetInteger(animationState, (int)States.Run);
            transform.localScale = new Vector3(1, 1, 1); //오른쪽 바라보는 방향
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }
        if (Input.GetKey(KeyCode.Space)) //점프
        {
            animator.SetBool("isJumping", true); // 플레이어 점프 상태로 전환

            if (isjumping == false) // 점프상태가 아니었을 때
            {
                // 위로 힘을 줌
                rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                
                //스크립트 내부 점프상태 전환
                isjumping = true;
            }
            else return;
        }
    }

    //키보드 키 감지가 없는경우 실행
    void NonKeyBoard()
    {
        //플레이어 기본 상태
        animator.SetInteger(animationState, (int)States.Idle);
    }

    //마우스에 (드래그 , 클릭) 들어왔을 때
    void OnMouseClicked(Define.MouseEvent mouse)
    {
        // 클릭상태이고 현재 플레이어가 Attack 상태가 아닐 때
        if(mouse == Define.MouseEvent.Click && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            animator.SetTrigger("isAttack");
    }

    
}
