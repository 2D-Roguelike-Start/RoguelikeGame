using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bomb_Type
{
    DarkBall,
    End
}

public class Bomb : MonoBehaviour
{
    GameObject go = null;
    public Bomb_Type Type = Bomb_Type.End;

    //DarkBall
    SpriteRenderer BallSprite;
    Color BallColor;

    private void Start()
    {
        StartCoroutine("bomb");
        //test
    }

    private void Update()
    {
        if(Type == Bomb_Type.DarkBall)
        {
            if (BallSprite == null)
            {
                BallSprite = GetComponent<SpriteRenderer>();
                BallColor = BallSprite.color;
            }

            BallColor.a += 0.003f;
            BallSprite.color = BallColor;
        }
    }

    IEnumerator bomb()
    {
        yield return new WaitForSeconds(3f);

        go = Managers.Resource.Instantiate("Effect/Poison Explosion");
        go.transform.position = this.gameObject.transform.position;

        if (Type == Bomb_Type.DarkBall)
        {
            var main = go.GetComponent<ParticleSystem>().main;
            main.startColor = new Color(1, 0, 1, 1);
        }

        Destroy(gameObject);        
    }
}
