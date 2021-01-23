using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnersSlow_clockwise : MonoBehaviour
{

   private float speed=3f;
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        this.transform.Rotate(0f, speed, 0f);
    }
}
