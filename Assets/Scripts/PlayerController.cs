using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public GameObject player;

    public GameObject StunUI;
    public GameObject knockBackCollider;
    public CameraController cameraTarget;
    public GameObject boosterEffect;
    public GameObject smokeEffect;
    public GameObject jumpEffect;
    public LandingCheck landingColider;

    public bool living;

    public bool readying;
    public bool readyKeyInput;
    public bool readySuccess;
    public bool readyFailure;
    
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

    public bool freezing;       //모든 키 입력 불가
    public bool immovable;      //회전만 가능
    public bool breaking;        //점프만 가능 (가다 멈추기)
    public bool keyReverse;     //방향 키 입력 반전
    public bool backtrip;
    public bool fronttrip;
    public bool stunning;
    public bool draining;
    public bool invincibility;

    public bool knockback;
    public bool landingBooster;
    public bool landingCheck;
    public bool landingKey;

    public float freezingTimer;
    public float immovableTimer;
    public float breakingTimer;
    public float keyReverseTimer;
    public float tripTimer;
    public float stunTimer;
    public float boosterTimer;
    public float drainingTimer;
    public float invincibilityTimer;

    private bool footSoundCheck;

    private bool restricted;

    private void Start()
    {
        living = true;
        readying = true;

        rigid = GetComponent<Rigidbody>();
        boosterGauge = boosterMaxGauge;
        animator = player.gameObject.GetComponent<Animator>();
        particle = smokeEffect.gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (GameManager.instance.gameLose || GameManager.instance.gameWin)
        {
            currentSpeed = 0;
            return;
        }

        GameStart();

        if (!readying)
        {
            if (living)
            {
                PlayerControl();
                StateTimerCheck();
                SoundCheck();

                if (boosterOnPad && !stunning && !knockback && !backtrip && !fronttrip)
                {
                    Booster();
                }

                if (stunning)
                {
                    Controlparalysis();
                }

                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            }
        }
    }


    private void PlayerControl()
    {


        if (!stunning && !backtrip && !fronttrip && !knockback && !draining && !freezing && !breaking)
        {
            if (!keyReverse)
                InputArrow();
            else
                InputReserveArrow(); 
        }

        if (!stunning && !backtrip && !fronttrip && !knockback && !draining && !freezing && !breaking)
        {
            PlayerBooster();
        }

        if (!stunning && !backtrip && !fronttrip && !knockback && !draining && !freezing)
        {
            PlayerJump();
        }

        if (!stunning && !backtrip && !fronttrip && !knockback && !draining)
        {
            PlayerRotate();
            PlayerAddSpeed();
        }

        if (!stunning && !backtrip && !fronttrip && !knockback)
        {
            PlayerBoosterGauge();
        }

        if (!stunning && !backtrip && !fronttrip)
        {
            LandingBooster();
            LandingKeyChecking();
        }

        StunTime(); 
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
        TimerCheck(ref backtrip, ref tripTimer);
        TimerCheck(ref fronttrip, ref tripTimer);
        TimerCheck(ref draining, ref drainingTimer);
        TimerCheck(ref invincibility, ref invincibilityTimer);
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
        Debug.Log(collision);
        if (collision.gameObject.tag == "Ground")
        {
            if(knockback)
            {
                StartCoroutine(PlaySmoke(0.3f));
                knockback = false;
                if(!landingBooster)
                {
                    backtrip = true;
                    tripTimer = 2.0f;
                    currentSpeed = 0;
                }
            }
 
            landing = true;
            singleJump = false;
            doubleJump = false;
            landingCheck = false;
            landingKey = false;

            if (landingBooster)
            {
                invincibility = true;
                invincibilityTimer = 0.5f;

                LandBooster();
            }

        }
        animator.SetBool("Landing", landing);

        if (collision.gameObject.tag == "Restricted")
        {
            if(knockback)
            {
                knockback = false;
                backtrip = true;
                tripTimer = 2.0f;
                landing = true;
            }

            restricted = true;
        }

        if (collision.gameObject.CompareTag("Hurdle"))
        {
            if (invincibility)
                return;

            while (true)
            {
                if (!fronttrip)
                {
                    invincibility = true;
                    invincibilityTimer = 2.5f;

                    fronttrip = true;
                    tripTimer = 2.0f;
                    BoosterOff();

                    currentSpeed = 0;
                    animator.SetTrigger("Tripped");
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.OuchVoice);
                    StartCoroutine(PlaySmoke(0.1f));
                }
                else
                {
                    HandleHurdleCollision(collision.gameObject);
                    return;
                }
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Restricted")
        {
            if (currentSpeed > 5.0f)
            {
                currentSpeed = 5.0f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Restricted")
        {
            restricted = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if (other.gameObject.CompareTag("Hurdle"))
        {
            if (invincibility)
                return;

            while (true)
            {
                if (!fronttrip)
                {
                    invincibility = true;
                    invincibilityTimer = 2.5f;

                    fronttrip = true;
                    tripTimer = 2.0f;
                    BoosterOff();

                    currentSpeed = 0;
                    animator.SetTrigger("Tripped");
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.OuchVoice);
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
            if(landingBooster)
            {
                knockback = false;
                landingBooster = false;
                landingCheck = false;
                landingKey = false;

                invincibility = true;
                invincibilityTimer = 0.5f;

                LandBooster();

                landing = true;
            }

            if (invincibility)
                return;

            knockback = false;
            backtrip = false;



            if (!knockback)
            {
                AudioManager.instance.PlayKnockBack();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.OuchVoice);
            }

            knockBackCollider.SetActive(true);
            KnockBackCollision();
        }

        if (other.gameObject.CompareTag("Booster"))
        {
            if (!stunning && !backtrip && !fronttrip)
            {
                AudioManager.instance.PlayBooster();
                boosterOnPad = true;
                boosterTimer = 2.0f;
            }
        }

        if (other.gameObject.CompareTag("Par"))
        {
            if (invincibility)
                return;

            if (!stunning)
            {
                StunUI.SetActive(true);

                AudioManager.instance.PlaySfx(AudioManager.Sfx.Stunning);
                currentSpeed = 0;
                stunning = true;
                stunTimer = 5.0f;
            }
        }

        if (other.gameObject.CompareTag("Reserve"))
        {
            if (invincibility)
                return;

            if (!keyReverse)
            {
                keyReverse = true;
                keyReverseTimer = 5.0f;
            }
        }
    }


    private void HandleHurdleCollision(GameObject hurdle)
    {
        Vector3 playerDirection = transform.forward;

        currentSpeed = 0;
        rigid.velocity = playerDirection * 7f;

    }
    private void KnockBackCollision()
    {
        invincibility = true;
        invincibilityTimer = 2.0f;

        knockback = true;
        landing = false;
        currentSpeed = 0;

        BoosterOff();
        
        Vector3 playerDirection = -transform.forward.normalized;
        Vector3 highVector = new Vector3(0, 1.5f, 0);

        rigid.velocity = (playerDirection + highVector) * 6f;
    }


    private void Booster()
    {
        breaking = false;
        breakingTimer = 0.0f;

        Vector2 playerDirection = transform.forward;
        inputDir = playerDirection;

        currentMaxSpeed = baseMaxSpeed + boosterMaxSpeed;
        if(restricted)
        {
            currentSpeed = 5.0f;
        }
        else
        {
            currentSpeed = currentMaxSpeed;
        }
    }

    public void Controlparalysis()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            stunTimer -= 0.5f;
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
                    drainedGauge += (Time.deltaTime);
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
            boosterGauge += (Time.deltaTime / 3) ;
            if (boosterGauge >= boosterMaxGauge)
            {
                boosterGauge = boosterMaxGauge;
            }
        }
    }

    public void BoosterOff()
    {
        boosterOnPad = false;
        boosterOnKey = false;
        boosterTimer = 0f;
        boosterEffect.SetActive(false);
        AudioManager.instance.StopBooster();
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
            inputDir += Vector2.down;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputDir += Vector2.up;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputDir += Vector2.right;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputDir += Vector2.left;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (singleJump == false)
            {
                int ran = Random.Range(0, 2);
                if (ran == 0)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.FstJump);
                else
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.FstJump2);
                PlayJumpEffect();
                rigid.AddForce(Vector3.up * baseJumpPower);
                landing = false;
                singleJump = true;
            }
            else if (doubleJump == false)
            {
                int ran = Random.Range(0, 2);
                if (ran == 0)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.SndJump);
                else
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.SndJump2);
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
    private void GameStart()
    {
        if (!readying)
            return;

        StartKeyInput();

        if (GameManager.instance.gameStart == true)
        {
            if (readyFailure)
            {
                fronttrip = true;
                tripTimer = 2.0f;
            }
            else if (readySuccess)
            {
                AudioManager.instance.PlayBooster();
                boosterOnPad = true;
                boosterTimer = 2.0f;
            }

            readyFailure = false;
            readySuccess = false;
            readyKeyInput = false;
            readying = false;
        }
    }

    void StartKeyInput()
    {
        float startTime = GameManager.instance.startTimer;
        if (!readyKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (startTime <= 0.3)
                {
                    readyFailure = false;
                    readySuccess = true;

                    readyKeyInput = true;
                }
            }
        }
    }

    public void LandingBooster()
    {
        if (landingCheck)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!landingKey)
                {
                    landingBooster = true;
                    landingKey = false;
                }
            }
        }
    }

    public void LandingKeyChecking()
    {
        if (knockback && !landingCheck)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                landingKey = true;
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


    private void PlayerRotate()
    {
        if (inputDir != Vector2.zero)
        {
            float inputDegree = (450.01f - (Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg)) % 360.0f - 0.01f;
            inputDegree = Mathf.Round(inputDegree);

            float cameraDegree = cameraTarget.cameraAngle;
            float playerDegree = transform.rotation.eulerAngles.y;
            float addCamDir = (360.0f - (cameraDegree - playerDegree)) % 360.0f;
            float directionCheck = (360.0f + inputDegree - addCamDir) % 360.0f;

            transform.rotation = Quaternion.Euler(0.0f, cameraDegree + inputDegree, 0.0f);

            //Debug.Log("input" + inputDegree + "///" + "camera" + cameraDegree + "///" + "player" + playerDegree + "///" + directionCheck + "///" );

            if (directionCheck < 2.0f || directionCheck > 358.0f)
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
                if (currentSpeed < 5.0f && !breaking)
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
        if (landing == true || (singleJump == true && doubleJump == false))
        {
            fallCountTimer = 0.0f;

            if (inputDir == Vector2.zero)
            {
                if (!boosterOnPad)
                    if (breaking == false && currentSpeed > 8.0f && !singleJump)
                    {
                        breaking = true;
                        breakingTimer = 0.6f;
                        StartCoroutine(PlaySmoke(0.2f));
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.BrakingSound);
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
        animator.SetBool("Backtrip", backtrip);
        animator.SetBool("Stunning", stunning);

        if (drainedGauge > 0.0f && boosterOnKey) animator.SetBool("Draining", true);
        else animator.SetBool("Draining", false);

        if (boosterTimer > 0.0f) animator.SetBool("Booster", true);
        else animator.SetBool("Booster", false);

        if ((boosterOnKey == true || boosterOnPad == true) && landing && !draining && !stunning && !knockback && !backtrip && !fronttrip)
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
        StartCoroutine(DestroyJump(buf));
    }

    private IEnumerator DestroyJump(GameObject buf)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(buf);
    }

    private void SoundCheck()
    {
        if (boosterOnPad && boosterTimer <= 0.1)
        {
            AudioManager.instance.StopBooster();
        }

        if (!boosterOnPad && !boosterOnKey)
        {
            AudioManager.instance.StopBooster();
        }

        if (draining) AudioManager.instance.StopBooster();

        if (landing && !draining && !stunning && !knockback && !backtrip && !fronttrip)
        {
            if (currentSpeed > 3.0f)
            {
                if (!footSoundCheck)
                {
                    AudioManager.instance.PlayFoot();
                    footSoundCheck = true;
                    Debug.Log("저주받은 발소리");
                }
            }
            else
            {
                AudioManager.instance.StopFoot();
                footSoundCheck = false;
            }
        }
        else
        {
            AudioManager.instance.StopFoot();
            footSoundCheck = false;
        }

    }

    public void LandBooster()
    {
        AudioManager.instance.PlayBooster();

        StartCoroutine(PlaySmoke(0f));
        landingBooster = false;
        boosterOnPad = true;
        boosterTimer = 1.0f;
    }

    public void StunTime()
    {
        if(stunning && stunTimer <= 0.1f)
        {
            StunUI.SetActive(false);

            invincibility = true;
            invincibilityTimer = 0.5f;
        }
    }
}