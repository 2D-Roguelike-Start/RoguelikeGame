using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject[] wall;
    public GameObject creature;
    public GameObject camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == (int)Define.Layer.Player)
        {
            foreach (GameObject go in wall)
                go.SetActive(true);

            creature.GetAddComponent<BossController_Spider>().enabled = true;
            CameraController camset = camera.GetAddComponent<CameraController>();
            camset.offset = new Vector2(-67.93f, 52.35f);
            camset.MinX = -88.3f; camset.MinY = 45.5f;
            camset.MaxX = -52f;   camset.MaxY = 61f;

            Managers.Sound.Clear();
            Managers.Sound.Play(Define.Sound.Bgm, "Sound_SpiderBoss", UI_Setting_SoundPopup.BgmSound);

            Managers.UI.ShowPopupUI<UI_SpiderBossHpPopup>();

            gameObject.SetActive(false);
        }
    }
}
