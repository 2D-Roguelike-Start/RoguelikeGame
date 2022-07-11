using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    string parentTag;
    GameObject projectile;

    private void Start()
    {

    }

    public void Shoot()
    {
        if (gameObject.transform.parent != null)
            parentTag = gameObject.transform.parent.tag;
        switch (parentTag) 
        {
            case "Player":
                Debug.Log("Player shoot");
                switch (gameObject.transform.parent.name) 
                {
                    case "Skeleton_B":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Arrow"); break;
                    case "Skeleton_C":
                        projectile = Managers.Resource.Instantiate("Creature/Projectile/Magic Missile"); break;
                }

                if (gameObject.transform.parent.transform.localScale.x >= 0)
                {
                    projectile.transform.localScale = new Vector3(projectile.transform.localScale.x, projectile.transform.localScale.y, 1);
                    projectile.transform.position = gameObject.transform.position + new Vector3(1, 1.4f, 0);
                }
                else
                {
                    projectile.transform.localScale = new Vector3(-projectile.transform.localScale.x, projectile.transform.localScale.y, 1);
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
            case "Flying_Long":
                Debug.Log("Flying Long");
                break;

        }
    }
}
