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
        hit = Physics2D.Raycast(transform.position, Vector2.right, 1, LayerMask.GetMask("Floor"));
        if (hit.collider != null) Destroy(gameObject);
        gameObject.transform.Translate(missileDir * 10 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player) {
            go = Managers.Resource.Instantiate("Effect/Poison Explosion");
            go.transform.position = this.gameObject.transform.position;
            Destroy(gameObject); 
        }
    }


}
