using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraChange : MonoBehaviour
{

    public CameraController cameraCon;
    public cameraMode changeCameraMode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cameraCon.currentCameraMode = changeCameraMode;
        }
    }


}
