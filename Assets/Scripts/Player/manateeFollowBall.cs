using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manateeFollowBall : MonoBehaviour
{

    public GameObject targ;
    //public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targ.transform.position, 10f);
        transform.position = targ.transform.position;
    }
}
