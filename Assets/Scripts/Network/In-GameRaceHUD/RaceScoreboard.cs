using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class RaceScoreboard :NetworkBehaviour
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

    private int StagesCount = 3;
    private float StageRowWidth = 130;
    private float ScoreHeight = 75;
    private float TotalUIWidth = 0;

    [SerializeField]
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

    private void Awake()
    {
        NumberOfPlayers = Room.GamePlayers.Count;

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


}
