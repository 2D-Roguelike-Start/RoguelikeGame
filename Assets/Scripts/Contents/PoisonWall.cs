using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWall : MonoBehaviour
{
    float currentDurationTime = 0;
    float damageTime = 1.5f;
    bool isPoison = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (isPoison)
            ElapseTime();
    }

    void ElapseTime()
    {
        if (currentDurationTime <= 0) PoisonOff();

        if (currentDurationTime > 0) currentDurationTime -= Time.deltaTime;

    }

    void PoisonOff()
    {
        isPoison = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPoison && collision.gameObject.layer == (int)Define.Layer.Player)
        {
            if(currentDurationTime <= 0)
            {
                PlayerStat.Hp -= 5;
                currentDurationTime = damageTime;
                isPoison = true;
            }
        }
    }


}
