using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageTest : MonoBehaviour
{
    public enum NextPositionType
    {
        InitPosition,
        SomePosition,
    }

    public NextPositionType nextPositionType;

    public Transform DestinationPoint;

    public bool fadeInOut;
    public bool CameraMoving;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag ("Player"))
        {
            if (nextPositionType == NextPositionType.InitPosition)
            {
                collision.transform.position = Vector3.zero;
                StartCoroutine(StageManager.Instance.FadeInOut(collision, Vector3.zero, fadeInOut, CameraMoving));
            }
                
            else if (nextPositionType == NextPositionType.SomePosition)
            {
                collision.transform.position = DestinationPoint.position;
                StartCoroutine(StageManager.Instance.FadeInOut(collision, DestinationPoint.position, fadeInOut, CameraMoving));
            }
            else
            {

            }
        }
    }
}
