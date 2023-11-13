using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterials[1] = GetComponent<MeshRenderer>().sharedMaterials[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
