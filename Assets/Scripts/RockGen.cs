using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    public GameObject rock;
    public float makeTime;
    private float timer;

    public GameObject map1;
    public GameObject map2;
    public GameObject map3;

    private void Start()
    {
        if(makeTime < 1.0f)
        {
            makeTime = 5.0f;
        }

        timer = makeTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > makeTime)
        {
            GameObject buf = Instantiate(rock, transform);
            timer = 0.0f;

            Physics.IgnoreCollision(map1.GetComponent<Collider>(), buf.GetComponent<Collider>());
            Physics.IgnoreCollision(map2.GetComponent<Collider>(), buf.GetComponent<Collider>());
            Physics.IgnoreCollision(map3.GetComponent<Collider>(), buf.GetComponent<Collider>());
        }
    }

}
