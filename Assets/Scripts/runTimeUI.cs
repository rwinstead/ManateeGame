using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class runTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text runTimeText;

    private void OnEnable()
    {
        runTimeText.text = "0:00";
    }

    private void updateCoinCount()
    {

        

    }


}
