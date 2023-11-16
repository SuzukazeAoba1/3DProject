using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleObstacle : MonoBehaviour
{
    public float delayTime;
    public float rotateValue;
    public float rotationSpeed;
    public bool isCollision;

    private Vector3 playerDirection;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private Quaternion targetRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        isCollision = false;
        playerDirection = Vector3.forward; // 초기 방향 설정
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollision)
        {
            isCollision = true;

            playerDirection = other.transform.forward;

            if (playerDirection.z > 0)
            {
                targetRotation = transform.rotation * Quaternion.Euler(rotateValue, 0, 0);
            }
            else
            {
                targetRotation = transform.rotation * Quaternion.Euler(-rotateValue, 0, 0);
            }
            StartCoroutine(ResetRotation(delayTime));
        }
    }

    void Update()
    {
        if (isCollision)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }
    }

    IEnumerator ResetRotation(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.rotation = originalRotation;
        transform.position = originalPosition;
        isCollision = false;
    }
}
