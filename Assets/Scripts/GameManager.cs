using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static GameManager instance; 

    public float playTime;
    public float playerLocation;
    public float playerLaps;

    public bool gameStart;
    public bool gameLose;
    public bool gameWin;

    public float startTimer;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(gameStart)
        {
            playTime += Time.deltaTime;
        }     
        else
        {
            timerCheck();
        }
    }

    void timerCheck()
    {
        if (startTimer >= 0)
        {
            startTimer -= Time.deltaTime;
        }

        if (startTimer <= 0)
        {
            startTimer = 0;
            gameStart = true;
        }
    }
}
