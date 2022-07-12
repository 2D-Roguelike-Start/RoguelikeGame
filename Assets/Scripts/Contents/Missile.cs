using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    GameObject go;
    Vector3 missileDir;
    RaycastHit2D hit;

    public string Effect_Name = "Poison Explosion";
    public bool IsDestroy = true; //맞으면 삭제되나요?
    public float Missile_launch_Time = 0;
    bool UpdateCheck = false;


    private void Start()
    {
        missileDir = Vector3.right;
        StartCoroutine(launch_Time());
    }

    private void Update()
    {
        if (!UpdateCheck)
            return;

        hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Floor"));
        if (hit.collider != null && IsDestroy) Destroy(gameObject); 

        gameObject.transform.Translate(missileDir * 10 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit Fireball 1");
        if (collision.gameObject.layer == (int)Define.Layer.Player)// && gameObject.layer == (int)Define.Layer.Enemy)
        {
            Debug.Log("Hit Fireball 2");
            if(gameObject.layer == (int)Define.Layer.Projectile_Enemy)
            {
                Debug.Log("Hit Fireball 3");
                switch (gameObject.name)
                {
                    case "Poison Missile":
                        go = Managers.Resource.Instantiate("Effect/Poison Explosion");
                        go.transform.position = this.gameObject.transform.position;
                        break;
                    case "BatFireball":
                        go = Managers.Resource.Instantiate("Effect/Explosion");
                        go.transform.position = this.gameObject.transform.position;
                        break;
                    default:
                        go = Managers.Resource.Instantiate("Effect/" + Effect_Name);
                        go.transform.position = this.gameObject.transform.position;
                        break;
                }
                if (IsDestroy) Destroy(gameObject);
            }
            
        }

        if (collision.gameObject.layer == (int)Define.Layer.Enemy && gameObject.layer == (int)Define.Layer.Projectile_Player)
        {
            switch (gameObject.name)
            {
                case "Poison Missile":
                    go = Managers.Resource.Instantiate("Effect/Poison Explosion");
                    go.transform.position = this.gameObject.transform.position;
                    break;
                case "BatFireball":
                    go = Managers.Resource.Instantiate("Effect/Explosion");
                    go.transform.position = this.gameObject.transform.position;
                    break;
                default:
                    go = Managers.Resource.Instantiate("Effect/" + Effect_Name);
                    go.transform.position = this.gameObject.transform.position;
                    break;
            }
            if (IsDestroy) Destroy(gameObject);
        }
    }

    IEnumerator launch_Time()
    {
        //발사 시간 정해놨으면 위험지역 표시해줌.
        if (Missile_launch_Time != 0)
        {
            //GameObject go = gameObject.AddComponent<Sprite>();
        }
        yield return new WaitForSeconds(Missile_launch_Time);
        UpdateCheck = true;
    }
}
