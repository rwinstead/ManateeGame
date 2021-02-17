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
        NetworkManagerMG.ServerChangedLevel += ResetCoinCount;
    }

    private void updateCoinCount()
    {
        coinCount++;
        coinCountText.text = "Coins: " + coinCount;

    }

    private void ResetCoinCount()
    {
        coinCount = 0;
        coinCountText.text = "Coins: " + coinCount;
    }

    private void OnDestroy()
    {
        coinManager.collectCoin -= updateCoinCount;
        NetworkManagerMG.ServerChangedLevel -= ResetCoinCount;
    }

    private void OnDisable()
    {
        coinManager.collectCoin -= updateCoinCount;
        NetworkManagerMG.ServerChangedLevel -= ResetCoinCount;
    }


}
