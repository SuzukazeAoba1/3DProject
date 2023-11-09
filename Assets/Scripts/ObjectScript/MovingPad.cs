using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPad : MonoBehaviour
{
    private Transform myTransform;

    private Vector3 originPosition;
    private Vector3 movingPosition;

    public float direction;
    public float tolerance;
    public float movingSpeed;
    public bool going;
    public bool coming;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        movingPosition = transform.position;
        movingPosition.x = transform.position.x + direction;

        going = true;
        coming = false;
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
            if (Mathf.Abs(transform.position.x - movingPosition.x) <= tolerance)
            {
                going = false;
                coming = true;
            }

            else transform.position = Vector3.Lerp(transform.position, movingPosition, movingSpeed);
        }
        else if(coming)
        {
            if (Mathf.Abs(transform.position.x - originPosition.x) <= tolerance)
            {
                going = true;
                coming = false;
            }

            else transform.position = Vector3.Lerp(transform.position, originPosition, movingSpeed);
        }
    }
}