using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnersFast : MonoBehaviour
{

    private float speed = 4.1f;
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
