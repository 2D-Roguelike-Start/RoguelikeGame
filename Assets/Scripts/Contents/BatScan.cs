using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScan : MonoBehaviour
{
    float spin = 360;
    float down = -1.5f;
    bool init = false;
    public float height = 5f;
    public float distance = 3f;
    public GameObject animator;
    public GameObject[] shootPos;
    GameObject player;

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(Mathf.Abs(gameObject.transform.parent.transform.position.x - player.transform.position.x) <= distance 
            && Mathf.Abs(gameObject.transform.parent.transform.position.y - player.transform.position.y) <= height && !init)
        {
            if (gameObject.transform.parent.transform.position.x <= player.transform.position.x)
            {
                gameObject.transform.parent.transform.position += new Vector3(0, down, 0);
                gameObject.transform.parent.rotation = Quaternion.AngleAxis(spin, Vector3.forward);
            }
            else
            {
                gameObject.transform.parent.transform.position += new Vector3(0, down, 0);
                gameObject.transform.parent.rotation = Quaternion.AngleAxis(spin, Vector3.forward);
                gameObject.transform.parent.transform.localScale = new Vector3(-1, 1, 1);
            }
            animator.GetAddComponent<Animator>().enabled = true;
            init = true;
            gameObject.transform.parent.gameObject.GetAddComponent<MonsterControllerShort>();
        }
    }

}
