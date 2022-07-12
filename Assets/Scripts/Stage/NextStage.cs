using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public enum NextMapName 
    {
        Stage1_Tutorial,
        Stage1_1,
        Stage1_BatBoss,
        Stage1_SpiderBoss,
        Stage1_2,
        Stage1_GhostBoss,
        Stage1_KnightBoss,

    }

    public NextMapName nextMapNameType;
    public Transform DestinationPoint;
    private CameraController cam;

    public bool fadeInOut;
    public bool CameraMoving;

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag ("Player"))
        {
            collision.transform.position = DestinationPoint.position;

            switch (nextMapNameType)
            {
                case NextMapName.Stage1_Tutorial:
                    Debug.Log("Tutorial");
                    cam.Set_MaxXY(28, 23);
                    break;
                case NextMapName.Stage1_1:
                    Debug.Log("stage1_1");
                    cam.Set_MaxXY(-20, 33);
                    break;
                case NextMapName.Stage1_BatBoss:
                    cam.Set_MaxXY(-68, 33);
                    break;
                case NextMapName.Stage1_SpiderBoss:
                    cam.Set_MaxXY(-68, 57);
                    break;
                case NextMapName.Stage1_2:
                    cam.Set_MaxXY(-20, 57);
                    break;
                case NextMapName.Stage1_GhostBoss:
                    cam.Set_MaxXY(28, 57);
                    break;
                case NextMapName.Stage1_KnightBoss:
                    cam.Set_MaxXY(28, 81);
                    break;
            }
        }
    }

    void CameraSet(float x, float y)
    {
       cam.MinX = x - 20.39f; cam.MaxX = x + 20.8f;
       cam.MinY = y - 12.32f; cam.MaxY = y + 13.68f;
    }
}
