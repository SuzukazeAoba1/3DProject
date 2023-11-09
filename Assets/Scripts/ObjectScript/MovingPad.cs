using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPad : MonoBehaviour
{
    private bool landing;
    private Vector3 previousPosition;

    public bool movingX;
    public bool movingY;
    public bool movingZ;

    public float direction;
    public float time;


    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        if (movingX) transform.DOLocalMoveX(direction, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        else if(movingY) transform.DOLocalMoveY(direction, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        else if (movingZ) transform.DOLocalMoveZ(direction, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            landing = collision.gameObject.GetComponent<PlayerController>().landing;

            if(landing)
            {
                if(movingX)
                {
                    collision.gameObject.transform.position += new Vector3(1, 0, 0);
                }
          
                else if(movingY) collision.gameObject.transform.position = transform.position;

                else if(movingZ) collision.gameObject.transform.position = transform.position;
            }
        }
    }

}