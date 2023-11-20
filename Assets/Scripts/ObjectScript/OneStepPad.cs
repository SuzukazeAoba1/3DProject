using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneStepPad : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject KnockPad;

    private void Awake()
    {
        KnockPad = transform.parent.gameObject;
    }

    void Start()
    {
        KnockPad.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerController>().invincibility == false) KnockPad.SetActive(false);
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
