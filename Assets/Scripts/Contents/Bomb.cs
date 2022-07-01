using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    GameObject go = null;

    private void Start()
    {
        StartCoroutine("bomb");
    }

    IEnumerator bomb()
    {
        yield return new WaitForSeconds(3f);
        go = Managers.Resource.Instantiate("Effect/Poison Explosion");
        go.transform.position = this.gameObject.transform.position;
        Destroy(gameObject);
    }
}
