using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player)
        {
            if (PlayerStat.Hp > 0) PlayerStat.Hp -= 10;
        }

        if (collision.gameObject.layer == (int)Define.Layer.Enemy)
        {
            if (collision.GetComponent<Stat>().Hp > 0) collision.GetComponent<Stat>().Hp -= 5;
        }
    }
    private void Start()
    {
        StartCoroutine("destroy");
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
