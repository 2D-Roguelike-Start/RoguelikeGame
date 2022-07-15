using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFixed : MonoBehaviour
{
    // fix�ϰ� ���� �ػ�
    public int setWidth = 1920;
    public int setHeight = 1080;

    //����� �ػ� üũ
    public float deviceWidth;
    public float deviceHeight;
    private void Awake()
    {
        deviceWidth = Screen.width;
        deviceHeight = Screen.height;
    }

    void Start()
    {
        SetResolution();
    }

    void SetResolution()
    {
        //fix �� �ػ󵵷� ȭ���� ���ߴ� ����
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //����� �ػ� �� �� ū���
        if((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            //���ο� �ʺ�
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            //���ο� rect ����
            Camera.main.rect = new Rect((1 - newWidth) / 2, 0, newWidth, 1);
        }

        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);

            Camera.main.rect = new Rect(0, (1 - newHeight) / 2, 1, newHeight);
        }
    }
}
