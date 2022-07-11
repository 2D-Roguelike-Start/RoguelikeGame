using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonExplosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerStat.Hp >= 0) PlayerStat.Hp -= 10;
        }
    }
    private void Start()
    {
        StartCoroutine("destroy");
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
