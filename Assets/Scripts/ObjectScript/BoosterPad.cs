using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPad : MonoBehaviour
{
    public bool isCollison;
    public float boosterSpeed;

    public GameObject player;
    private Rigidbody playerRigid;
    private Vector3 playerDirection;

    public PlayerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = player.GetComponent<Rigidbody>();
        playerCtrl = player.GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerCtrl.Booster();
            isCollison = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollison = false;
        }
    }
}


/*
 *  
 * 
 * 
 * if(collision.gameObject.CompareTag("Player"))
        {
            playerDirection = collision.transform.forward;
            Debug.Log(playerDirection);
            
            
            playerRigid.velocity = Vector3.zero;
            playerRigid.AddForce(playerDirection * boosterSpeed, ForceMode.Impulse);
            isCollison = true;
        }
 */