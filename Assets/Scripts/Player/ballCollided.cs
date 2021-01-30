using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballCollided : MonoBehaviour
{
    [SerializeField] playerCollision Pc;

    public void OnTriggerEnter(Collider other)
    {
        Pc.HandleCollision(other);
    }

    public void OnTriggerStay(Collider other)
    {
        Physics.IgnoreLayerCollision(10, 10, false);
    }

    public void OnTriggerExit(Collider other)
    {
        Physics.IgnoreLayerCollision(10, 10);
    }


}
