using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class NetworkManagerLobby : NetworkManager
{

    [SerializeField] private int minPlayers = 1;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;

    [Header("In-Game")]
    [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;

    private string selectedMap = "MarbleRun_Ryan_networked";

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();

    public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public override void OnStartServer()
    {
        //base.OnStartServer();
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        //base.OnStartClient();

        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        //Stops people from joining if game is in progress
        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != menuScene)
        {
            conn.Disconnect();
            return;
        }

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);
        // Debug.Log(SceneManager.GetActiveScene());
        // Debug.Log(menuScene);
        //Debug.Log(SceneManager.GetActiveScene().name == menuScene);

        if("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene) //only add player to lobby if we're in the menu screen
        {

            bool isLeader = RoomPlayers.Count == 0; //Room leader if you're first to join

            Debug.Log("player added to server. spawning player prefab.");
            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }


    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>(); //Refrence to the player who disconnected

            RoomPlayers.Remove(player); //Removes disconnected player from list of current players

            NotifyPlayersOfReadyState();

        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        //base.OnStopServer();
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;


    }

    public void StartGame()
    {
        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }

            ServerChangeScene(selectedMap);
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {

        //From menu to game

        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
        {
            for (int i = RoomPlayers.Count -1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);

            }
        }

            base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName != "MainMenu")
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }



    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    public void selectMap(int mapName)
    {
        Debug.Log("Host has selected " + mapName);

        if(mapName == 0)
        {
            selectedMap = "MarbleRun_Ryan_networked";
        }

        if(mapName == 1)
        {
            selectedMap = "Racing_Multi_networked";
        }
    }





}
