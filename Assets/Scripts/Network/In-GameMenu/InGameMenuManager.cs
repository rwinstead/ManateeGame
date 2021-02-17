using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class InGameMenuManager : MonoBehaviour
{
    private NetworkManagerMG NetworkManager;
    
    public bool isLeader;

    [SerializeField]
    private GameObject InGameMenuCanvas;

    private bool CanvasIsActive = false;

    private void Start()
    {
            NetworkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerMG>();           
    }

    public void CloseGameMenu()
    {
        CanvasIsActive = !CanvasIsActive;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasIsActive = !CanvasIsActive;
        }

        if (CanvasIsActive)
        {
            InGameMenuCanvas.SetActive(true);
        }
        else
        {
            InGameMenuCanvas.SetActive(false);
        }
    }

}
