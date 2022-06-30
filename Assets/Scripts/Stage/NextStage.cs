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
        switch (nextMapNameType) 
        {
            case NextMapName.Stage1_Tutorial:
                Debug.Log("Tutorial");
                cam.offset.x = 4.39f; cam.offset.y = 10.62f;
                CameraSet(cam.offset.x, cam.offset.y); break;
            case NextMapName.Stage1_1:
                Debug.Log("stage1_1");
                cam.offset.x = -46.71f; cam.offset.y = 20.42f;
                CameraSet(cam.offset.x, cam.offset.y); break;
            case NextMapName.Stage1_BatBoss:
                cam.offset.x = -96.7f; cam.offset.y = 20.53f;
                CameraSet(cam.offset.x, cam.offset.y); break;
        }



        if (collision.transform.CompareTag ("Player"))
        {
            Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
            if (rigid.velocity.x >= 0) collision.transform.position = DestinationPoint.position + new Vector3(2, 0, 0);
            else collision.transform.position = DestinationPoint.position + new Vector3(-2, 0, 0);
            //if (nextPositionType == NextPositionType.InitPosition)
            //{
            //    collision.transform.position = Vector3.zero;
            //    StartCoroutine(StageManager.Instance.FadeInOut(collision, Vector3.zero, fadeInOut, CameraMoving));
            //}
                
            //else if (nextPositionType == NextPositionType.SomePosition)
            //{
            //    collision.transform.position = DestinationPoint.position;
            //    StartCoroutine(StageManager.Instance.FadeInOut(collision, DestinationPoint.position, fadeInOut, CameraMoving));
            //}
            //else
            //{

            //}
        }
    }

    void CameraSet(float x, float y)
    {
       cam.MinX = x - 20.39f; cam.MaxX = x + 20.8f;
       cam.MinY = y - 12.32f; cam.MaxY = y + 13.68f;
    }
}
