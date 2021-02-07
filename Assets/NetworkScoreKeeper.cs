using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkScoreKeeper : NetworkBehaviour
{
    //This class handles all scorekeeping, storing finish position and collectible values sent from the player.

    SyncDictionary<uint, List<int>> PlayerScores = new SyncDictionary<uint, List<int>>(); //Stores NETID of player and a list of scores for each round
    private int MapIndex = 1;
    int position = 1;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateScore(string map, int netID, int finishPosition)
    {

    }

    public void AddPlayerOnStart(uint NetID, string DisplayName)
    {
        PlayerScores[NetID] = new List<int>();
        PrintDict();
    }

    public void FinishedRace(uint NetID)
    {
        if (PlayerScores[NetID].Count < MapIndex)
        {
            PlayerScores[NetID].Add(position);
            position++;
            PrintDict();
        }

    }



    private void PrintDict() //Basically makes the dictionaries look like Python dictionary =]
    {
        foreach (KeyValuePair<uint, List<int>> pair in PlayerScores)
        {
            string Output = string.Empty;
            Output = pair.Key.ToString() + ": {";
            //Debug.Log("Key: " + pair.Key);

            foreach(int score in pair.Value)
            {
                Output += score + ",";
                //Debug.Log(score);
            }
            Output += "}";
            Debug.Log(Output);
        }
    }


}
