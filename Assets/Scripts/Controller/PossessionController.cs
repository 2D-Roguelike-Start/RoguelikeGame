using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
    public GameObject GetClickedObject()
    {
        GameObject target = null;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 5);

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            target = hit.collider.gameObject;
        }

        return target;
    }

    public GameObject GetTouchedObject()
    {
        GameObject target = null;
        if(Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 5);

                    if (hit.collider != null)
                    {
                        Debug.Log(hit.collider.name);
                        target = hit.collider.gameObject;
                    }
                    break;
                }
            }
        }
        return target;
    }

    public void Possession(GameObject go)
    {
        go.gameObject.tag = "Player";
        go.gameObject.layer = (int)Define.Layer.Player;

        if (go.GetComponent<MonsterControllerShort>() != null) Destroy(go.GetComponent<MonsterControllerShort>());
        if (go.GetComponent<MonsterControllerLong>() != null) Destroy(go.GetComponent<MonsterControllerLong>());
        Destroy(go.GetComponent<Stat>());

        go.AddComponent<PlayerController>();
        go.AddComponent<PossessionController>();
        go.GetComponentInChildren<Animator>().speed = PlayerStat.AttackSpeed;
    }
}
