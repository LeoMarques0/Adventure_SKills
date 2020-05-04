using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NPC_One : BaseStats
{

    Animator anim;
    Vector2 openGatePos;

    public Transform gate;
    public Vector2 whereToOpenGate;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (health != 0)
            health = 0;

        openGatePos = (Vector2)gate.position + whereToOpenGate;
        health = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Action"))
        {
            photonView.RPC("StartAnimation", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void StartAnimation()
    {
        anim.Play("Action");
    }
    public void OpenGate()
    {
        StartCoroutine(GateOpen());
    }

    IEnumerator GateOpen()
    {
        while((Vector2)gate.position != openGatePos)
        {
            gate.position = Vector3.Lerp(gate.position, openGatePos, 1f * Time.deltaTime);
            yield return null;
        }
    }
}
