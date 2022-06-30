using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public ParticleSystem[] effect;

    //trans = 맞는 대상, go = 이펙트
    public GameObject EffectOn(Transform trans, string name = null)
    {
        GameObject go = Managers.Resource.Instantiate($"Effect/{name}");
        // 이펙트의 위치 trans(맞는 대상) 의 위치기준으로 설정
        if(go != null) go.transform.position = trans.transform.position + new Vector3(0, 1, -4);
        go.GetComponent<ParticleSystem>().Play();
        return go;
    }
    
    
}
