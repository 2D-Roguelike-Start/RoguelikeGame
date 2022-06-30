using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public ParticleSystem[] effect;

    //trans = �´� ���, go = ����Ʈ
    public GameObject EffectOn(Transform trans, string name = null)
    {
        GameObject go = Managers.Resource.Instantiate($"Effect/{name}");
        // ����Ʈ�� ��ġ trans(�´� ���) �� ��ġ�������� ����
        if(go != null) go.transform.position = trans.transform.position + new Vector3(0, 1, -4);
        go.GetComponent<ParticleSystem>().Play();
        return go;
    }
    
    
}
