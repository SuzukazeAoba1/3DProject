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

    private void Update()
    {
        if(isCollison)
        {
            playerDirection = player.transform.forward;
            playerRigid.AddForce(playerDirection * boosterSpeed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollison = true;
            StartCoroutine(ReleaseFreeze(2.0f));
        }

    }

    private IEnumerator ReleaseFreeze(float dealy)
    {
        yield return new WaitForSeconds(dealy);
        isCollison = false;
    }


}