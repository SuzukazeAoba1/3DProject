using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
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
    public bool boosterOnPad;
    public bool boosterSkill;
    public float boosterAddAccel;
    public float boosterMaxSpeed;
    public float boosterGauge;
    public float awakenGauge;

    public bool landing;
    public float fallCountTimer;

    public bool singleJump;
    public bool doubleJump;
    public bool superJump;

    public bool freezing;       //모든 키 입력 불가
    public bool immovable;      //회전만 가능
    public bool breaking;        //점프만 가능 (가다 멈추기)
    public bool keyReverse;     //방향 키 입력 반전
    public bool stunning;

    public float freezingTimer;
    private float freezingOriginTimer;
    public float immovableTimer;
    private float immovableOriginTimer;
    public float breakingTimer;
    private float breakingOriginTimer;
    public float keyReverseTimer;
    public float boosterTimer;
    private float boosterOriginTimer;
    public float stunTimer;
    private float stunOriginTimer;

    private void Start()
    {
        player = transform.Find("Player").gameObject;
        animator = player.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerControl();
        StateTimerCheck();
        
        if (boosterOnPad && !stunning)
        {
            Booster();
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void PlayerControl()
    {

        //착지 대시용 콜라이더 필요
        if (freezing == false)
        {
            if (breaking == false)
            {
                if (!stunning)
                {
                    InputArrow();
                    PlayerBooster();
                }
            }
            PlayerJump();
        }

        PlayerKeyReverse();
        PlayerRotate();
        PlayerAddSpeed();
        AnimatorSet();
    }


    private void StateTimerCheck()
    {

        if(landing == false)
        {
            fallCountTimer += Time.deltaTime;
        }

        if (freezing)
        {
            freezingTimer -= Time.deltaTime;

            if (freezingTimer <= 0.0f)
            {
                freezingTimer = freezingOriginTimer;
                freezing = false;
            }
        }

        if (stunning)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0.0f)
            {
                stunTimer = stunOriginTimer;
                stunning = false;
            }
        }

        if (boosterOnPad)
        {
            boosterTimer -= Time.deltaTime;

            if (boosterTimer <= 0.0f)
            {
                boosterTimer = boosterOriginTimer;
                boosterOnPad = false;
            }
        }

        if (immovable)
        {
            immovableTimer -= Time.deltaTime;

            if (immovableTimer <= 0.0f)
            {
                immovableTimer = immovableOriginTimer;
                immovable = false;
            }
        }

        if (breaking)
        {
            breakingTimer -= Time.deltaTime;

            if (breakingTimer <= 0.0f)
            {
                breakingTimer = breakingOriginTimer;
                breaking = false;
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
            HandleBoosterCollision();
        }

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

    public void PlayerKeyReverse()
    {
        if (keyReverse == true)
        {
            inputDir = -inputDir;
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
 
            if(directionCheck < 1.0f || directionCheck > 359.0f)
            {

            }
            else if (directionCheck < 170.0f)
            {
                transform.rotation = Quaternion.Euler(0.0f, playerDegree + baseRotSpeed * Time.deltaTime, 0.0f);
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

    public void PlayerAddSpeed()
    {
        if (landing == true)
        {
            fallCountTimer = 0.0f;

            if (inputDir == Vector2.zero)
            {
                if (breaking == false && currentSpeed > 8.0f)
                {
                    breaking = true;
                    breakingTimer = 0.6f;
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
        animator.SetBool("Breaking", breaking);
    }

    private void HandleHurdleCollision(GameObject hurdle)
    {
        HurdleObstacle hurdleScript = hurdle.GetComponent<HurdleObstacle>();
        if (hurdleScript != null && Mathf.Abs(hurdleScript.transform.rotation.eulerAngles.x) <= 0f)
        {
            stunning = true;
            Vector3 playerDirection = transform.forward;

            currentSpeed = 0;
            rigid.AddForce(playerDirection * 4f, ForceMode.Impulse);
        }
    }

    private void HandleBoosterCollision()
    {
        boosterOnPad = true;
    }


    private void KnockBackCollision()
    {
        stunning = true;
        Vector3 playerDirection = -transform.forward;
        Vector3 highVector = new Vector3(0, 1, 0);
        rigid.AddForce((playerDirection + highVector) * 4.5f, ForceMode.Impulse);
      
        currentSpeed = 0;
    }


    private void Booster()
    {
        Vector2 playerDirection = transform.forward;
        Debug.Log(playerDirection);
        inputDir = playerDirection;

        currentMaxSpeed = baseMaxSpeed + boosterMaxSpeed;
        currentSpeed = currentMaxSpeed;
    } 

}
