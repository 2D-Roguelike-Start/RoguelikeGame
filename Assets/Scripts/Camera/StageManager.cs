using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if(instance == null)
                {
                    var instanceInstantiate = new GameObject("Stage Manager");
                    instance = instanceInstantiate.GetComponent<StageManager>();
                }
            }
            return instance;
        }
    }

    private static StageManager instance;

    public Image FadeInOutImage;

    private float a;
    
    public IEnumerator FadeInOut (Collider2D collision, Vector3 destination, bool fadeInOut, bool CameraMoving)
    {
        yield return null;
        if (fadeInOut)
        {
            yield return StartCoroutine(FadeIn());
        }
        CameraController.Instance.cameraMoving = CameraMoving;

        collision.transform.position = destination;

        if (fadeInOut)
        {
            yield return StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        a = 1;
        FadeInOutImage.color = new Vector4(0, 0, 0, a);
        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator FadeOut()
    {
        while (a >= 0)
        {
            FadeInOutImage.color = new Vector4(0, 0, 0, a);
            a -= 0.02f;
            yield return null;
        }
    }
}
