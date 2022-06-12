using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;
    public Image nowHpBar;


    void Attacked()
    {
        attacked = true;
    }

    void NotAttacked()
    {
        attacked = false;
    }

    void SetatkSpeed(float speed)
    {

    }

    private void Start()
    {
        maxHp = 50;
        nowHp = 50;
        atkDmg = 10;
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            nowHp -= atkDmg;
            Debug.Log(nowHp);
            if(nowHp < 0)
            {
                Destroy(gameObject);
                Destroy(nowHpBar);
            }
        }
    }

    private void Update()
    {
        nowHpBar.transform.position = transform.position + new Vector3(0, 2.5f);
        nowHpBar.fillAmount = Mathf.Lerp(nowHpBar.fillAmount, (float)nowHp / (float)maxHp, Time.deltaTime);
    }

}
