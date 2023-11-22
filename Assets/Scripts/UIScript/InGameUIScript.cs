using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameUIScript : MonoBehaviour
{
    public TextMeshProUGUI playMinTime;
    public TextMeshProUGUI playSecTime;
    public TextMeshProUGUI playMiliSecTime;

    public Slider BoosterSlider;
    public PlayerController player;


    // Update is called once per frame
    void Update()
    {
        ControlBoosterGauge();
        PrintPlayTime();
    }

    public void ControlBoosterGauge()
    {
        BoosterSlider.value = (player.boosterGauge / 10);
    }

    public void PrintPlayTime()
    {
        int miliSec = Mathf.FloorToInt((GameManager.instance.playTime * 1000) % 1000);

        int sec = Mathf.FloorToInt(GameManager.instance.playTime % 60);
        int min = Mathf.FloorToInt(GameManager.instance.playTime / 60);
        playMinTime.text = string.Format("{0:00}", min);
        playSecTime.text = string.Format("{0:00}", sec);
        playMiliSecTime.text = string.Format("{0:000}", miliSec);
    }

}
