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
    //ī�޶� �÷��̾� ����ٴϴ� �̵��ӵ�
    float cameraspeed;
    float width;
    float height;

    void Start()
    {
        playertransform = GameObject.Find("Player").GetComponent<Transform>();

        //ī�޶� �������� �ݸ�ŭ�� ����
        height = Camera.main.orthographicSize;
        //ī�޶� �������� �ݸ�ŭ�� ����
        width = height * (Screen.width / Screen.height);
        
        //ī�޶� ������ִ� ���� ũ�� ���� ���� ����
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
            //ī�޶� �ε巴�� �÷��̾� ��ġ�� �̵�
            transform.position = Vector3.Lerp(transform.position, playertransform.position + new Vector3(0, 1.5f, -10), cameraspeed * Time.deltaTime); ;

            ////��������� ������ ī�޶� ����� ���ϰ� ����
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
