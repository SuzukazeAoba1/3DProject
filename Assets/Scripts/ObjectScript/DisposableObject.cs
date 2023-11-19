using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(other);
        }
    }

}
