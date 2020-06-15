using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField]
    private Boss boss = null;
    [SerializeField]
    private GameObject bossHeathUI = null;

    Vector2 openGatePos;
    Vector2 exitGatePos;

    public CameraController camController;
    public Transform camPos;

    public StageManager stageManager;
    public Transform[] playersPos;
    public Transform gate;
    public Transform exit;
    public Vector2 whereToCloseGate;
    public Vector2 whereToOpenExit;

    bool exitOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        openGatePos = (Vector2)gate.position + whereToCloseGate;
        exitGatePos = (Vector2)exit.position + whereToOpenExit;
    }

    private void Update()
    {
        if(boss.health <= 0 && !exitOpen)
        {
            StartCoroutine(GateOpen());
            exitOpen = true;
        }
    }

    IEnumerator GateClose()
    {
        while ((Vector2)gate.position != openGatePos)
        {
            gate.position = Vector3.Lerp(gate.position, openGatePos, 1f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator GateOpen()
    {
        while ((Vector2)exit.position != exitGatePos)
        {
            exit.position = Vector3.Lerp(exit.position, exitGatePos, 1f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            boss.activated = true;
            StartCoroutine(GateClose());

            stageManager.myPlayer.transform.position = playersPos[GameManager.singleton.playerCharacterIndex].position;
            bossHeathUI.SetActive(true);

            camController.target = camPos;
            camController.distanceFromTarget = -45;
        }
    }
}
