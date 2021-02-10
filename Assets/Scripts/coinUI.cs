using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class coinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCountText;

    private int coinCount = 0;
    private void OnEnable()
    {
        coinManager.collectCoin += updateCoinCount;
        coinCountText.text = "Coins: 0";
    }

    private void updateCoinCount()
    {
        coinCount++;
        coinCountText.text = "Coins: " + coinCount;

    }

    private void OnDestroy()
    {
        coinManager.collectCoin -= updateCoinCount;
    }

    private void OnDisable()
    {
        coinManager.collectCoin -= updateCoinCount;
    }


}
