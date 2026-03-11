using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText; 

    void Update()
    {
        if(GameManager.instance != null)
        {
            goldText.text = " " + GameManager.instance.currentGold.ToString("F0");
		}
	}

}
