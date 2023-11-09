using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingWall : MonoBehaviour
{
    public bool moveCtrl;

    public float changeValue;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 currentPosition = transform.position;

        Vector3 changeVector = currentPosition;
        changeVector.z += changeValue;

        transform.DOMove(changeVector, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    }
}
