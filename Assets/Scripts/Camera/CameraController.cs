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
    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    Vector2 center;
    [SerializeField]
    Vector2 mapsize;

    [SerializeField]
    //카메라가 플레이어 따라다니는 이동속도
    float cameraspeed;
    float width;
    float height;

    void Start()
    {
        playertransform = GameObject.Find("Player").GetComponent<Transform>();

        //카메라 수직축의 반만큼의 길이
        height = Camera.main.orthographicSize;
        //카메라 수평축의 반만큼의 길이
        width = height * (Screen.width / Screen.height);
        
        //카메라가 비출수있는 맵의 크기 가로 세로 절반
        mapsize.x = width * 2;
        mapsize.y = height * 2;
    }

    void FixedUpdate()
    {
        CameraArea();
    }

    public bool cameraMoving;

    void CameraArea()
    {
        if(cameraMoving)
        {
            //카메라가 부드럽게 플레이어 위치로 이동
            transform.position = Vector3.Lerp(transform.position, playertransform.position + new Vector3(0, 1.5f, -10), cameraspeed * Time.deltaTime); ;

            ////비출수없는 영역을 카메라가 벗어나지 못하게 만듬
            //float x = mapsize.x - width;
            //float clampX = Mathf.Clamp(transform.position.x, -x + center.x, x + center.x);

            //float y = mapsize.y - height;
            //float clampY = Mathf.Clamp(transform.position.y, -y + center.y, y + center.y);


            //transform.position = new Vector3(clampX, clampY, -10f);
        }
        else
        {
            transform.position = playertransform.position;
            cameraMoving = true;
        }
        
    }
}
