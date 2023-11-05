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

    public bool freezing;       //��� Ű �Է� �Ұ�
    public bool immovable;      //ȸ�� ����
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

    private void BoosterOn()
    {

    }


    private void PlayerControl()
    {

        //�÷��̾��� ����� �Է��� ��ġ�ϸ� �ӵ� ����,
        //��ġ���� ������ �Է� �������� ȸ��,
        //�Է����� ������ �ӵ� ����
        //���� ��ÿ� �ݶ��̴� �ʿ�

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

}
