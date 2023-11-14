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

    public TextMeshProUGUI maxLapText;
    public TextMeshProUGUI currentLapText;

    public Slider BoosterSlider;
    public Slider AwakenSlider;
    public PlayerController player;


    // Update is called once per frame
    void Update()
    {
        ControlBoosterGauge();
        PrintPlayTime();
        PrintCurrentLap();
    }

    public void ControlBoosterGauge()
    {
        BoosterSlider.value = (player.boosterGauge / 10);
        AwakenSlider.value = (player.awakenGauge / 10);
    }

    public void PrintPlayTime()
    {
        //float miliSec = GameManager.instance.playTime % 1f * 100;
        //if(miliSec >= 100)
       // {
        //    miliSec = 0;
        //}
        int sec = Mathf.RoundToInt(GameManager.instance.playTime % 60);
        int min = Mathf.RoundToInt(GameManager.instance.playTime / 60);
        playMinTime.text = string.Format("{0:00}", min);
        playSecTime.text = string.Format("{0:00}", sec);
        //playMiliSecTime.text = string.Format("{0:00}", miliSec);
    }

    public void PrintCurrentLap()
    {
        maxLapText.text = GameManager.instance.maxLaps.ToString();
        currentLapText.text = GameManager.instance.playerLaps.ToString();
    }
}
