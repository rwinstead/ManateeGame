using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject networkManagerPrefab;

    private GameObject networkManager;

    private void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");

        if(networkManager == null)
        {
            Instantiate(networkManagerPrefab);
        }
    }

}
