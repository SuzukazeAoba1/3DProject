using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneStepPad : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject KnockPad;

    void Start()
    {
        KnockPad.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            KnockPad.SetActive(false);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "KnockBackCollider")
        {
            KnockPad.SetActive(false);
        }
    }
}
