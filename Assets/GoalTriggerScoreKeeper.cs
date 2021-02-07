using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoalTriggerScoreKeeper : NetworkBehaviour
{


    void OnTriggerExit(Collider other)
    {
        //if(!isServer) { return; }
        if (other.gameObject.CompareTag("Player") && other.gameObject.name == "PlayerCollider")
        {
            Debug.Log(other.gameObject.name);
            //Debug.Log(other.gameObject.GetComponentInParent<NetworkIdentity>().netId.Value);
            other.gameObject.GetComponentInParent<LinkToGamePlayer>().FinishedRace();
        }
    }

}
