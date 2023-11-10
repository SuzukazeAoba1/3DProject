using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject boosterEffect;
    public GameObject smokeEffect;
    public GameObject jumpEffect;
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

    public bool boosterOnKey;
    public bool boosterOnPad;
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
    public float drainingTimer;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        living = true;
        boosterGauge = boosterMaxGauge;
        animator = player.gameObject.GetComponent<Animator>();
        particle = smokeEffect.gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (living)
        {
            PlayerControl();
            StateTimerCheck();

            if (boosterOnPad && !stunning && !knockback& !paralysis)
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

        if (!stunning && !paralysis && !knockback && !draining && !freezing && !breaking && !keyReverse)
        {
            if (!keyReverse)
                InputArrow();
            else
                InputReserveArrow(); 
        }

        if (!stunning && !paralysis && !knockback && !draining && !freezing && !breaking)
        {
            PlayerBooster();
        }

        if (!stunning && !paralysis && !knockback && !draining && !freezing)
        {
            PlayerJump();
        }

        if (!stunning && !paralysis && !knockback && !draining)
        {
            PlayerRotate();
            PlayerAddSpeed();
        }

        if (!stunning && !paralysis && !knockback)
        {
            PlayerBoosterGauge();
        }

        if (!stunning && !paralysis)
        {
            LandingBooster();
        }

        AnimatorSet();

    }


    private void StateTimerCheck()
    {
        if(landing == false)
        {
            fallCountTimer += Time.deltaTime;
        }

        TimerCheck(ref freezing, ref freezingTimer);
        TimerCheck(ref stunning, ref stunTimer);
        TimerCheck(ref boosterOnPad, ref boosterTimer);
        TimerCheck(ref immovable, ref immovableTimer);
        TimerCheck(ref breaking, ref breakingTimer);
        TimerCheck(ref keyReverse, ref keyReverseTimer);
        TimerCheck(ref paralysis, ref paralysisTimer);
        TimerCheck(ref draining, ref drainingTimer);
    }

    private void TimerCheck(ref bool tswitch, ref float timer)
    {
        if (tswitch)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                timer = 0.0f;
                tswitch = false;
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
            if(knockback)
            {
                StartCoroutine(PlaySmoke(0.3f));
                knockback = false;
                if(!landingBooster)
                {
                    stunning = true;
                    stunTimer = 2.0f;
                    currentSpeed = 0;
                }
            }
 
            landing = true;
            singleJump = false;
            doubleJump = false;
            landingCheck = false;

            if (landingBooster)
            {
                StartCoroutine(PlaySmoke(0f));
                landingBooster = false;
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
                    BoosterOff();

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
            //animator.SetTrigger("BackFlip");
            KnockBackCollision();
                
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


    private void HandleHurdleCollision(GameObject hurdle)
    {
        Vector3 playerDirection = transform.forward;

        currentSpeed = 0;
        rigid.velocity = playerDirection * 5f;

    }
    private void KnockBackCollision()
    {
        knockback = true;
        landing = false;
        currentSpeed = 0;

        BoosterOff();
        
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

    }

    public void Controlparalysis()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            paralysisTimer -= Time.deltaTime;
        }
    }


    public void PlayerBoosterGauge()
    {
        if (drainedGauge >= 2.0f && landing)
        {
            drainedGauge = 0.0f;
            currentSpeed = 0f;
            draining = true;
            drainingTimer = 2.0f;
            animator.SetTrigger("Drained");

        }

        if (currentMaxSpeed >= 20 && currentSpeed > 0 && !boosterOnPad)
        {
            boosterGauge -= Time.deltaTime;
            if (boosterGauge <= 0)
            {
                boosterGauge = 0;

                if (!draining)
                {
                    drainedGauge += (Time.deltaTime * 2);
                }
            }
        }
        else if (drainedGauge > 0)
        {
            drainedGauge -= Time.deltaTime;
            if (drainedGauge <= 0)
            {
                drainedGauge = 0;
            }
        }
        else
        {
            boosterGauge += (Time.deltaTime / 5) ;
            if (boosterGauge >= boosterMaxGauge)
            {
                boosterGauge = boosterMaxGauge;
            }
        }
    }

    void BoosterOff()
    {
        boosterOnPad = false;
        boosterOnKey = false;
        boosterTimer = 0f;
    }
}




public partial class PlayerController : MonoBehaviour
{

    //PlayerControl partial
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

    private void InputReserveArrow()
    {
        inputDir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputDir -= Vector2.up;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputDir -= Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputDir -= Vector2.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputDir -= Vector2.right;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (singleJump == false)
            {
                PlayJumpEffect();
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

    public void LandingBooster()
    {
        if (landingCheck)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                knockback = false;
                landingBooster = true;
            }
        }
    }

    private void PlayerBooster()
    {
        if (Input.GetKey(KeyCode.Z) && !boosterOnPad)
        {
            boosterOnKey = true;
            currentMaxSpeed = baseMaxSpeed + boosterMaxSpeed;
            currentAccel = baseAccel + boosterAddAccel;
        }
        else
        {
            boosterOnKey = false;
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

            if (directionCheck < 1.0f || directionCheck > 359.0f)
            {

            }
            else if (directionCheck < 170.0f)
            {
                transform.rotation = Quaternion.Euler(0.0f, playerDegree + baseRotSpeed * Time.deltaTime, 0.0f);
            }
            else if (directionCheck > 190.0f)
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
        animator.SetBool("Jumping", singleJump);
        animator.SetBool("DoubleJumping", doubleJump);
        animator.SetBool("Breaking", breaking);
        animator.SetBool("Knockback", knockback);
        animator.SetBool("Stunning", stunning);

        if (boosterTimer > 0.0f) animator.SetBool("Booster", true);
        else animator.SetBool("Booster", false);

        if ((boosterOnKey == true || boosterOnPad == true) && landing && !draining && !stunning && !knockback && !paralysis)
        {
            boosterEffect.SetActive(true);
        }
        else
        {
            boosterEffect.SetActive(false);
        }

    }

    private IEnumerator PlaySmoke(float sec)
    {
        yield return new WaitForSeconds(sec);
        GameObject buf = Instantiate(smokeEffect);
        buf.transform.position = transform.position + transform.forward * (currentSpeed / 10.0f);
        buf.SetActive(true);
    }

    private void PlayJumpEffect()
    {
        GameObject buf = Instantiate(jumpEffect);
        buf.transform.position = transform.position + transform.forward * (currentSpeed / 15.0f);
        buf.SetActive(true);
    }
}