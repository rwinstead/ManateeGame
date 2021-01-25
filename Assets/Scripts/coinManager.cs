using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class coinManager : NetworkBehaviour
{

    public static event Action collectCoin;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<NetworkIdentity>().hasAuthority)
            {
                collectCoin?.Invoke();
            }
            gameObject.SetActive(false);
        }
    }
}
