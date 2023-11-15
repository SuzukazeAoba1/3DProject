using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    public GoalManager goalManager;
    public PlayerController playerCon;
    public CameraController cameraCon;
    public cameraMode changeCameraMode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CheckPoint();
        }
    }
    void CheckPoint()
    {
        switch (gameObject.name)
        {
            case "FirstPoint":
                if (!goalManager.firstPoint && !goalManager.secondPoint && !goalManager.thirdPoint && !goalManager.FinalPoint)
                    goalManager.firstPoint = true;
                break;

            case "SecondPoint":
                if (goalManager.firstPoint && !goalManager.secondPoint && !goalManager.thirdPoint && !goalManager.FinalPoint)
                    goalManager.secondPoint = true;
                break;

            case "ThirdPoint":
                if (goalManager.firstPoint && goalManager.secondPoint && !goalManager.thirdPoint && !goalManager.FinalPoint)
                    goalManager.thirdPoint = true;
                break;

            case "FinalPoint":
                if (goalManager.firstPoint && goalManager.secondPoint && goalManager.thirdPoint && !goalManager.FinalPoint)
                    goalManager.FinalPoint = true;
                break;

            case "GoalPoint":
                if (goalManager.firstPoint && goalManager.secondPoint && goalManager.thirdPoint && goalManager.FinalPoint)
                {
                    goalManager.goalPoint = true;
                    GameManager.instance.playerLaps++;
                    if(GameManager.instance.playerLaps >= GameManager.instance.maxLaps)
                    {
                        cameraCon.currentCameraMode = changeCameraMode;
                        playerCon.BoosterOff();
                    }
                    

                }
                break;

        }
    }
}
