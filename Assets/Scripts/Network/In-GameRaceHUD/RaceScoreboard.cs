using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System.Linq;

public class RaceScoreboard : NetworkBehaviour
{

    //Headers in board
    [Header("Scoreboard headers")]
    [SerializeField] private Transform StageRowTransformContainer;
    [SerializeField] private Transform StageRowTemplate;
    [SerializeField] private Transform CollectiblesHeader;
    [SerializeField] private Transform TotalScoreHeader;

    //Player score row backgrounds
    [Header("Player score rows")]
    [SerializeField] private Transform PlayerScoresBackgroundContainer;
    [SerializeField] private Transform PlayerScoresBackgroundTemplate;
    [SerializeField] private Transform PlayerScoresBackground;

    //Player score tiles
    [SerializeField] private Transform PlayerScoresContainer;
    [SerializeField] private Transform PlayerScoresTemplate; //This element contains the text mesh pro component

    public NetworkScoreKeeper networkScoreKeeper;

    private int StagesCount = 3;
    private int TotalNumberOfHeaders;
    private float StageRowWidth = 130;
    private float ScoreHeight = 75;
    private float TotalUIWidth = 0;

    private bool CanvasIsActive = false;

    [SerializeField]
    private GameObject ScoreboardCanvas;

    [SyncVar]
    public int CurrentMapIndex;

    //Syncvars below are used for initialization on the client side

    [SyncVar]
    public int NumberOfPlayers;

    SyncList<string> ListOfPlayerNames = new SyncList<string>();
    SyncList<uint> ListOfPlayerNetIds = new SyncList<uint>();

    Dictionary<uint, NetworkScoreKeeper.PlayerData> SyncedPlayerScores = new Dictionary<uint, NetworkScoreKeeper.PlayerData>();

    Dictionary<uint, List<TextMeshProUGUI>> CanvasScores = new Dictionary<uint, List<TextMeshProUGUI>>();

    private NetworkManagerMG room;

    public NetworkManagerMG Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }

            return room = NetworkManager.singleton as NetworkManagerMG;
        }
    }

    /*
     * Getting the # of players must be called before start. If called in start, the syncvar won't be updated in time
     * for the client, and it will call start with number of players = 0.
     */
    private void Awake()
    {
        NumberOfPlayers = Room.GamePlayers.Count;
        TotalNumberOfHeaders = StagesCount + 3;

        foreach(var player in Room.GamePlayers)
        {
            ListOfPlayerNames.Add(player.displayName);
            ListOfPlayerNetIds.Add(player.netId);
        }

    }

    private void Start()
    {

        InitializeScoreboard();

    }

    private void IncreaseUIWidth()
    {
        TotalUIWidth += StageRowWidth;
    }

    private void UpdateScoreBoardCanvas(uint netId, int collectibles, int position)
    {
        CanvasScores[netId][CurrentMapIndex].SetText(position.ToString());

        if(CanvasScores[netId][CanvasScores[netId].Count - 1].text != " ")
        {
            int CollectiblesIndex = CanvasScores[netId].Count - 2;
            CanvasScores[netId][CollectiblesIndex].text = (int.Parse(CanvasScores[netId][CollectiblesIndex].text) + collectibles).ToString();

            int TotalIndex = CanvasScores[netId].Count - 1;
            CanvasScores[netId][TotalIndex].text = (int.Parse(CanvasScores[netId][TotalIndex].text) + position).ToString();
        }
        else
        {
            int CollectiblesIndex = CanvasScores[netId].Count - 2;
            CanvasScores[netId][CollectiblesIndex].text = collectibles.ToString();

            int TotalIndex = CanvasScores[netId].Count - 1;
            CanvasScores[netId][TotalIndex].text = position.ToString();
        }


    }

    [Server]
    public void UpdateScoreboard(Dictionary<uint, NetworkScoreKeeper.PlayerData> PlayerScores, uint FinishedNetId)
    {
        int NewScoreIndex = 0;
        foreach (KeyValuePair<uint, NetworkScoreKeeper.PlayerData> pair in PlayerScores)
        {
            SyncedPlayerScores[pair.Key] = pair.Value;

            /*
             * Below sends entire arrays of values, but not needed unless player joins mid-way through.
            int[] StageCollectibles = pair.Value.StageCollectibles.ToArray();
            int[] StageFinishPosition = pair.Value.StageFinishPosition.ToArray();
            double[] StageTime = pair.Value.StageTime.ToArray();
            */           
        }

        if (PlayerScores[FinishedNetId].StageCollectibles.Count > 0)
        {
            NewScoreIndex = PlayerScores[FinishedNetId].StageCollectibles.Count - 1;
        }
        UpdateScoreBoardCanvas(FinishedNetId, PlayerScores[FinishedNetId].StageCollectibles[NewScoreIndex], PlayerScores[FinishedNetId].StageFinishPosition[NewScoreIndex]);
        //This awful-looking function sends the client the playerscore values for the player that just finished, so they can update their scoreboard.
        RpcSendScoreData(FinishedNetId, PlayerScores[FinishedNetId].DisplayName, PlayerScores[FinishedNetId].StageCollectibles[NewScoreIndex],
                PlayerScores[FinishedNetId].StageFinishPosition[NewScoreIndex], PlayerScores[FinishedNetId].StageTime[NewScoreIndex]);      


        PrintDict();
    }

    [ClientRpc]

    private void RpcSendScoreData(uint netId, string DisplayName, int collectibles, int position, double time)
    {
        if (!isServer)
        {
            if (SyncedPlayerScores.ContainsKey(netId))
            {
                SyncedPlayerScores[netId].StageCollectibles.Add(collectibles);
                SyncedPlayerScores[netId].StageFinishPosition.Add(position);
                SyncedPlayerScores[netId].StageTime.Add(time);
                PrintDict();
                UpdateScoreBoardCanvas(netId, collectibles, position);
            }

            else //Should never be called, but just in case
            {
                SyncedPlayerScores[netId] = new NetworkScoreKeeper.PlayerData();
                SyncedPlayerScores[netId].DisplayName = DisplayName;
                SyncedPlayerScores[netId].StageCollectibles.Add(collectibles);
                SyncedPlayerScores[netId].StageFinishPosition.Add(position);
                SyncedPlayerScores[netId].StageTime.Add(time);
                PrintDict();
            }
        }
        
    }


    private void InitializeScoreboard()
    {

        // ADDS GAMEPLAYERS TO SCOREBOARD DICTIONARY ***********************************************

        for (int i = 0; i < ListOfPlayerNetIds.Count; i++)
        {
            uint initNetId = 0;
            initNetId = ListOfPlayerNetIds[i];
            SyncedPlayerScores[initNetId] = new NetworkScoreKeeper.PlayerData();
            SyncedPlayerScores[initNetId].DisplayName = ListOfPlayerNames[i];
        }

        // GENERATES HEADERS ************************************************************************
        float containerWidth = StageRowTransformContainer.GetComponent<RectTransform>().anchoredPosition.x;
        float containerHeight = StageRowTransformContainer.GetComponent<RectTransform>().anchoredPosition.y;

        StageRowTemplate.gameObject.SetActive(false);
        PlayerScoresBackgroundTemplate.gameObject.SetActive(false);
        PlayerScoresTemplate.gameObject.SetActive(false);

        for (int i = 0; i < StagesCount; i++) //Generates and places all headers
        {
            var stageRow = Instantiate(StageRowTemplate, StageRowTransformContainer);
            stageRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(StageRowWidth * i, 0);
            stageRow.GetComponent<TextMeshProUGUI>().text = "" + (i + 1);
            stageRow.gameObject.SetActive(true);
            IncreaseUIWidth();
        }

        CollectiblesHeader.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalUIWidth + containerWidth, containerHeight);
        IncreaseUIWidth();

        TotalScoreHeader.GetComponent<RectTransform>().anchoredPosition = new Vector2(TotalUIWidth + containerWidth, containerHeight);
        IncreaseUIWidth();

        // GENERATES PLACEHOLDERS FOR SCORES ***********************************************************

        PlayerScoresBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(130 * (TotalNumberOfHeaders), ScoreHeight);

        for (int i = 0; i < NumberOfPlayers; i++) // Creates the background for each player score row
        {
            var PlayerBkgScoreRow = Instantiate(PlayerScoresBackgroundTemplate, PlayerScoresBackgroundContainer);
            var ScoreBkgRectTransform = PlayerBkgScoreRow.GetComponent<RectTransform>();
            ScoreBkgRectTransform.anchoredPosition = new Vector2(0, i * -ScoreHeight);
            PlayerBkgScoreRow.gameObject.SetActive(true);
        }

        for (int i = 0; i < NumberOfPlayers; i++) //Creates each cell in the scoreboard table that contains player data
        {
            uint initNetId = 0;
            initNetId = ListOfPlayerNetIds[i];
            CanvasScores.Add(initNetId, new List<TextMeshProUGUI>());

            for (int k = 0; k < TotalNumberOfHeaders; k++)
            {
                var PlayerScoreRow = Instantiate(PlayerScoresTemplate, PlayerScoresContainer);
                var ScoreRectTransform = PlayerScoreRow.GetComponent<RectTransform>();
                ScoreRectTransform.anchoredPosition = new Vector2(k * StageRowWidth, i * -ScoreHeight);
                PlayerScoreRow.gameObject.SetActive(true);

                CanvasScores[initNetId].Add(PlayerScoreRow.GetComponent<TextMeshProUGUI>());
                CanvasScores[initNetId][0].SetText(ListOfPlayerNames[i]); //Sets name for player
            }
        }

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CanvasIsActive = !CanvasIsActive;
        }

        if (CanvasIsActive)
        {
            ScoreboardCanvas.SetActive(true);
        }
        else
        {
            ScoreboardCanvas.SetActive(false);
        }

    }




    private void PrintDict()
    {
        foreach (KeyValuePair<uint, NetworkScoreKeeper.PlayerData> pair in SyncedPlayerScores)
        {
            string Output = string.Empty;
            Output = pair.Key.ToString() + " " + pair.Value.DisplayName + " : { Positions: (";
            //Debug.Log("Key: " + pair.Key);

            foreach (int score in pair.Value.StageFinishPosition)
            {
                Output += score + ",";
                //Debug.Log(score);
            }
            Output += ") | Collectibles: (";

            foreach (int collectiblecount in pair.Value.StageCollectibles)
            {
                Output += collectiblecount + ",";
                //Debug.Log(score);
            }

            Output += ") | Times: (";

            foreach (float time in pair.Value.StageTime)
            {
                Output += time + ",";
                //Debug.Log(score);
            }
            Output += ")}";
            Debug.Log(Output);
        }
    }


}
