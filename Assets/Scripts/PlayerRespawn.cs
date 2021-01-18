using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject StartPosition;
    public Vector3 PlayerRespawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRespawnLocation = StartPosition.transform.position;
    }

    public bool Respawn()
    {
        try
        {
            this.transform.position = PlayerRespawnLocation;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Respawning Player: " + this.name);
            return true;
        }
        catch
        {
            return false;
        }
        
        
    }

    public bool updateRespawn(Vector3 newRespawn)
    {
        try
        {
            PlayerRespawnLocation = newRespawn;
            Debug.Log("New Respawn Location for " + this.name + " - " + newRespawn);
            return true;
        }
        catch
        {
            return false;
        }


    }
}
