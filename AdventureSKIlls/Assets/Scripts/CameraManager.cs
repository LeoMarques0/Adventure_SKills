using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        PlayerNetworking[] players = FindObjectsOfType<PlayerNetworking>();
        CinemachineVirtualCamera cam = GetComponent<CinemachineVirtualCamera>();

        foreach(PlayerNetworking player in players)
        {
            if(player.photonView.IsMine)
            {
                cam.m_Follow = player.transform;
                cam.LookAt = player.transform;
            }
        }
        */

        PlayerNetworking[] players = FindObjectsOfType<PlayerNetworking>();
        CameraController cam = GetComponent<CameraController>();

        foreach (PlayerNetworking player in players)
        {
            if (player.photonView.IsMine)
            {
                cam.target = player.transform;
            }
        }

    }
}
