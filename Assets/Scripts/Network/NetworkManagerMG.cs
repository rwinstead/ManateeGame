﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class NetworkManagerMG : NetworkManager
{

    //just using this to test connection
    public int thisGuy = 42;

    [SerializeField] private int minPlayers = 1;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;

    [Header("In-Game")]
    [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    [SerializeField] private GameObject ScoreKeeperPrefab = null;
    [SerializeField] private GameObject StageStartCountdown = null;
    [SerializeField] private GameObject RaceModeHUD = null;

    private string selectedMap = "MarbleRun_active";

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();

    public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action ServerStopped;
    public static event Action ServerChangedLevel;

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

        //Stops people from joining if game is in progress. Commented out for testing.

        /*

        if ("Assets/Scenes/ActiveScenes/" + SceneManager.GetActiveScene().name + ".unity" != menuScene)
        {
            conn.Disconnect();
            return;
        }

        */

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {

        //if("Assets/Scenes/ActiveScenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene) //only add player to lobby if we're in the menu screen
        

            bool isLeader = RoomPlayers.Count == 0; //Room leader if you're first to join

            Debug.Log("player added to server. spawning player prefab.");
            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);



    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkBehaviour>(); //Refrence to the player who disconnected

            if (player is NetworkRoomPlayer roomPlayer)
            {

                RoomPlayers.Remove(roomPlayer); //Removes disconnected player from list of current players

                NotifyPlayersOfReadyState();
            }

            else if (player is NetworkGamePlayer gamePlayer)
            {
                GamePlayers.Remove(gamePlayer);
            }

        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        ServerStopped?.Invoke();
        RoomPlayers.Clear();
        GamePlayers.Clear();
        //base.OnStopServer();
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
        if ("Assets/Scenes/ActiveScenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
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

        if ("Assets/Scenes/ActiveScenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
        {

            for (int i = RoomPlayers.Count -1; i >= 0; i--)
            {
                Debug.Log("Room player number: " + i);
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                if (i == 0) { gameplayerInstance.isLeader = true; }
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);
                GamePlayers.Add(gameplayerInstance);
            }

            var RaceModeHUDInstance = Instantiate(RaceModeHUD);
            var ScoreKeeperInstance = Instantiate(ScoreKeeperPrefab);

            //These references will only work for the server
            RaceModeHUDInstance.GetComponent<RaceScoreboard>().networkScoreKeeper = ScoreKeeperInstance.GetComponent<NetworkScoreKeeper>();
            ScoreKeeperInstance.GetComponent<NetworkScoreKeeper>().stageTimer = RaceModeHUDInstance.GetComponent<ServerStageTimer>();
            ScoreKeeperInstance.GetComponent<NetworkScoreKeeper>().raceScoreboard = RaceModeHUDInstance.GetComponent<RaceScoreboard>();
            

            NetworkServer.Spawn(ScoreKeeperInstance);
            NetworkServer.Spawn(RaceModeHUDInstance);

        }

            base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName != "MainMenu")
        {
            GameObject StageStartCountdownInstance = Instantiate(StageStartCountdown);
            NetworkServer.Spawn(StageStartCountdownInstance);

            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            //NetworkScoreKeeper.UpdateScorekeeperNewRound();
            ServerChangedLevel?.Invoke();
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
            selectedMap = "MarbleRun_active";
        }

        if(mapName == 1)
        {
            selectedMap = "Racing01_active";
        }

        if(mapName == 2)
        {
            selectedMap = "Racing02_active";
        }

    }

}
