using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    string parentName;
    GameObject projectile;
    EffectController particle;

    private void Start()
    {
        if (gameObject.transform.parent != null)
            parentName = gameObject.transform.parent.tag;
        particle = GameObject.Find("Effect").GetComponent<EffectController>();
    }

    public void Shoot()
    {
        switch (parentName) 
        {
            case "Player":
                switch (gameObject.transform.parent.name) 
                {
                    case "Skeleton_B":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Arrow"); break;
                    case "Skeleton_C":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Magic Missile"); break;
                }

                if (gameObject.transform.parent.transform.localScale.x >= 0)
                {
                    projectile.transform.localScale = new Vector3(1, 1, 1);
                    projectile.transform.position = gameObject.transform.position + new Vector3(1, 1.4f, 0);
                }
                else
                {
                    projectile.transform.localScale = new Vector3(-1, 1, 1);
                    projectile.transform.position = gameObject.transform.position + new Vector3(-1, 1.4f, 0);
                }
                break;
            case "Landing_Long":
                switch (gameObject.transform.parent.name)
                {
                    case "Skeleton_B":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Arrow"); break;
                    case "Skeleton_C":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Magic Missile"); break;
                }

                if (gameObject.transform.parent.transform.localScale.x >= 0)
                {
                    projectile.transform.localScale = new Vector3(1, 1, 1);
                    projectile.transform.position = gameObject.transform.parent.transform.position + new Vector3(1, 1.4f, 0);
                }
                else
                {
                    projectile.transform.localScale = new Vector3(-1, 1, 1);
                    projectile.transform.position = gameObject.transform.parent.transform.position + new Vector3(-1, 1.4f, 0);
                }
                break;
           

        }
    }
}
