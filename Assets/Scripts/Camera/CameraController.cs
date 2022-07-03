using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraController>();
                if(Instance == null)
                {
                    var instanceInstantiate = new GameObject ( "Main Camera" );
                    instance = instanceInstantiate.GetComponent<CameraController>();
                }
            }
            return instance;
        }
    }
    private static CameraController instance;

    [SerializeField]
    Transform playertransform;
    //[SerializeField]
    //Vector3 cameraPosition;

    public Vector2 offset;
    public float MinX, MaxX, MinY, MaxY;

    //[SerializeField]
    //Vector2 center;
    //[SerializeField]
    //Vector2 mapsize;

    [SerializeField]
    //카메라가 플레이어 따라다니는 이동속도
    float cameraspeed;
    float width;
    float height;

    void Start()
    {
        offset.x = Camera.main.transform.position.x; // 4.16f tutorial
        offset.y = Camera.main.transform.position.y; //10.62f tutorial

        MinX = offset.x - 20.39f; MaxX = offset.x + 20.41f;
        MinY = offset.y - 12.32f; MaxY = offset.y + 13.68f;
        //카메라 수직축의 반만큼의 길이
        height = Camera.main.orthographicSize;
        //카메라 수평축의 반만큼의 길이
        width = height * (Screen.width / Screen.height);

    }

    void LateUpdate()
    {
        CameraArea();
    }

    public bool cameraMoving;

    void CameraArea()
    {
        playertransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (cameraMoving)
        {
            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(playertransform.position.x /*+ offset.x*/, MinX + width, MaxX - width),
                Mathf.Clamp(playertransform.position.y/* + offset.y*/, MinY + height, MaxY - height),
                -10);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraspeed);
        }
        else
        {
            transform.position = playertransform.position;
            cameraMoving = true;
        }
        
    }
}
