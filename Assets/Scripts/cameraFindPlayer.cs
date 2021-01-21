using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class cameraFindPlayer : NetworkBehaviour
{
    // Start is called before the first frame update

    public Transform player;
    public CinemachineFreeLook vcam;

    private void Awake()
    {
        vcam = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineFreeLook>();
    }

    void Start()
    {
        if (isLocalPlayer) {
            //Debug.Log(vcam);

            //vcam.m_Follow = player;
            vcam.Follow = player;

            //vcam.m_LookAt = player;
            vcam.LookAt = player;
        }
    }

    // Update is called once per frame

}
