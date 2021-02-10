using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private NetworkManagerMG networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;

    private void OnEnable()
    {
        if(networkManager == null)
        {
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerMG>();
        }
    }

    public void HostLobby()
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }




}
