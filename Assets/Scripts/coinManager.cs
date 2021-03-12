using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class coinManager : NetworkBehaviour
{

    public AudioSource audioSource;
    public AudioClip coinSound;
    public static event Action collectCoin;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<LinkToGamePlayer>().thisPlayer.hasAuthority)
            {
                collectCoin?.Invoke();
            }
        audioSource.PlayOneShot(coinSound, 0.5f);
        Debug.Log("Play sound");
        gameObject.SetActive(false);
        }
    }
}
