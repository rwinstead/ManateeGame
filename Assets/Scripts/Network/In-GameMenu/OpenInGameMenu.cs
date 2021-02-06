using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OpenInGameMenu : NetworkBehaviour
{

    [SerializeField] private GameObject InGameMenuPrefab;

    private GameObject InGameMenu;

    private void Start()
    {
        if (!hasAuthority) { return; }
        if(InGameMenu == null)
        {
            InGameMenu = Instantiate(InGameMenuPrefab);
            InGameMenu.GetComponent<InGameMenuManager>().isLeader = gameObject.GetComponent<NetworkGamePlayer>().isLeader;
            DontDestroyOnLoad(InGameMenu);
        }
    }

    void Update()
    {
        if(!hasAuthority) { return; }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenu.SetActive(true);
        }
    }









}
