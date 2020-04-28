﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : PlayerAttack
{
    public GameObject projectile;
    public int maxProjectiles;

    public BaseStats player;

    List<Projectile> newShots = new List<Projectile>();

    public override void AttackInput()
    {
        if (Input.GetButtonDown("Attack") && newShots.Count < maxProjectiles)
        {
            StopAllCoroutines();
            StartCoroutine(AttackDelay());
        }

        for(int x = 0; x < newShots.Count; x++)
        {
            if (newShots[x] == null)
            {
                newShots.Remove(newShots[x]);
                x--;
            }
        }
    }

    public void Shoot()
    {
        newShots.Add(Instantiate(projectile, transform.position, transform.parent.rotation).GetComponent<Projectile>());
        newShots[newShots.Count - 1].parent = player.transform;
        ParticleSystem.MainModule mainModule = newShots[newShots.Count - 1].transform.GetChild(0).GetComponent<ParticleSystem>().main;
        mainModule.startRotationY = player.transform.localEulerAngles.y * Mathf.Deg2Rad;
        newShots[newShots.Count - 1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }
}