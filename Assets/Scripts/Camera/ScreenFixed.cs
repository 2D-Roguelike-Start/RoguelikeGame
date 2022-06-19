using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFixed : MonoBehaviour
{
    // fix하고 싶은 해상도
    public int setWidth = 1920;
    public int setHeight = 1080;

    //기기의 해상도 체크
    public int deviceWidth;
    public int deviceHeight;

    void Start()
    {
        deviceWidth = Screen.width;
        deviceHeight = Screen.height;
        SetResolution();
    }

    void SetResolution()
    {
        //fix 한 해상도로 화면을 맞추는 과정
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //기기의 해상도 비가 더 큰경우
        if((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            //새로운 너비
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            //새로운 rect 적용
            Camera.main.rect = new Rect((1 - newWidth) / 2, 0, newWidth, 1);
        }

        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);

            Camera.main.rect = new Rect(0, (1 - newHeight) / 2, 1, newHeight);
        }
    }
}
