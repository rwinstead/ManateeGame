using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncedServerDataForClients : NetworkBehaviour
{
    public SyncList<NetworkGamePlayer> PlayerList = new SyncList<NetworkGamePlayer>();

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
