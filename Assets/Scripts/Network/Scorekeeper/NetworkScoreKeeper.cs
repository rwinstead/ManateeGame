using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkScoreKeeper : NetworkBehaviour
{
    //This class handles all scorekeeping, storing finish position and collectible values sent from the player.

    //The dictionary stores NETID (key) of player and a playerdata object (value).
    //The player data class is just an object with lists for collectibles, stage finish positions, and stage times.

    Dictionary<uint, PlayerData> PlayerScores = new Dictionary<uint, PlayerData>();
    public ServerStageTimer stageTimer;
    public RaceScoreboard raceScoreboard;
    private int MapIndex = 0;
    private int position = 1;
    

    public class PlayerData
    {
        public string DisplayName = string.Empty;
        public List<int> StageCollectibles = new List<int>();
        public List<int> StageFinishPosition = new List<int>();
        public List<double> StageTime = new List<double>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetworkManagerMG.ServerChangedLevel += UpdateScorekeeperNewRound;
        NetworkManagerMG.ServerStopped += UnsubscribeFromEvents;
    }

    public void AddPlayerOnStart(uint NetID, string DisplayName)
    {
        PlayerScores[NetID] = new PlayerData();
        PlayerScores[NetID].DisplayName = DisplayName;
        PrintDict();
    }

    public void FinishedRace(uint NetID, int CollectibleCount)
    {
        if (PlayerScores[NetID].StageFinishPosition.Count < MapIndex) //Make sure that we're loggging the score of the current stage
        {
            PlayerScores[NetID].StageFinishPosition.Add(position);
            position++;
            PlayerScores[NetID].StageTime.Add(stageTimer.displayTime);

            //Get difference between total collectibles of this stage and last total to get collectibles gained for this round
            //but only if it's not the first round
            if (PlayerScores[NetID].StageCollectibles.Count == 0)
            {
                PlayerScores[NetID].StageCollectibles.Add(CollectibleCount);
            }
            else
            {
                int PreviousCollectibleTotal = 0;
                foreach (int collectibleScore in PlayerScores[NetID].StageCollectibles)
                {                    
                    PreviousCollectibleTotal += collectibleScore;
                }

                PlayerScores[NetID].StageCollectibles.Add(CollectibleCount - PreviousCollectibleTotal);
            }

            raceScoreboard.UpdateScoreboard(PlayerScores);
            PrintDict();
        }
        
    }

    public void UpdateScorekeeperNewRound()
    {
        Debug.Log("map index is now " + MapIndex);
        MapIndex++;
        position = 1;
    }

    private void UnsubscribeFromEvents()
    {
        NetworkManagerMG.ServerChangedLevel -= UpdateScorekeeperNewRound;
        NetworkManagerMG.ServerStopped -= UnsubscribeFromEvents;
        Destroy(gameObject);
    }




    private void PrintDict() //Basically makes the dictionaries look like Python dictionary =]
    {
        foreach (KeyValuePair<uint, PlayerData> pair in PlayerScores)
        {
            string Output = string.Empty;
            Output = pair.Key.ToString() +" " + pair.Value.DisplayName + " : { Positions: (";
            //Debug.Log("Key: " + pair.Key);

            foreach(int score in pair.Value.StageFinishPosition)
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
