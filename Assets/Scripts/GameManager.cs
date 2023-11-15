using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject playerPrefab;
    public GameObject resultUI;
    public static GameManager instance;

    public float playTime;
    public float playerLocation;

    public int playerLaps;
    public int maxLaps;

    public bool gameStart;
    public bool gameLose;
    public bool gameWin;

    public float startTimer;

    private void Awake()
    {
        resultUI.SetActive(false);
        Time.timeScale = 1;
    }

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
        if (gameLose || gameWin)
            return;

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


    public void GameClear()
    {
        gameWin = true;
        StartCoroutine(PrintResultUI());
    }

    IEnumerator PrintResultUI()
    {
        yield return new WaitForSeconds(3.0f);
        resultUI.SetActive(true);
        playerPrefab.GetComponent<Animator>().SetTrigger("Win");
    }
}
