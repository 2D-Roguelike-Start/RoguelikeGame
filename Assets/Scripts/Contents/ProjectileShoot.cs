using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private int _attack = 10;
    private float distance = 0.2f;
    private float angle;
    private static string Who;

    private RaycastHit2D hit;
    private RaycastHit2D ray;
    private Vector3 PlayerShotdir = Vector3.zero;
    private Vector3 EnemyShotdir = Vector3.zero;

    GameObject player;
    EffectController ef;

    string Effect;
    
    private void Start()
    {
        EffectSelect();
        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.transform.localScale.x >= 0) hit = Physics2D.Raycast(transform.position, Vector3.left, 2f, LayerMask.GetMask("Player", "Enemy"));
        else hit = Physics2D.Raycast(transform.position, Vector3.right, 2f, LayerMask.GetMask("Player", "Enemy"));

        switch(hit.collider.gameObject.layer)
        {
            case (int)Define.Layer.Player:
                PlayerShotdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10)) - this.transform.position;
                angle = Mathf.Atan2(PlayerShotdir.y, PlayerShotdir.x) * Mathf.Rad2Deg;

                if (this.transform.localScale == new Vector3(1, 1, 1)) transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                else transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                Who = "Player"; break;

            case (int)Define.Layer.Enemy:
                EnemyShotdir = player.transform.position - this.transform.position;
                angle = Mathf.Atan2(EnemyShotdir.y, EnemyShotdir.x) * Mathf.Rad2Deg;

                if (this.transform.localScale == new Vector3(1, 1, 1)) transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                else transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);

                Who = "Enemy"; break;
        }
        Invoke("DestroyShot", 2);
    }

    void Update()
    {
        switch (Who)
        {
            case "Player":
                PlayerShot(); break;
            case "Enemy":
                Shot(); break;
        }
    }

    void PlayerShot()
    {
        if (gameObject.transform.localScale.x >= 0)
        {
            ray = Physics2D.Raycast(transform.position, Vector3.right, distance, LayerMask.GetMask("Enemy", "Floor"));
            gameObject.transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        else
        {
            ray = Physics2D.Raycast(transform.position, Vector3.left, distance, LayerMask.GetMask("Enemy", "Floor"));
            gameObject.transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (ray.collider == null) Invoke("DestroyShot", 2);
        else
        {
            if (ray.collider.gameObject.layer == (int)Define.Layer.Enemy &&
                ray.collider.GetComponent<Stat>().Hp > 0)
            {
                ef.EffectOn(ray.collider.transform, Effect);
                ray.collider.GetComponent<Stat>().Hp -= _attack;
                Destroy(gameObject);
            }
            if (ray.collider.gameObject.layer == (int)Define.Layer.Floor)
                Destroy(gameObject);
        }
    }

    void Shot()
    {
        if (gameObject.transform.localScale.x >= 0)
        {
            gameObject.transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime);
            ray = Physics2D.Raycast(transform.position, Vector3.right, distance, LayerMask.GetMask("Player", "Floor"));
        }
        else
        {
            gameObject.transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime);
            ray = Physics2D.Raycast(transform.position, Vector3.left, distance, LayerMask.GetMask("Player", "Floor"));
        }
        
        if (ray.collider == null) Invoke("DestroyShot", 2);
        else
        {
            if (ray.collider.gameObject.layer == (int)Define.Layer.Player &&
                PlayerStat.Hp > 0)
            {
                ef.EffectOn(ray.collider.transform, Effect);
                PlayerStat.Hp -= _attack;
                Destroy(gameObject);
            }
            if (ray.collider.gameObject.layer == (int)Define.Layer.Floor)
                Destroy(gameObject);
        }
    }

    public void DestroyShot()
    {
       Destroy(gameObject);
    }

    public void EffectSelect()
    {
        ef = GameObject.Find("Effect").GetAddComponent<EffectController>();
        if (gameObject.name == "Arrow") Effect = "Blood";
        else if (gameObject.name == "Magic Missile") Effect = "Magic Missile explosion";
    }
        
}
