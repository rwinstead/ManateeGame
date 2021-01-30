using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballCollided : MonoBehaviour
{
    [SerializeField] playerCollision pc;
    public void OnCollisionEnter(Collision collision)
    {

        pc.handleCollision(collision);

    }


}
