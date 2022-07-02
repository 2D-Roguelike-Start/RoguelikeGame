using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossController_Spider : MonoBehaviour
{
    GameObject _lockTarget;
    public GameObject[] PatternObject;
    Rigidbody2D rigid;
    Animator anim;
    Stat stat;
    //TilemapCollider2D onofftile;
    public GameObject[] shootposition;

    string animationState = "AnimationState";
    int next = 0;
    float dir;
    bool ispoison = false;
    bool Poison = false;
    Vector3 Movedir;

    enum Patterns
    {
        rush,
        spiderWeb,
        poison,
    }

    private void Start()
    {
        _lockTarget = GameObject.FindWithTag("Player");
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponentInChildren<Animator>();
        stat = this.gameObject.GetComponent<Stat>();
        //onofftile = GameObject.Find("OnOffTile").GetAddComponent<TilemapCollider2D>();

        stat.Level = 1;
        stat.MaxHp = 50;
        stat.Hp = 50;
        stat.MoveSpeed = 5;
        stat.Attack = 10;

        StartCoroutine("rush");
    }

    void LookPlayer()
    {
        float scale = transform.localScale.y;
        dir = _lockTarget.transform.position.x > transform.position.x ? scale : scale * (-1); //방향
        Movedir = (_lockTarget.transform.position - transform.position).normalized * dir;
        transform.localScale = new Vector3(dir, scale, scale);
    }

    void LookDownPlayer()
    {
        float scale = transform.localScale.y;
        dir = _lockTarget.transform.position.x > transform.position.x ? scale * (-1) : scale; //방향
        transform.localScale = new Vector3(dir, scale, scale);
    }

    void LookPoisonPos()
    {
        float scale = transform.localScale.y;
        dir = -92f > transform.position.x ? scale : scale * (-1);
        transform.localScale = new Vector3(dir, scale, scale);
    }

    void nextPattern()
    {
        switch(next)
        {
            case 0: StartCoroutine(rush()); break;
            case 1: StartCoroutine(spiderWeb()); break;
            case 2: StartCoroutine(poison()); break;
        }
    }

    IEnumerator rush()
    {
        anim.SetInteger(animationState, 0);
        LookPlayer();
        Vector3 playerPosition = _lockTarget.transform.position;
        Debug.Log(playerPosition);
        yield return new WaitForSeconds(1);
        bool isCatching = false;
        float stop = 2f;

        while(!isCatching)
        {
            yield return new WaitForSeconds(0.1f);
            if (stat.MoveSpeed > rigid.velocity.x * dir)
            {
                anim.SetInteger(animationState, 1);
                //onofftile.enabled = false;
                rigid.AddForce(Movedir * dir * 450);
            }
            if (Mathf.Abs(transform.position.x - playerPosition.x) <= stop)
            {
                anim.SetInteger(animationState, 0);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                //onofftile.enabled = true;
                isCatching = true;
                break;
            }
        }
        if (stat.Hp / stat.MaxHp <= 0.3 && !Poison) ispoison = true;

        if (!ispoison)
        {
            if (Random.value > 0.7) // 30% 확률
                next = (int)Patterns.spiderWeb;
            else next = (int)Patterns.rush;
        }
        else
        {
            next = (int)Patterns.poison;
            Poison = true;
            ispoison = false;
        }
        
        yield return new WaitForSeconds(3);
        nextPattern();
    }

    IEnumerator spiderWeb()
    {
        ConstantForce2D Force = gameObject.GetAddComponent<ConstantForce2D>();
        anim.SetInteger(animationState, 0);
        transform.localScale = new Vector3(-2, 2, 1);
        yield return new WaitForSeconds(1.5f);
        #region 벽타기 구현부
        bool iscollider = false;
        while (!iscollider)
        {
            yield return new WaitForSeconds(0.1f);
            if (stat.MoveSpeed > rigid.velocity.x * dir)
            {
                //onofftile.enabled = false;
                anim.SetInteger(animationState, 1);
                rigid.AddForce(Vector3.left * 2 * 300);
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left, 2, LayerMask.GetMask("Floor"));
            if (hit.collider != null)
            {
                //onofftile.enabled = true;
                rigid.velocity = new Vector2(0, rigid.velocity.y); //멈추고
                Force.force = new Vector2(-50, 0); //중력값 x방향 왼쪽으로 바꾸고
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward); //회전시키고
                transform.localScale = new Vector3(-2, 2, 0); //바라보는 방향 바꾸고
                bool istouch = false;
                while (!istouch)
                {
                    yield return new WaitForSeconds(0.1f);
                    Force.force = new Vector2(-50, 60);
                    RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector3.up, 2, LayerMask.GetMask("Floor"));
                    if (rayhit.collider != null)
                    {
                        Debug.Log("isnt null");
                        transform.rotation = Quaternion.AngleAxis(-180, Vector3.forward); //다시 한번 회전, 아예 뒤집어버림
                        Force.force = new Vector2(60, 50);
                        bool isfound = false;
                        while (!isfound)
                        {
                            yield return new WaitForSeconds(0.1f);
                            
                            if (Mathf.Abs(transform.position.x - _lockTarget.transform.position.x) <= 2f)
                            {
                                anim.SetInteger(animationState, 0);
                                Force.force = new Vector2(0, 50);
                                rigid.velocity = new Vector2(0, rigid.velocity.y); //멈추고
                                isfound = true; break;
                            }
                        }
                        istouch = true; break;
                    }
                }
                iscollider = true; break;
            }
        }
        #endregion
        yield return new WaitForSeconds(1);
        bool isattack = false;
        int cnt = 0;
        while(!isattack)
        {
            anim.SetInteger(animationState, 1);
            yield return new WaitForSeconds(0.1f);
            LookDownPlayer();
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            if (stat.MoveSpeed > rigid.velocity.x * dir)
            {
                anim.SetInteger(animationState, 0);
                rigid.AddForce(transform.right * dir * 300);
                if (Mathf.Abs(transform.position.x - _lockTarget.transform.position.x) <= 2f)
                {
                    yield return new WaitForSeconds(0.1f);
                    anim.SetTrigger("isAttack");
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                    for (int i = 0; i < 3; i++)
                    {
                        switch (i)
                        {
                            case 0: shootposition[i].transform.rotation = Quaternion.AngleAxis(-120, Vector3.forward); break;
                            case 1: shootposition[i].transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward); break;
                            case 2: shootposition[i].transform.rotation = Quaternion.AngleAxis(-60, Vector3.forward); break;
                        }
                        Managers.Resource.Instantiate("Creature/Projectile/Poison Bomb").transform.position = shootposition[i].transform.position;
                        GameObject go = Managers.Resource.Instantiate("Creature/Projectile/Poison Missile");
                        go.transform.position = shootposition[i].transform.position;
                        go.transform.rotation = shootposition[i].transform.rotation;
                    }
                    yield return new WaitForSeconds(2f);
                    cnt++;
                }
            }
            if (Mathf.Abs(transform.position.x - _lockTarget.transform.position.x) >= 5f)
                LookDownPlayer();
            if (cnt >= 5) { isattack = true; break; }
        }
        Force.force = new Vector2(0, 0);
        anim.SetInteger(animationState, 1);
        yield return new WaitForSeconds(4);
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        next = (int)Patterns.rush;
        nextPattern();
    }

    IEnumerator poison()
    {
        Debug.Log("isPoison");
        anim.SetInteger(animationState, 0);
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        yield return new WaitForSeconds(2);
        LookPoisonPos();
        yield return new WaitForSeconds(1);
        bool isReady = false;
        while (!isReady)
        {
            yield return new WaitForSeconds(0.1f);
            if (stat.MoveSpeed > rigid.velocity.x * dir)
            {
                anim.SetInteger(animationState, 1);
                rigid.AddForce(transform.right * dir * 200);
            }
            if (Mathf.Abs(transform.position.x - (-92f)) <= 1.5f)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                anim.SetInteger(animationState, 0);
                isReady = true;
                break;
            }
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 50; i++)
        {
            switch (i % 4)
            {
                case 0: shootposition[i % 4].transform.rotation = Quaternion.AngleAxis(120, Vector3.forward); break;
                case 1: shootposition[i % 4].transform.rotation = Quaternion.AngleAxis(150, Vector3.forward); break;
                case 2: shootposition[i % 4].transform.rotation = Quaternion.AngleAxis(30, Vector3.forward); break;
                case 3: shootposition[i % 4].transform.rotation = Quaternion.AngleAxis(50, Vector3.forward); break;
            }
            GameObject go = Managers.Resource.Instantiate("Creature/Projectile/Poison Bomb");
            go.transform.position = shootposition[i % 4].transform.position;
            go.transform.rotation = shootposition[i % 4].transform.rotation;
            go.GetAddComponent<Rigidbody2D>().AddForce(go.transform.right * 1000f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(4);
        foreach (GameObject go in PatternObject) go.SetActive(true);

        next = (int)Patterns.rush;
        nextPattern();
    }

    private void Update()
    {
        Debug.Log($"Boss Hp : {stat.Hp}");
    }
}
