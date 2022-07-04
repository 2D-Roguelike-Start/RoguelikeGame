using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D boxCol2D;
    private Rigidbody2D rigid; //플레이어  rigid body
    private Animator animator; //플레이어 애니메이션
    private PossessionController possession; // 플레이어 빙의
    private ActionController action;
    public EffectController effect;
    private GameObject target = null;

    private bool inputIdle = false;
    private bool inputRight = false;
    private bool inputLeft = false;
    private bool inputJump = false;
    private bool isdie = false;
    public bool ispossession; // 스크립트 내부 빙의 상태 제어
    private string animationState = "AnimationState";

    //플레이어 상태들
    enum States
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Skill = 3,
        Die = 4,
    }

    void Init() //플레이어 컴포넌트 연결부분
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
        if (PlayerStat.ShortOrLong)
        {
            target = GameObject.Find("Target");
            gameObject.AddComponent<ProjectileController>();
        }
        Init();
        action.PossessionTimerOff();
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

    private void Update()
    {
        if(target != null) target.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
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
                    effect.effect[0].transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                }
                else
                {
                    collision.transform.position += new Vector3(-0.3f, 0, 0);
                    effect.effect[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                }

                effect.EffectOn(collision.transform, "Slash 3");
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

    //키보드에 뭔가가 들어왔을 때 실행
    public void OnKeyBoard()
    {
        RaycastHit2D raycasHit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, new Vector2(0, -0.3f), Color.red);
        if (raycasHit.collider != null)
            animator.SetBool("isJumping", false);
        else animator.SetBool("isJumping", true);

        if(PlayerStat.ShortOrLong)
        {
            if (transform.position.x >= Camera.main.ScreenToWorldPoint(Input.mousePosition).x) transform.localScale = new Vector3(-1, 1, 1);
            else transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKey(KeyCode.A)) // 왼쪽 이동
        {
            inputLeft = true;
            animator.SetInteger(animationState, (int)States.Run);
           
            if(!PlayerStat.ShortOrLong) transform.localScale = new Vector3(-1, 1, 1); //왼쪽 바라보는 방향
        }
        if (Input.GetKey(KeyCode.D)) //오른쪽 이동
        {
            inputRight = true;
            animator.SetInteger(animationState, (int)States.Run);

            if (!PlayerStat.ShortOrLong) transform.localScale = new Vector3(1, 1, 1); //오른쪽 바라보는 방향

        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping")) //점프
        {
            inputJump = true;
            animator.SetBool("isJumping", true); // 플레이어 점프 상태로 전환
        }

        //빙의 상태
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ispossession = true;
        }

    }

    //키보드 키 감지가 없는경우 실행
    void NonKeyBoard()
    {
        RaycastHit2D raycasHit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, new Vector2(0, -0.3f), Color.red);
        if (raycasHit.collider != null) animator.SetBool("isJumping", false);
        else animator.SetBool("isJumping", true);

        //플레이어 기본 상태
        inputIdle = true;
        animator.SetInteger(animationState, (int)States.Idle);

        if (PlayerStat.ShortOrLong)
        {
            if (transform.position.x >= Camera.main.ScreenToWorldPoint(Input.mousePosition).x) transform.localScale = new Vector3(-1, 1, 1);
            else transform.localScale = new Vector3(1, 1, 1);
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

    //마우스에 (드래그 , 클릭) 들어왔을 때
    public void OnMouseClicked(Define.MouseEvent mouse)
    {
        // 클릭상태이고 현재 플레이어가 Attack 상태가 아닐 때
        if(mouse == Define.MouseEvent.Click)
        {
            //빙의 가능한 상태
            if (ispossession)
            {
                if (possession.GetClickedObject() == null) return;
                // 반환되는 오브젝트가 적이다?
                if (possession.GetClickedObject().layer == (int)Define.Layer.Enemy
                    && possession.GetClickedObject().GetComponent<Stat>().Hp <= 0)
                {
                    PlayerStat.PossessionClicked = true;

                    if (possession.GetClickedObject().tag == "Landing_Long" || possession.GetClickedObject().tag == "Flying_Long") PlayerStat.ShortOrLong = true;
                    else PlayerStat.ShortOrLong = false;
                    if (possession.GetClickedObject().tag == "Flying_Short" || possession.GetClickedObject().tag == "Flying_Long") PlayerStat.LandOrFly = true;
                    else PlayerStat.LandOrFly = false;

                    if (PlayerStat.ShortOrLong)
                    {
                        GameObject go = GameObject.Find("Target");
                        if(go == null)
                            target = Managers.Resource.Instantiate("UI/Target");
                    }
                    else Managers.Resource.Destroy(GameObject.Find("Target"));
                            
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
        }       
    }
}
