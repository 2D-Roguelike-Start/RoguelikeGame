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

        MaxX = 28f;
        MinX = MaxX - 48;
        MaxY = 23f;
        MinY = MaxY - 24;
        
        //MinX = offset.x - 20.39f; MaxX = offset.x + 20.41f;
        //MinY = offset.y - 12.32f; MaxY = offset.y + 13.68f;
        
        //카메라 수직축의 반만큼의 길이
        height = Camera.main.orthographicSize;
        //카메라 수평축의 반만큼의 길이
        width = height * ((float)Screen.width / (float)Screen.height); ;

        playertransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        CameraArea();
    }

    public bool cameraMoving;

    public void Set_MaxXY(int _X, int _Y)
    {
        MaxX = _X;
        MinX = MaxX - 48;
        MaxY = _Y;
        MinY = MaxY - 24;
    }

    public void Set_PlayerXY(Transform _Trans)
    {
        playertransform = _Trans;
    }

    void CameraArea()
    {
        //카메라 수직축의 반만큼의 길이
        height = Camera.main.orthographicSize;
        //카메라 수평축의 반만큼의 길이
        width = height * ((float)Screen.width / (float)Screen.height); ;

        if (cameraMoving)
        {
            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(playertransform.position.x, MinX + width, MaxX - width),
                Mathf.Clamp(playertransform.position.y, MinY + height, MaxY - height), -10);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraspeed);
            transform.position = desiredPosition;
        }
        else
        {
            transform.position = playertransform.position;
            cameraMoving = true;
        }
        
    }
}
