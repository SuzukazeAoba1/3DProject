using Cinemachine;
using UnityEngine;

public enum cameraMode
{
    baseline,
    leftcurve,
    secondline,
    rightcurve,
    gamewin
}

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera subCamera;
    public cameraMode currentCameraMode;
    public Transform playerTransform;
    public Transform targetLeft;
    public Transform targetRight;
    public float cameraDistance;
    public float cameraHeight;
    private CinemachineTransposer subTranspos;
    public Vector3 directionAngle;
    public float cameraAngle;
    public float winAngle;

    private void Start()
    {
        subTranspos = subCamera.GetCinemachineComponent<CinemachineTransposer>();
        directionAngle = Vector3.zero;
        winAngle = 0.0f;
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

        switch (currentCameraMode)
        {
            case cameraMode.baseline:
                directionAngle = rotation * Vector3.left;
                break;
            case cameraMode.rightcurve:
                directionAngle = rotation * (targetRight.position - playerTransform.position).normalized;
                break;
            case cameraMode.secondline:
                directionAngle = rotation * Vector3.right;
                break;
            case cameraMode.leftcurve:
                directionAngle = rotation * (targetLeft.position - playerTransform.position).normalized;
                break;
            case cameraMode.gamewin:
                winAngle += Time.deltaTime * 20.0f;
                rotation = Quaternion.Euler(0f, winAngle, 0f);
                directionAngle = rotation * (targetLeft.position - playerTransform.position).normalized;
                break;
        }

        if (currentCameraMode == cameraMode.gamewin)
        {
            directionAngle.y = 0.0f;
            subTranspos.m_FollowOffset = Vector3.up * cameraHeight * 0.5f;
            subTranspos.m_FollowOffset += directionAngle * -cameraDistance * 0.5f;
        }
        else
        {
            directionAngle.y = 0.0f;
            subTranspos.m_FollowOffset = Vector3.up * cameraHeight;
            subTranspos.m_FollowOffset += directionAngle * -cameraDistance;
        }

        cameraAngle = (450.01f - (Mathf.Atan2(directionAngle.z, directionAngle.x) * Mathf.Rad2Deg)) % 360.0f - 0.01f;

    }
}
