using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    GameObject go;
    Vector3 missileDir;
    RaycastHit2D hit;

    private void Start()
    {
        missileDir = Vector3.right;
    }

    private void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Floor"));
        if (hit.collider != null) Destroy(gameObject);
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
                }
                Destroy(gameObject);
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
            }
            Destroy(gameObject);
        }
    }


}
