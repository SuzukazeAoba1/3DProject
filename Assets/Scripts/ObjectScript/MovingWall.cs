using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingWall : MonoBehaviour
{
    public float direction;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveX(direction, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
