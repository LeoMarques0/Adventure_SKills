using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    Animator anim;
    Vector2 openGatePos;

    public Transform gate;
    public Vector2 whereToOpenGate;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        openGatePos = (Vector2)gate.position + whereToOpenGate;
    }

    IEnumerator GateOpen()
    {
        while ((Vector2)gate.position != openGatePos)
        {
            gate.position = Vector3.Lerp(gate.position, openGatePos, 1f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet" || collision.gameObject.layer == 10)
        {
            anim.SetBool("Activated", true);
            StartCoroutine(GateOpen());
        }
    }
}
