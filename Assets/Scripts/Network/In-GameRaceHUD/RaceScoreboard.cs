using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class RaceScoreboard : NetworkBehaviour
{

    //Headers in board
    [Header("Scoreboard headers")]
    [SerializeField] private Transform StageRowTransformContainer;
    [SerializeField] private Transform StageRowTemplate;
    [SerializeField] private Transform CollectiblesHeader;
    [SerializeField] private Transform TotalScoreHeader;

    //Player score rows
    [Header("Player score rows")]
    [SerializeField] private Transform PlayerScoresContainer;
    [SerializeField] private Transform PlayerScoresTemplate;
    [SerializeField] private Transform PlayerScoresBackground;

    public NetworkScoreKeeper networkScoreKeeper;

    private int StagesCount = 3;
    private float StageRowWidth = 130;
    private float ScoreHeight = 75;
    private float TotalUIWidth = 0;

    [SerializeField]
    [SyncVar]
    public int NumberOfPlayers;

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
    }

    private void Start()
    {
        float containerWidth = StageRowTransformContainer.GetComponent<RectTransform>().anchoredPosition.x;
        float containerHeight = StageRowTransformContainer.GetComponent<RectTransform>().anchoredPosition.y;

        StageRowTemplate.gameObject.SetActive(false);
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
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var PlayerScoreRow = Instantiate(PlayerScoresTemplate, PlayerScoresContainer);
            var ScoreRectTransform = PlayerScoreRow.GetComponent<RectTransform>();
            ScoreRectTransform.anchoredPosition = new Vector2(0, i * -ScoreHeight);
            PlayerScoreRow.gameObject.SetActive(true);
        }
    }

    private void IncreaseUIWidth()
    {
        TotalUIWidth += StageRowWidth;
    }

    public void UpdateScoreboard(Dictionary<uint, NetworkScoreKeeper.PlayerData> PlayerScores)
    {
        foreach (KeyValuePair<uint, NetworkScoreKeeper.PlayerData> pair in PlayerScores)
        {
            string Output = string.Empty;
            Output = pair.Key.ToString() + ": { Positions: (";
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

