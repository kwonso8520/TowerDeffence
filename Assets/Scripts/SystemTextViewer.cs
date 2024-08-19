using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public enum SystemType { Money = 0, Build }

public class SystemTextViewer : MonoBehaviour
{
    private Text systemTxt;
    private TextAlpha textAlpha;

    private void Awake()
    {
        systemTxt = GetComponent<Text>();   
        textAlpha = GetComponent<TextAlpha>();
    }
    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                systemTxt.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                systemTxt.text = "System : Invalid build tower...";
                break;
        }
        textAlpha.FadeOut();
    }
}
