using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingCollierCheck : MonoBehaviour
{
    public GameObject player;
    public bool topCheck;
    public bool landingBooster;
    public bool playerKnockBack;
    public float landingBoosterTimer;
    // Start is called before the first frame update
    private void Start()
    {
        topCheck = false;
        landingBooster = false;
    }

    private void Update()
    {
        playerKnockBack = player.GetComponent<PlayerController>().knockback;

        if(playerKnockBack && player.transform.position.y > 1)
        {
            topCheck = true;
        }

        if (landingBooster && topCheck)
        {
            player.GetComponent<PlayerController>().landingCheck = true;
            topCheck = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            landingBooster = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            landingBooster = false;
        }
    }
}
