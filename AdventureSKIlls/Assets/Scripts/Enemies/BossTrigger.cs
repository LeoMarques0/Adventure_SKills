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

    public StageManager stageManager;
    public Transform[] playersPos;
    public Transform gate;
    public Vector2 whereToOpenGate;

    // Start is called before the first frame update
    void Start()
    {
        openGatePos = (Vector2)gate.position + whereToOpenGate;
    }

    IEnumerator GateClose()
    {
        while ((Vector2)gate.position != openGatePos)
        {
            gate.position = Vector3.Lerp(gate.position, openGatePos, 1f * Time.deltaTime);
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
        }
    }
}
