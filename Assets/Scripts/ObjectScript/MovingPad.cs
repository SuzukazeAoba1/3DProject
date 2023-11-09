using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPad : MonoBehaviour
{
    public bool movingX;
    public bool movingY;
    public bool movingZ;

    public float direction;
    public float time;
    void Start()
    {
        if (movingX)
        {
            transform.DOLocalMoveX(direction, time)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else if (movingY)
        {
            transform.DOLocalMoveY(direction, time)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else if (movingZ)
        {
            transform.DOLocalMoveZ(direction, time)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 contactNormal = collision.contacts[0].normal;

        if (collision.gameObject.tag == "Player" && contactNormal == Vector3.down)
        {
            collision.transform.parent = transform;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;

        }
    }
}
