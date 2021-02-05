using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class InGameMenuManager : MonoBehaviour
{
    private NetworkManagerLobby NetworkManager;
    
    public bool isLeader;

    private void Start()
    {
            NetworkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerLobby>();           
    }

    public void CloseGameMenu()
    {
        gameObject.SetActive(false);
    }

    public void ExitToMenu()
    {

        Debug.Log(isLeader);
        if (isLeader)
        {
            Debug.Log("Stopping host " + NetworkManager);

            NetworkManager.StopHost();
        }

        else
        {
            NetworkManager.StopClient();
        }
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

}
