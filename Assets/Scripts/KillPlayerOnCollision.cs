using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{

    public Transform startPosition;
    public GameObject Player;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = Player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered");

        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Player.transform.position);
            Debug.Log(startPosition.position);
            cc.enabled = false;
            Player.transform.position = startPosition.position;
            cc.enabled = true;
        }



    }


}