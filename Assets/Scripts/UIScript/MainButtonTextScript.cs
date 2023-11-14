using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainButtonTextScript : MonoBehaviour
{
    public TextMeshProUGUI myText;

    // Start is called before the first frame update
    private void Update()
    {
    }

    // Update is called once per frame
    public void OnMouseEnter()
    {
        Debug.Log("Ω««‡¿Ã æ»µ ?");
        myText.color = Color.black;
    }

    public void OnMouseExit()
    {
        myText.color = Color.white;
    }
}
