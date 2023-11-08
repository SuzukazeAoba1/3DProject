using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingCheck : MonoBehaviour
{
    public bool landing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            landing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            landing = false;
        }
    }

    public bool GetBool()
    {
        return landing;
    }

}