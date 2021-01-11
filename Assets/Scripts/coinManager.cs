using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinManager : MonoBehaviour
{

    private int coinsCollected;
    // Start is called before the first frame update
    void Start()
    {
        coinsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
