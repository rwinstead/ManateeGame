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
    public static int MapIndex = 0;
    public static int position = 1;
    

    class PlayerData
    {
        public List<int> StageCollectibles = new List<int>();
        public List<int> StageFinishPosition = new List<int>();
        public List<float> StageTime = new List<float>();

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayerOnStart(uint NetID, string DisplayName)
    {
        PlayerScores[NetID] = new PlayerData();
        PrintDict();

    }

    public void FinishedRace(uint NetID, int CollectibleCount, float Time)
    {
        Debug.Log("received coins: " + CollectibleCount);
        if (PlayerScores[NetID].StageFinishPosition.Count < MapIndex) //Make sure that we're loggging the score of the current stage
        {
            PlayerScores[NetID].StageFinishPosition.Add(position);
            position++;
            PlayerScores[NetID].StageTime.Add(Time);

            //Get difference between total collectibles of this stage and last to get collectibles gained for this round
            //but only if it's not the first round
            if (PlayerScores[NetID].StageCollectibles.Count == 0)
            {
                PlayerScores[NetID].StageCollectibles.Add(CollectibleCount);
            }
            else
            {
                PlayerScores[NetID].StageCollectibles.Add(CollectibleCount - PlayerScores[NetID].StageCollectibles[MapIndex - 2]);
            }

            PrintDict();
        }
        
    }

    public static void UpdateScorekeeperNewRound()
    {
        MapIndex++;
        position = 1;
    }


    
    private void PrintDict() //Basically makes the dictionaries look like Python dictionary =]
    {
        foreach (KeyValuePair<uint, PlayerData> pair in PlayerScores)
        {
            string Output = string.Empty;
            Output = pair.Key.ToString() + ": { Positions: (";
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
