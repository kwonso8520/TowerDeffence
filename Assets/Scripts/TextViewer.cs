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

    [SerializeField]
    private Text waveTxt;
    [SerializeField]
    private WaveSystem wave;

    [SerializeField]
    private Text enemyCountTxt;
    [SerializeField]
    private EnemySpawner enemySpawner;

    void Update()
    {
        playerHpTxt.text = playerHp.CurrentHp + " / " + playerHp.MaxHp;
        playerGoldTxt.text = playerGold.CurrentGold.ToString();
        waveTxt.text = wave.CurrentWave + " / " + wave.MaxWave;
        enemyCountTxt.text = enemySpawner.CurrentEnemyCount + " / " + enemySpawner.MaxEnemyCount;
    }
}
