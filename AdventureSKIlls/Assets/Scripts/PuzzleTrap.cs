using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrap : MonoBehaviour
{
    public GameObject[] flamethrowers;

    public Vector2 checkSize;
    public Vector2 checkOffset;

    public LayerMask playerMask;

    Collider2D[] hit;
    bool trapActive = false;

    bool activated = true;

    // Update is called once per frame
    void Update()
    {

        if (activated)
        {
            hit = Physics2D.OverlapBoxAll((Vector2)transform.position + checkOffset, checkSize, 0, playerMask);

            if (hit.Length > 0)
            {
                Thief thief = null;
                foreach (Collider2D col in hit)
                {
                    col.TryGetComponent(out thief);
                    if (thief == null)
                        break;
                }

                if (!trapActive && (thief == null || !thief.usingInvisibility))
                    ActivateTrap();
                else if (trapActive && thief != null && thief.usingInvisibility)
                    DeactivateTrap();
            }
            else
            {
                if (trapActive)
                    DeactivateTrap();
            }
        }
    }

    void ActivateTrap()
    {
        trapActive = true;
        foreach(GameObject go in flamethrowers)
        {
            go.SetActive(true);
        }
    }

    void DeactivateTrap()
    {
        trapActive = false;
        foreach (GameObject go in flamethrowers)
        {
            go.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
            activated = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + checkOffset, checkSize);
    }
}
