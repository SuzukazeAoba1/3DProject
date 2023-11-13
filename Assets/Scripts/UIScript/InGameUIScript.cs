using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameUIScript : MonoBehaviour
{
    public TextMeshProUGUI playTimeText;

    public TextMeshProUGUI maxLapText;
    public TextMeshProUGUI currentLapText;

   // public Slider BoosterSlider;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
 
    }

    // Update is called once per frame
    void Update()
    {
        ControlBoosterGauge();
        PrintPlayTime();
        PrintCurrentLap();
    }

    public void ControlBoosterGauge()
    {
       // BoosterSlider.value = (player.boosterGauge / 10);
    }

    public void PrintPlayTime()
    {
    }

    public void PrintCurrentLap()
    {
        maxLapText.text = GameManager.instance.maxLaps.ToString();
        currentLapText.text = GameManager.instance.playerLaps.ToString();
    }
}
