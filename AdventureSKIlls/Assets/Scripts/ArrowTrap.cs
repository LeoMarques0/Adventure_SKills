using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{

    public Transform[] arrowHole;
    public GameObject arrow;

    public Vector2 checkSize;
    public Vector2 checkOffset;

    public LayerMask playerMask;

    Collider2D[] hit;
    bool trapActive = false;

    // Update is called once per frame
    void Update()
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
            {
                StartCoroutine(ShootArrow());
                trapActive = true;
            }
            else if (trapActive && thief != null && thief.usingInvisibility)
            {
                trapActive = false;
                StopAllCoroutines();
            }
        }
        else
        {
            if (trapActive)
            {
                StopAllCoroutines();
                trapActive = false;
            }
        }
    }

    IEnumerator ShootArrow()
    {
        foreach (Transform tr in arrowHole)
        {
            Projectile newArrow = Instantiate(arrow, tr.position + Vector3.left * 1.25f, Quaternion.Euler(0, 180, 0)).GetComponent<Projectile>();
            newArrow.parent = transform;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShootArrow());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + checkOffset, checkSize);
    }
}
