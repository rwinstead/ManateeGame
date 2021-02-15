using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerSpawnSystem : NetworkBehaviour
{

    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer()
    {
        Debug.Log("Spawning a player!!!!!!!!!");
        NetworkManagerMG.OnServerReadied += SpawnPlayer;
    }

    //[Server]

    private void OnDestroy()
    {
        Debug.Log("Removing player spawnage");
        NetworkManagerMG.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {

        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);

        foreach (var item in conn.clientOwnedObjects)
        {
            if (item.CompareTag("GamePlayer"))
            {
                //playerInstance.GetComponent<LinkToGamePlayer>().SetDisplayName(item.GetComponent<NetworkGamePlayer>().displayName);
                //playerInstance.GetComponent<LinkToGamePlayer>().SetClientGamePlayer(item.GetComponent<NetworkGamePlayer>());
            }
        }

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;

    }



}
