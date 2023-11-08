using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPad : MonoBehaviour
{
    private Transform myTransform;
    public float direction;
    public bool going;
    public bool coming;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if(going)
        {
        }
        else if(coming)
        {

        }
    }
}