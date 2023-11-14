using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIScript : MonoBehaviour
{
    public Image mapNameImage;
    public Image mapSetImage;
    public TextMeshProUGUI mapInfoText;
    public Button chekcButton;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        MapUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapUpdate()
    {
    }

    public void OnClickTutoMap()
    {
    }

    public void onClickWholeMap()
    {
    }
}
