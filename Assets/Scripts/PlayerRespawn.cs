using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject StartPosition;
    public Vector3 PlayerRespawnLocation;

    // Start is called before the first frame update

    private void Awake()
    {
        StartPosition = GameObject.FindGameObjectWithTag("Start");
    }
    void Start()
    {
        
        PlayerRespawnLocation = StartPosition.transform.position;
    }

    public void Respawn() {

            gameObject.GetComponentInChildren<Rigidbody>().position = PlayerRespawnLocation;
            gameObject.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Respawning Player: " + this.name);


        
        
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
