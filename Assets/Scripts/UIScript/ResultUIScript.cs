using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIScript : MonoBehaviour
{
    public TextMeshProUGUI playTime;
    public TextMeshProUGUI highTime;
    public float HighScore;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        string prefname = GameManager.instance.playStageNum.ToString();

        if (!PlayerPrefs.HasKey(prefname))
        {
            PlayerPrefs.SetFloat(prefname, 60.0f * 60.0f - 1.0f);
        }

        HighScore = PlayerPrefs.GetFloat(prefname);

        if (HighScore > GameManager.instance.playTime)
        {
            PlayerPrefs.SetFloat(prefname, GameManager.instance.playTime);
        }

        PrintPlayTime();
    }

    public void PrintPlayTime()
    {
        int miliSec = Mathf.FloorToInt((GameManager.instance.playTime * 1000) % 1000);
        int sec = Mathf.FloorToInt(GameManager.instance.playTime % 60);
        int min = Mathf.FloorToInt(GameManager.instance.playTime / 60);

        int hmiliSec = Mathf.FloorToInt((HighScore * 1000) % 1000);
        int hsec = Mathf.FloorToInt(HighScore % 60);
        int hmin = Mathf.FloorToInt(HighScore / 60);

        playTime.text = string.Format("{0:D2} : {1:D2} : {2:D3}", min, sec, miliSec);
        highTime.text = string.Format("{0:D2} : {1:D2} : {2:D3}", hmin, hsec, hmiliSec);
    }
}
