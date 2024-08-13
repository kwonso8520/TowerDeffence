using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextViewer : MonoBehaviour
{
    [SerializeField]
    private Text playerHpTxt;
    [SerializeField]
    private PlayerHp playerHp;

    void Update()
    {
        playerHpTxt.text = playerHp.CurrentHp + " / " + playerHp.MaxHp;
    }
}
