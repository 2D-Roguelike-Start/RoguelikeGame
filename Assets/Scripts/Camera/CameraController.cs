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
    //ī�޶� �÷��̾� ����ٴϴ� �̵��ӵ�
    float cameraspeed;
    float width;
    float height;

    void Start()
    {
        //ī�޶� �������� �ݸ�ŭ�� ����
        height = Camera.main.orthographicSize;
        //ī�޶� �������� �ݸ�ŭ�� ����
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
                Mathf.Clamp(playertransform.position.x + offset.x, MinX + width, MaxX - width),
                Mathf.Clamp(playertransform.position.y + offset.y, MinY + height, MaxY - height),
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
