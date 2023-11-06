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
    public bool immovable;      //회전만 가능
    public bool braking;        //점프만 가능 (가다 멈추기)
    public bool keyReverse;     //방향 키 입력 반전

    public float freezingTimer;
    public float immovableTimer;
    public float brakingTimer;
    public float keyReverseTimer;


    private void Start()
    {
        animator = transform.Find("Player").gameObject.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerControl();
        StateTimerCheck();
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void StateTimerCheck()
    {
        if (freezing)
        {
            freezingTimer -= Time.deltaTime;

            if (freezingTimer <= 0.0f)
            {
                freezingTimer = 0.0f;
                freezing = false;
            }
        }

        if (immovable)
        {
            immovableTimer -= Time.deltaTime;

            if (immovableTimer <= 0.0f)
            {
                immovableTimer = 0.0f;
                immovable = false;
            }
        }

        if (braking)
        {
            brakingTimer -= Time.deltaTime;

            if (brakingTimer <= 0.0f)
            {
                brakingTimer = 0.0f;
                braking = false;
            }
        }

        if (keyReverse)
        {
            keyReverseTimer -= Time.deltaTime;

            if (keyReverseTimer <= 0.0f)
            {
                keyReverseTimer = 0.0f;
                keyReverse = false;
            }
        }
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

    private void PlayerControl()
    {

        //착지 대시용 콜라이더 필요

        if (freezing == false)
        {
            if (braking == false)
            {
                InputArrow();
                PlayerBooster();
            }
            PlayerJump();
        }

        if (keyReverse == true)
        {
            inputDir = -inputDir;
        }

        PlayerRotate();

        if (landing == true)
        {
            if (inputDir == Vector2.zero)
            {
                if (braking == false && currentSpeed > 8.0f)
                {
                    braking = true;
                    brakingTimer = 0.6f;
                }
                else
                {
                    currentSpeed -= currentBraking * Time.deltaTime;
                }
            }
            else if (immovable == false)
            {
                currentSpeed += currentAccel * Time.deltaTime;
            }
        }

        SpeedCheck();
        AnimatorSet();
    }

    public void InputArrow()
    {
        inputDir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputDir += Vector2.up;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputDir += Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputDir += Vector2.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputDir += Vector2.right;
        }
    }

    public void PlayerJump()
    {
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
                if (rigid.velocity.y < 0)
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
    }

    public void PlayerBooster()
    {
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
    }

    public void PlayerRotate()
    {

        if (inputDir != Vector2.zero)
        {

            float playerDegree = transform.rotation.eulerAngles.y;
            float inputDegree = (450.01f - (Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg)) % 360.0f - 0.01f;
            inputDegree = Mathf.Round(inputDegree);

            float directionCheck = (360.0f + inputDegree - playerDegree) % 360.0f;
            Debug.Log(directionCheck);

            if (directionCheck < 170.0f)
            {
                transform.rotation = Quaternion.Euler(0.0f, playerDegree + baseRotSpeed * Time.deltaTime , 0.0f);
            }
            else if(directionCheck > 190.0f)
            {
                transform.rotation = Quaternion.Euler(0.0f, playerDegree - baseRotSpeed * Time.deltaTime, 0.0f);
            }
            else
            {
                if (currentSpeed < 5.0f)
                {
                    currentSpeed = 0.0f; 
                    transform.rotation = Quaternion.Euler(0.0f, playerDegree - baseRotSpeed * Time.deltaTime, 0.0f);
                }
                else
                {
                    inputDir = Vector2.zero; //정지
                }
            }
        }
    }

    public void SpeedCheck()
    {
        if (currentSpeed > currentMaxSpeed)
        {
            currentSpeed -= currentBraking * Time.deltaTime;
        }

        if (currentSpeed >= baseMaxSpeed + boosterMaxSpeed)
        {
            currentSpeed = baseMaxSpeed + boosterMaxSpeed;
        }

        if (currentSpeed <= 0) currentSpeed = 0;
    }
    public void AnimatorSet()
    {
        animator.SetFloat("RunAniSpeed", (currentSpeed / 20.0f) + 0.5f);
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
