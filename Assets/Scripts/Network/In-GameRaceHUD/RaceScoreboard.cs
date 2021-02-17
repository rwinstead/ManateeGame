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

    [SerializeField]
    [SyncVar]
    public int NumberOfPlayers;

    Dictionary<uint, NetworkScoreKeeper.PlayerData> SyncedPlayerScores = new Dictionary<uint, NetworkScoreKeeper.PlayerData>();

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
     * Getting the # of players must be called in OnEnable. If called in start, the syncvar won't be updated in time
     * for the client, and it will call start with number of players = 0.
     */
    private void OnEnable()
    {
        NumberOfPlayers = Room.GamePlayers.Count;
        TotalNumberOfHeaders = StagesCount + 3;

}

    private void Start()
    {
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

        PlayerScoresBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(TotalUIWidth + 130, ScoreHeight);

        Debug.Log(NumberOfPlayers);

        /*
        for (int i = 0; i < NumberOfPlayers; i++) // Creates the background for each player score row
        {
            var PlayerBkgScoreRow = Instantiate(PlayerScoresBackgroundTemplate, PlayerScoresBackgroundContainer);
            var ScoreBkgRectTransform = PlayerBkgScoreRow.GetComponent<RectTransform>();
            ScoreBkgRectTransform.anchoredPosition = new Vector2(0, i * -ScoreHeight);
            PlayerBkgScoreRow.gameObject.SetActive(true);
        }

        for (int i = 0; i < NumberOfPlayers; i++) //Creates each cell in the scoreboard table that contains player data
        {
            for(int k = 0; k < TotalNumberOfHeaders; k++)
            {
                var PlayerScoreRow = Instantiate(PlayerScoresTemplate, PlayerScoresContainer);
                var ScoreRectTransform = PlayerScoreRow.GetComponent<RectTransform>();
                ScoreRectTransform.anchoredPosition = new Vector2(k * StageRowWidth, i * -ScoreHeight);
                PlayerScoreRow.gameObject.SetActive(true);
            }
        }
        */

    }

    private void IncreaseUIWidth()
    {
        TotalUIWidth += StageRowWidth;
    }

    private void UpdateScoreBoardCanvas()
    {

    }

    [Server]
    public void UpdateScoreboard(Dictionary<uint, NetworkScoreKeeper.PlayerData> PlayerScores, uint FinishedNetId)
    {
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
            Debug.Log("wrong path");
            int NewScoreIndex = PlayerScores[FinishedNetId].StageCollectibles.Count - 1;

            //This awful-looking function sends the client the playerscore values for the player that just finished, so they can update their scoreboard.
            RpcSendScoreData(FinishedNetId, PlayerScores[FinishedNetId].DisplayName, PlayerScores[FinishedNetId].StageCollectibles[NewScoreIndex],
                PlayerScores[FinishedNetId].StageFinishPosition[NewScoreIndex], PlayerScores[FinishedNetId].StageTime[NewScoreIndex]);
        }

        else
        {
            if (!isServer)
            {
                RpcInitializeScoreboard(FinishedNetId, PlayerScores[FinishedNetId].DisplayName);
            }
            else
            {
                InitializeScoreboard(FinishedNetId, PlayerScores[FinishedNetId].DisplayName);
            }
        }

        PrintDict();
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

    [ClientRpc] // This method doesnt work currently -- it's never called on client since it' called before client even joins.
    //Need to listen for event to create scoreboard when everyone has joined or do coroutine.

    private void RpcInitializeScoreboard(uint netId, string DisplayName)
    {
        Debug.Log("init scoreboard");
        SyncedPlayerScores[netId] = new NetworkScoreKeeper.PlayerData();
        SyncedPlayerScores[netId].DisplayName = DisplayName;

        int NumberOfPlayersJoined = 0;

            var PlayerBkgScoreRow = Instantiate(PlayerScoresBackgroundTemplate, PlayerScoresBackgroundContainer);
            var ScoreBkgRectTransform = PlayerBkgScoreRow.GetComponent<RectTransform>();
            ScoreBkgRectTransform.anchoredPosition = new Vector2(0, NumberOfPlayersJoined * -ScoreHeight);
            PlayerBkgScoreRow.gameObject.SetActive(true);

            for (int k = 0; k < TotalNumberOfHeaders; k++)
            {
                var PlayerScoreRow = Instantiate(PlayerScoresTemplate, PlayerScoresContainer);
                var ScoreRectTransform = PlayerScoreRow.GetComponent<RectTransform>();
                ScoreRectTransform.anchoredPosition = new Vector2(k * StageRowWidth, NumberOfPlayersJoined * -ScoreHeight);
                PlayerScoreRow.gameObject.SetActive(true);
            }

        NumberOfPlayersJoined++;

        PrintDict();
    }

    private int NumberOfPlayersJoinedServer = 0;

    [Server]
    private void InitializeScoreboard(uint netId, string DisplayName)
    {
        Debug.Log("init scoreboard server");

        PlayerScoresBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(130 * (TotalNumberOfHeaders), ScoreHeight);
        Debug.Log(TotalUIWidth);

        var PlayerBkgScoreRow = Instantiate(PlayerScoresBackgroundTemplate, PlayerScoresBackgroundContainer);
        var ScoreBkgRectTransform = PlayerBkgScoreRow.GetComponent<RectTransform>();
        ScoreBkgRectTransform.anchoredPosition = new Vector2(0, NumberOfPlayersJoinedServer * -ScoreHeight);
        PlayerBkgScoreRow.gameObject.SetActive(true);

        for (int k = 0; k < TotalNumberOfHeaders; k++)
        {
            var PlayerScoreRow = Instantiate(PlayerScoresTemplate, PlayerScoresContainer);
            var ScoreRectTransform = PlayerScoreRow.GetComponent<RectTransform>();
            ScoreRectTransform.anchoredPosition = new Vector2(k * StageRowWidth, NumberOfPlayersJoinedServer * -ScoreHeight);
            PlayerScoreRow.gameObject.SetActive(true);
        }

        NumberOfPlayersJoinedServer++;

    }


}
