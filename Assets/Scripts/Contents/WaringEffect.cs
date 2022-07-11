using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaringEffect : MonoBehaviour
{
    public float Effect_Time = 0;
     Transform Effect;
    // Start is called before the first frame update
    void Start()
    {
        Effect = transform.Find("Warning_Effect");
        
        StartCoroutine(Effect_Timer());
    }

    // Update is called once per frame
    void Update()
    {
       Effect.localScale = new Vector3(Effect.localScale.x + 20 * Time.deltaTime, Effect.localScale.y, Effect.localScale.z);
       Effect.localPosition = new Vector3(Effect.localScale.x/2, Effect.localPosition.y, Effect.localPosition.z);
        Debug.Log($"¿ö´×ÀÌÆåÅÍx : {Effect.position.x}");
    }

    IEnumerator Effect_Timer()
    {
        yield return new WaitForSeconds(Effect_Time);
        Destroy(gameObject);
    }
}
