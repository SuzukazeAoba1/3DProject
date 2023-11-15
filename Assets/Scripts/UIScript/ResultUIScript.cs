using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIScript : MonoBehaviour
{
    public TextMeshProUGUI playTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PrintPlayTime();
    }

    public void PrintPlayTime()
    {
        int miliSec = Mathf.FloorToInt((GameManager.instance.playTime * 1000) % 1000);
        int sec = Mathf.FloorToInt(GameManager.instance.playTime % 60);
        int min = Mathf.FloorToInt(GameManager.instance.playTime / 60);

        playTime.text = string.Format("{0:D2} : {1:D2} : {2:D2}", min, sec, miliSec);
    }
}
