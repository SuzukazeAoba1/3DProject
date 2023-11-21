using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Close();
    }
    void Update()
    {
        if (GameManager.instance.gameWin)
            Open();
    }
    public void Open()
    {
        animator.SetBool("Open", true);
    }
    public void Close()
    {
        animator.SetBool("Open", false);
    }
}
