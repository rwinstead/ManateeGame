using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinManager : MonoBehaviour
{

    private int coinsCollected;
    public TMP_Text coinDisplay;

    // Start is called before the first frame update
    void Start()
    {
        coinDisplay = GameObject.FindGameObjectWithTag("coinText").GetComponent<TMP_Text>();
        coinsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        coinDisplay.text = "Coins: " + coinsCollected;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Collectable"))
        {
            coinsCollected++;
            Debug.Log("Got Coin! New Total: "+coinsCollected);
            other.gameObject.SetActive(false);
        }
    }
}
