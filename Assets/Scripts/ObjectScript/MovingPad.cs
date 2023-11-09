using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPad : MonoBehaviour
{
    private bool landing;
    private Rigidbody playerRigid;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 moveDireciton;

    public bool movingX;
    public bool movingY;
    public bool movingZ;

    public float direction;
    public float time;

    private void Awake()
    {
        currentPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (movingX) transform.DOLocalMoveX(direction, time).SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.InOutSine).OnUpdate(() => moveDireciton = tempPosition());
        else if (movingY) transform.DOLocalMoveY(direction, time).SetLoops(-1, LoopType.Yoyo).
                 SetEase(Ease.InOutSine).OnUpdate(() => previousPosition = transform.position);
        else if (movingZ) transform.DOLocalMoveZ(direction, time).SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.InOutSine).OnUpdate(() => previousPosition = transform.position);
    }

    // Update is called once per frame
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            landing = collision.gameObject.GetComponent<PlayerController>().landing;
            playerRigid = collision.gameObject.GetComponent<Rigidbody>();

            if(landing)
            {
                if(movingX)
                {
                    playerRigid.MovePosition(collision.gameObject.transform.position + moveDireciton);
                }
          
                else if(movingY) collision.gameObject.transform.position = transform.position;

                else if(movingZ) collision.gameObject.transform.position = transform.position;
            }
        }
    }

    public Vector3 tempPosition()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;

        return currentPosition - previousPosition;
    }

}