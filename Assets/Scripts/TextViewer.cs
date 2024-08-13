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

    [SerializeField]
    private Text playerGoldTxt;
    [SerializeField]
    private PlayerGold playerGold;

    void Update()
    {
        playerHpTxt.text = playerHp.CurrentHp + " / " + playerHp.MaxHp;
        playerGoldTxt.text = playerGold.CurrentGold.ToString();
    }
}
