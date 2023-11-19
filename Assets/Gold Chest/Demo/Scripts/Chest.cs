using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem openfx;
    void Start()
    {
        openfx.Stop();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
    }
    public void Open()
    {
        animator.SetBool("Open", true);
    }
    public void Close()
    {
        animator.SetBool("Open", false);
    }
    void OpenFX()
    {
        openfx.Play();
    }
}
