using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject smoke;
    public LandingCheck landingColider;

    public bool living;
    public Vector2 inputDir;

    private Rigidbody rigid;
    private Animator animator;
    private ParticleSystem particle;
   
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
    public float boosterMaxGauge;
    public float drainedGauge;
    public float awakenGauge;

    public bool landing;   
    public float fallCountTimer;

    private bool singleJump;
    private bool doubleJump;
    private bool superJump;

    public bool freezing;       //모든 키 입력 불가
    public bool immovable;      //회전만 가능
    public bool breaking;        //점프만 가능 (가다 멈추기)
    public bool keyReverse;     //방향 키 입력 반전
    public bool stunning;
    public bool paralysis;
    public bool draining;

    public bool knockback;
    public bool landingBooster;
    public bool landingCheck;

    public float freezingTimer;
    public float immovableTimer;
    public float breakingTimer;
    public float keyReverseTimer;
    public float paralysisTimer;
    public float stunTimer;
    public float boosterTimer;
    public float landingTimer;
    public float drainingTimer;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        living = true;
        boosterGauge = boosterMaxGauge;
        animator = player.gameObject.GetComponent<Animator>();
        particle = smoke.gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (living)
        {
            PlayerControl();
            StateTimerCheck();
            LandingBooster();

            if (boosterOnPad && !stunning)
            {
                Booster();
            }

            if(paralysis)
            {
                Controlparalysis();
            }

            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }


    private void PlayerControl()
    {
        if (stunning == false)
        {
            if(paralysis == false)
            {
                if (draining == false)
                {
                    //착지 대시용 콜라이더 필요
                    if (freezing == false)
                    {
                        if (breaking == false)
                        {
                            InputArrow();
                            PlayerBooster();
                        }
                        PlayerJump();
                    }
                    PlayerRotate();
                    PlayerAddSpeed();
                    PlayerKeyReverse();
                }
                PlayerBoosterGauge();
            }
        }
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
                freezingTimer = 0.0f;
                freezing = false;
            }
        }

        if (stunning)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0.0f)
            {
                stunTimer = 0.0f;
                stunning = false;
            }
        }

        if (boosterOnPad)
        {
            boosterTimer -= Time.deltaTime;

            if (boosterTimer <= 0.0f)
            {
                boosterTimer = 0.0f;
                boosterOnPad = false;
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

        if (breaking)
        {
            breakingTimer -= Time.deltaTime;

            if (breakingTimer <= 0.0f)
            {
                breakingTimer = 0.0f;
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

        if (paralysis)
        {
            paralysisTimer -= Time.deltaTime;

            if (paralysisTimer <= 0.0f)
            {
                paralysisTimer = 0.0f;
                paralysis = false;
            }
        }

        if (draining)
        {
            drainingTimer -= Time.deltaTime;

            if (drainingTimer <= 0.0f)
            {
                drainingTimer = 0.0f;
                draining = false;
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
            knockback = false;
            landingCheck = false;

            if (landingBooster)
            {
                stunning = false;
                boosterOnPad = true;
                boosterTimer = 2.0f;
            }

        }
        animator.SetBool("Landing", landing);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hurdle"))
        {
            while (true)
            {
                if (!stunning)
                {
                    stunning = true;
                    stunTimer = 2.0f;

                    currentSpeed = 0;
                    animator.SetTrigger("Tripped");
                    StartCoroutine(PlaySmoke(0.1f));
                }
                else
                {
                    HandleHurdleCollision(other.gameObject);
                    return;
                }
            }
        }

        if (other.gameObject.CompareTag("KnockBack"))
        {
            while (true)
            {
                if (!stunning)
                {
                    stunning = true;
                    stunTimer = 2.0f;

                    currentSpeed = 0;

                    //animator.SetTrigger("BackFlip");
                    StartCoroutine(PlaySmoke(0.8f));
                }
                else
                {
                    KnockBackCollision();
                    return;
                }
            }
        }

        if (other.gameObject.CompareTag("Booster"))
        {
            if (!boosterOnPad && !stunning)
            {
                boosterOnPad = true;
                boosterTimer = 2.0f;
            }
        }

        if (other.gameObject.CompareTag("Para"))
        {
            if (!paralysis)
            {
                currentSpeed = 0;
                paralysis = true;
                paralysisTimer = 5.0f;
            }
        }
    }


    private void InputArrow()
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

    private void PlayerJump()
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

    private void PlayerBooster()
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

    private void PlayerKeyReverse()
    {
        if (keyReverse == true)
        {
            inputDir = -inputDir;
        }
    }

    private void PlayerRotate()
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

    private void PlayerAddSpeed()
    {
        if (landing == true)
        {
            fallCountTimer = 0.0f;

            if (inputDir == Vector2.zero)
            {
                if (!boosterOnPad)
                if (breaking == false && currentSpeed > 8.0f)
                {
                    breaking = true;
                    breakingTimer = 0.6f;
                    StartCoroutine(PlaySmoke(0.2f));
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


    private void SpeedCheck()
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

    private void AnimatorSet()
    {
        animator.SetFloat("RunAniSpeed", (currentSpeed / 20.0f) + 0.5f);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("Landing", landing);
        animator.SetBool("Landing2", landingColider.GetBool());
        animator.SetBool("Jumping", singleJump);
        animator.SetBool("DoubleJumping", doubleJump);
        animator.SetBool("Breaking", breaking);
        
        if(boosterTimer > 0.0f) animator.SetBool("Booster", true);
        else                    animator.SetBool("Booster", false);

    }

    private IEnumerator PlaySmoke(float sec)
    {
        yield return new WaitForSeconds(sec);
        GameObject buf = Instantiate(smoke);
        buf.transform.position = transform.position + transform.forward * (currentSpeed / 10.0f);
        buf.SetActive(true);
    }


    private void HandleHurdleCollision(GameObject hurdle)
    {
        HurdleObstacle hurdleScript = hurdle.GetComponent<HurdleObstacle>();
        if (hurdleScript != null && Mathf.Abs(hurdleScript.transform.rotation.eulerAngles.x) <= 0f)
        {
            Vector3 playerDirection = transform.forward;

            currentSpeed = 0;
            rigid.velocity = playerDirection * 6f;

        }
    }
    private void KnockBackCollision()
    {
        knockback = true;

        Vector3 playerDirection = -transform.forward.normalized;
        Vector3 highVector = new Vector3(0, 1.5f, 0);

        rigid.velocity = (playerDirection + highVector) * 4.5f;
    }


    private void Booster()
    {
        breaking = false;
        breakingTimer = 0.0f;

        Vector2 playerDirection = transform.forward;
        inputDir = playerDirection;

        currentMaxSpeed = baseMaxSpeed + boosterMaxSpeed;
        currentSpeed = currentMaxSpeed;

        if(landingBooster)
        {
            landingBooster = false;
        }

    }

    public void Controlparalysis()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            paralysisTimer -= Time.deltaTime;
        }
    }

    public void LandingBooster()
    {
        if(landingCheck)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                knockback = false;
                landingBooster = true;
            }
        }        
    }

    public void PlayerBoosterGauge()
    {
        if (currentMaxSpeed >= 20 && currentSpeed >0 && !boosterOnPad)
        {
            boosterGauge -= Time.deltaTime;
            if (boosterGauge <= 0)
            {
                boosterGauge = 0;

                if(!draining)
                {
                    drainedGauge += Time.deltaTime;
                }
                if (drainedGauge >= 1.0f)
                {
                    drainedGauge = 0.0f;
                    currentSpeed = 0f;
                    draining = true;
                    drainingTimer = 2.0f;
                }
            }
        }
        else
        {
            boosterGauge += Time.deltaTime;
            if (boosterGauge >= boosterMaxGauge)
            {
                boosterGauge = boosterMaxGauge;
            }
        }
    }
}
