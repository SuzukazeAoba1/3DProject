using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody rigid;

    public Vector2 inputDir;
    public float currentSpeed;
    public float currentMaxSpeed;
    public float currentAccel;
    public float currentBraking;

    public float baseMaxSpeed;
    public float baseAccel;
    public float baseRotSpeed;
    public float baseJumpPower;

    public bool boosterbuf;
    public bool boosterSkill;
    public float boosterGauge;
    public float boosterAddAccel;
    public float boosterMaxSpeed;
    public float awakenGauge;

    public bool landing;
    public bool singleJump;
    public bool doubleJump;
    public bool superJump;

    public bool freezing;       //모든 키 입력 불가
    public bool immovable;      //회전 가능
    public bool keyReverse;
    

    private void Start()
    {
        animator = transform.Find("Player").gameObject.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerControl();
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (rigid.velocity.y < 10 && rigid.velocity.y > -10)
        {
            rigid.AddForce(Vector3.down * 10.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            landing = true;
            singleJump = false;
            doubleJump = false;
        }

        animator.SetBool("Landing", landing);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hurdle"))
        {
            HandleHurdleCollision(other.gameObject);
        }

        if (other.gameObject.CompareTag("KnockBack"))
        {
            KnockBackCollision();
        }

        if (other.gameObject.CompareTag("Booster"))
        {

        }

    }

    private void BoosterOn()
    {

    }


    private void PlayerControl()
    {

        //플레이어의 방향과 입력이 일치하면 속도 증가,
        //일치하지 않으면 입력 방향으로 회전,
        //입력하지 않으면 속도 감소
        //착지 대시용 콜라이더 필요

        bool move = false;

        inputDir = Vector2.zero;

        if (freezing == false)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                inputDir += Vector2.up;
                move = true;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                inputDir += Vector2.down;
                move = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                inputDir += Vector2.left;
                move = true;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                inputDir += Vector2.right;
                move = true;
            }


            if (Input.GetKey(KeyCode.Z))
            {
                currentMaxSpeed = baseMaxSpeed + boosterMaxSpeed;
                currentAccel = baseAccel + boosterAddAccel;
            }
            else
            {
                currentMaxSpeed = baseMaxSpeed;
                currentAccel = baseAccel;
            }


            if (Input.GetKeyDown(KeyCode.C))
            {
                if (singleJump == false)
                {
                    rigid.AddForce(Vector3.up * baseJumpPower);
                    landing = false;
                    singleJump = true;
                }
                else if (doubleJump == false)
                {
                    if (rigid.velocity.y < 0 )
                    {
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
                    }
                    else if (rigid.velocity.y > 3)
                    {
                        rigid.velocity = new Vector3(rigid.velocity.x, 3, rigid.velocity.z);
                    }
                    rigid.AddForce(Vector3.up * baseJumpPower * 0.5f);
                    doubleJump = true;
                }
            }

            if (move == true)
            {
                if (keyReverse)
                {
                    inputDir = -inputDir;
                }

                float degree = (450.01f - (Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg)) % 360.0f - 0.01f;
                degree = Mathf.Round(degree);

                transform.rotation = Quaternion.Euler(0.0f, degree, 0.0f);

                if (landing == true && immovable == false)
                {
                    currentSpeed += currentAccel * Time.deltaTime;
                }
            }
        }

        if (landing == true)
        {
            if (move == false || currentSpeed > currentMaxSpeed)
            {
                currentSpeed -= currentBraking * Time.deltaTime;
            }
        }

        if (currentSpeed >= baseMaxSpeed + boosterMaxSpeed)
        {
            currentSpeed = baseMaxSpeed + boosterMaxSpeed;
        }

        if (currentSpeed <= 0) currentSpeed = 0;

        AnimatorSet();
    }

    public void AnimatorSet()
    {
        animator.SetFloat("RunAniSpeed", (currentSpeed / 40.0f) + 0.4f);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("Landing", landing);
        animator.SetBool("Jumping", singleJump);
        animator.SetBool("DoubleJumping", doubleJump);
    }



    private void HandleHurdleCollision(GameObject hurdle)
    {
        HurdleObstacle hurdleScript = hurdle.GetComponent<HurdleObstacle>();
        if (hurdleScript != null && Mathf.Abs(hurdleScript.transform.rotation.eulerAngles.x) <= 0f)
        {
            Debug.Log(hurdleScript.isCollision);
            Vector3 playerDirection = transform.forward;

            freezing = true;
            currentSpeed = 0;
            rigid.AddForce(playerDirection * 4f, ForceMode.Impulse);

            StartCoroutine(ReleaseFreeze(2.0f));
        }
    }


    private void KnockBackCollision()
    {
        Vector3 playerDirection = -transform.forward;
        Vector3 hightVector = new Vector3(0, 1, 0);

        freezing = true;
        currentSpeed = 0;
        rigid.AddForce((playerDirection + hightVector) * 4.5f, ForceMode.Impulse);
        StartCoroutine(ReleaseFreeze(1.0f));
    }

    private IEnumerator ReleaseFreeze(float dealy)
    {
        yield return new WaitForSeconds(dealy);
        freezing = false;
    }

}
