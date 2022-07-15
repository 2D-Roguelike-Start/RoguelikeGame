using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    ScreenFixed screenfix;

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
    float Width;
    float Height;

    public float width;
    public float height;
    public float screenwidth;
    public float screenheight;
    public float fix_width;
    public float fix_height;

    void Start()
    {
        screenfix = Camera.main.GetComponent<ScreenFixed>();
        //맞추고싶은 비율
        width = (float)screenfix.setWidth / (float)screenfix.setHeight;
        height = (float)screenfix.setHeight / (float)screenfix.setWidth;
        Debug.Log($"{width.ToString("N3")}, {height.ToString("N3")}");

        //기기의 비율
        screenwidth = screenfix.deviceWidth / screenfix.deviceHeight;
        screenheight = screenfix.deviceHeight / screenfix.deviceWidth;
        Debug.Log($"{screenwidth}, {screenheight}");

        fix_width = (width <= screenwidth) ? screenwidth - width : width - screenwidth;
        fix_height = (height <= screenheight) ? screenheight - height : height - screenheight;
        Debug.Log($"{fix_width}, {fix_height}");

        offset.x = Camera.main.transform.position.x; // 4.16f tutorial
        offset.y = Camera.main.transform.position.y; //10.62f tutorial

        MinX = offset.x - (20.39f + (20.39f * fix_width)); MaxX = offset.x + (20.8f + (20.8f * fix_width));
        MinY = offset.y - (12.32f + (12.32f * fix_height)); MaxY = offset.y + (13.68f + (13.68f * fix_height));
        //카메라 수직축의 반만큼의 길이
        Height = Camera.main.orthographicSize;
        //카메라 수평축의 반만큼의 길이
        Width = Height * (Screen.width / Screen.height);

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
                Mathf.Clamp(playertransform.position.x /*+ offset.x*/, MinX + Width, MaxX - Width),
                Mathf.Clamp(playertransform.position.y/* + offset.y*/, MinY + Height, MaxY - Height),
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
