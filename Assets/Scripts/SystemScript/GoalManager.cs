using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public bool firstPoint;
    public bool secondPoint;
    public bool thirdPoint;
    public bool FinalPoint;
    public bool goalPoint;

    // Update is called once per frame
    void Update()
    {
        if (goalPoint)
        {
            GameManager.instance.playerLaps++;
            if(GameManager.instance.playerLaps == GameManager.instance.maxLaps)
                GameManager.instance.GameClear();
        }
    }

}
