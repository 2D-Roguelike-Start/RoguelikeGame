using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D boxCol2D;
    private Rigidbody2D rigid; //�÷��̾�  rigid body
    public Animator animator; //�÷��̾� �ִϸ��̼�
    private PossessionController possession; // �÷��̾� ����
    private ActionController action;
    public EffectController effect;
    //private GameObject target = null;

    private bool inputIdle = false;
    public bool inputRight = false;
    public bool inputLeft = false;
    public bool inputJump = false;
    private bool isdie = false;
    public bool isflying = false;
    public bool ispossession; // ��ũ��Ʈ ���� ���� ���� ����
    private string animationState = "AnimationState";
    public string moveInfo = null;
    public string possInfo = null;
    public string jumpInfo = null;

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

        string name = gameObject.name;
        int index = name.LastIndexOf('_');
        if (index >= 0)
            name = name.Substring(0, index);

        if (name == "Bat")
        {
            isflying = true;
        }
    }

    void Start()
    {
        //if (PlayerStat.ShortOrLong)
        //{
        //    target = GameObject.Find("Target");
        //    //gameObject.AddComponent<ProjectileController>();
        //}
        
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
        //Managers.Input.MouseAction -= OnMouseClicked;
        //Managers.Input.MouseAction += OnMouseClicked;
        //�ȵ���̵�� ��ġ
        Managers.Input.TouchAction -= OnTouch;
        Managers.Input.TouchAction += OnTouch;
    }

    private void Update()
    {
        //�������
        //if(target != null) target.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
        //�ȵ���̵��
        //if(target != null || Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);
        //    target.transform.position = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 1);
        //}

        if (ispossession) Debug.Log("isPossession true");
    }

    private void FixedUpdate()
    {
        float verticalSpeed = rigid.velocity.y;
        float horizonSpeed = rigid.velocity.x;
        
        if (inputIdle)
        {
            inputIdle = false;
            rigid.velocity = new Vector2(0, verticalSpeed);
        }
        if (inputRight)
        {
            inputRight = false;
            rigid.velocity = new Vector2(PlayerStat.MoveSpeed, verticalSpeed);
        }
        if(inputLeft)
        {
            inputLeft = false;
            rigid.velocity = new Vector2(-PlayerStat.MoveSpeed, verticalSpeed);
        }
        if (inputJump)
        {
            inputJump = false;
            if(isflying)
            {
                if (verticalSpeed <= PlayerStat.MoveSpeed)
                    rigid.AddForce(Vector2.up * PlayerStat.JumpPower, ForceMode2D.Impulse);
            }
            else rigid.AddForce(Vector2.up * PlayerStat.JumpPower, ForceMode2D.Impulse);
        }
        if (isdie)
        {
            isdie = false;
            rigid.velocity = new Vector2(0, verticalSpeed);
        }

    }

    //�ݶ��̴��� enemy ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���Ϳ� ���� by �÷��̾� �����ݶ��̴�
        GameObject go = collision.gameObject;

        if (go.layer == (int)Define.Layer.Enemy)
        {
            Debug.Log($"Player-> Enemy Trigger : {collision.gameObject}");
            if (go.GetComponent<Stat>().Hp > 0)
            {
                if (transform.position.x <= go.transform.position.x)
                {
                    go.transform.position += new Vector3(0.3f, 0, 0);
                    //effect.EffectOn(collision.transform, "Slash 3").transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                    //effect.effect[0].transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                }
                else
                {
                    go.transform.position += new Vector3(-0.3f, 0, 0);
                    //effect.EffectOn(collision.transform, "Slash 3").transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    //effect.effect[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                }

                //effect.EffectOn(collision.transform, "Slash 3");
                if(go.GetComponent<Stat>().Hp >= 0)
                {
                    go.GetComponent<Stat>().Hp -= PlayerStat.Attack;
                }
            }
        }
        else if(collision.gameObject.layer == 4)
        {
            PlayerStat.Hp = 0;
            Managers.UI.ShowPopupUI<UI_DeadPopup>();
            Managers.Sound.Clear();
            Managers.Sound.Play(Define.Sound.Bgm, "Sound_Die", 0.2f);
        } //Water 
       
        if (PlayerStat.Hp <= 0)
        {
            isdie = true;
            animator.SetBool("isDie", true);
            Managers.UI.ShowPopupUI<UI_DeadPopup>();
            Managers.Sound.Clear();
            Managers.Sound.Play(Define.Sound.Bgm, "Sound_Die", 0.2f);
            Managers.Input.KeyAction -= OnKeyBoard;
            Managers.Input.NonKeyAction -= NonKeyBoard;
            //Managers.Input.MouseAction -= OnMouseClicked;
            Managers.Input.TouchAction -= OnTouch;
            return;
        }
    }

    //Ű���忡 ������ ������ �� ����
    public void OnKeyBoard()
    {
        RaycastHit2D raycasHit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, new Vector2(0, -0.3f), Color.red);
        if (raycasHit.collider != null)
            animator.SetBool("isJumping", false);
        else animator.SetBool("isJumping", true);

        //if(PlayerStat.ShortOrLong)
        //{
        //    if (transform.position.x >= Camera.main.ScreenToWorldPoint(Input.mousePosition).x) transform.localScale = new Vector3(-1, 1, 1);
        //    else transform.localScale = new Vector3(1, 1, 1);
        //}

        if (Input.GetKey(KeyCode.A) || moveInfo == "LEFT") // ���� �̵�
        {
            inputLeft = true;
            animator.SetInteger(animationState, (int)States.Run);
           
            if(!PlayerStat.ShortOrLong) transform.localScale = new Vector3(-1, 1, 1); //���� �ٶ󺸴� ����
        }
        if (Input.GetKey(KeyCode.D) || moveInfo == "RIGHT") //������ �̵�
        {
            inputRight = true;
            animator.SetInteger(animationState, (int)States.Run);

            if (!PlayerStat.ShortOrLong) transform.localScale = new Vector3(1, 1, 1); //������ �ٶ󺸴� ����

        }
        if(!isflying)
        {
            if ((Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping")) || jumpInfo == "JUMP") //����
            {
                inputJump = true;
                animator.SetBool("isJumping", true); // �÷��̾� ���� ���·� ��ȯ
                jumpInfo = null;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) || jumpInfo == "JUMPISFLYING")// && !animator.GetBool("isJumping")) //����
            {
                //�ȵ���̵�
                PlayerStat.JumpPower = 100f;
                //PC
                //PlayerStat.JumpPower = 20f;
                inputJump = true;
                animator.SetBool("isJumping", true); // �÷��̾� ���� ���·� ��ȯ
                jumpInfo = null;
            }
        }

        //���� ����
        if (Input.GetKey(KeyCode.LeftShift) || possInfo == "Possesion")
        {
            ispossession = true;
            Debug.Log($"Android callback : {ispossession}");
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

        //if (PlayerStat.ShortOrLong)
        //{
        //    if (transform.position.x >= Camera.main.ScreenToWorldPoint(Input.mousePosition).x) transform.localScale = new Vector3(-1, 1, 1);
        //    else transform.localScale = new Vector3(1, 1, 1);
        //}

        if (PlayerStat.Hp <= 0)
        {
            isdie = true;
            animator.SetBool("isDie", true);
            Managers.UI.ShowPopupUI<UI_DeadPopup>();
            Managers.Sound.Clear();
            Managers.Sound.Play(Define.Sound.Bgm, "Sound_Die", 0.2f);
            Managers.Input.KeyAction -= OnKeyBoard;
            Managers.Input.NonKeyAction -= NonKeyBoard;
            //Managers.Input.MouseAction -= OnMouseClicked;
            Managers.Input.TouchAction -= OnTouch;
            return;
        }
    }

    #region ��ǻ��
    //���콺�� (�巡�� , Ŭ��) ������ ��
    //public void OnMouseClicked(Define.MouseEvent mouse)
    //{
    //    // Ŭ�������̰� ���� �÷��̾ Attack ���°� �ƴ� ��
    //    if(mouse == Define.MouseEvent.Click)
    //    {
    //        //���� ������ ����
    //        if (ispossession)
    //        {
    //            if (possession.GetClickedObject() == null) return;
    //            // ��ȯ�Ǵ� ������Ʈ�� ���̴�?
    //            if (possession.GetClickedObject().layer == (int)Define.Layer.Enemy
    //                && possession.GetClickedObject().GetComponent<Stat>().Hp <= 0)
    //            {
    //                PlayerStat.PossessionClicked = true;

    //                if (possession.GetClickedObject().tag == "Landing_Long" || possession.GetClickedObject().tag == "Flying_Long") PlayerStat.ShortOrLong = true;
    //                else PlayerStat.ShortOrLong = false;
    //                if (possession.GetClickedObject().tag == "Flying_Short" || possession.GetClickedObject().tag == "Flying_Long") PlayerStat.LandOrFly = true;
    //                else PlayerStat.LandOrFly = false;

    //                //if (PlayerStat.ShortOrLong)
    //                //{
    //                //    GameObject go = GameObject.Find("Target");
    //                //    if(go == null)
    //                //        target = Managers.Resource.Instantiate("UI/Target");
    //                //}
    //                //else Managers.Resource.Destroy(GameObject.Find("Target"));
                            
    //                float currentHp = PlayerStat.Hp;
    //                Debug.Log($"current player Hp : {PlayerStat.Hp}");
    //                possession.Possession(possession.GetClickedObject());

    //                animator.SetBool("isDie", true);
    //                gameObject.layer = (int)Define.Layer.Enemy;
    //                gameObject.tag = "Untagged";
    //                Managers.Input.KeyAction -= OnKeyBoard;
    //                Managers.Input.NonKeyAction -= NonKeyBoard;
    //                Managers.Input.MouseAction -= OnMouseClicked;
    //                Destroy(gameObject, 5f);
    //            }
    //        }
    //        else
    //        {
    //            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
    //            {
    //                //PC
    //                //switch (gameObject.name) 
    //                //{
    //                //    case "Slime_A" :
    //                //        Managers.Sound.Play(Define.Sound.Effect, "Sound_Slime_A_Hit", UI_Setting_SoundPopup.EffectSound);
    //                //        break;
    //                //    case "Bat_A":
    //                //        GameObject go = Managers.Resource.Instantiate("Creature/Projectile/BatFireball");
    //                //        go.layer = (int)Define.Layer.Projectile_Player;

    //                //        if (gameObject.transform.localScale.x >= 0) go.transform.position = gameObject.transform.position + new Vector3(1, 0, 0);
    //                //        else go.transform.position = gameObject.transform.position + new Vector3(-1, 0, 0);

    //                //        Vector3 PlayerShotdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10)) - this.transform.position;
    //                //        float angle = Mathf.Atan2(PlayerShotdir.y, PlayerShotdir.x) * Mathf.Rad2Deg;

    //                //        go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //                //        break;

    //                //}
    //                //animator.SetTrigger("isAttack");

    //            }
    //        }
    //    }       
    //}
#endregion
    public void OnTouch()
    {
        if (ispossession)
        {
            Debug.Log($"NAME : {possession.GetTouchedObject().name} Hp : {possession.GetTouchedObject().GetComponent<Stat>().Hp}");
            if (possession.GetTouchedObject() == null) return;
            // ��ȯ�Ǵ� ������Ʈ�� ���̴�?
            if (possession.GetTouchedObject().layer == (int)Define.Layer.Enemy
                && possession.GetTouchedObject().GetComponentInChildren<ActionController>().Timer.activeSelf == true)
            {
                PlayerStat.PossessionClicked = true;

                //if (possession.GetTouchedObject().tag == "Landing_Long" || possession.GetTouchedObject().tag == "Flying_Long") { }// PlayerStat.ShortOrLong = true;
                //else PlayerStat.ShortOrLong = false;
                if (possession.GetTouchedObject().tag == "Flying_Short" || possession.GetTouchedObject().tag == "Flying_Long") PlayerStat.LandOrFly = true;
                else PlayerStat.LandOrFly = false;

                //if (PlayerStat.ShortOrLong)
                //{
                //    GameObject go = GameObject.Find("Target");
                //    if (go == null)
                //        target = Managers.Resource.Instantiate("UI/Target");
                //}
                //else Managers.Resource.Destroy(GameObject.Find("Target"));

                float currentHp = PlayerStat.Hp;
                Debug.Log($"current player Hp : {PlayerStat.Hp}");
                possession.Possession(possession.GetTouchedObject());

                animator.SetBool("isDie", true);
                gameObject.layer = (int)Define.Layer.Enemy;
                gameObject.tag = "Untagged";
                Managers.Input.KeyAction -= OnKeyBoard;
                Managers.Input.NonKeyAction -= NonKeyBoard;
                Managers.Input.TouchAction -= OnTouch;
                Destroy(gameObject, 5f);
            }
        }
    }
}
