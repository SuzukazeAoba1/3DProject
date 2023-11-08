using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingCollierCheck : MonoBehaviour
{
    public GameObject player;
    private bool landingBooster;
    private bool playerKnockBack;
    public float landingBoosterTimer;
    // Start is called before the first frame update
    private void Start()
    {
        landingBooster = false;
    }

    private void Update()
    {
        playerKnockBack = player.GetComponent<PlayerController>().knockback;

        if (landingBooster && playerKnockBack && player.transform.position.y > 1.0f)
        {
            Debug.Log("이것부터 안되는거니?");

            player.GetComponent<PlayerController>().landingTime = true;
            player.GetComponent<PlayerController>().landingTimer = 0.5f;
            landingBooster = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            landingBooster = true;
        }
    }
}
