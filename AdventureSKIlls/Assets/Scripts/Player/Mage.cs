using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    public float abilityDelay;
    public int healAmount;

    public PlayerProjectile magic;
    public LayerMask ableToHeal;
    public Vector2 healOffset, healArea;

    bool canHeal = true;

    public override void Update()
    {
        base.Update();
        CallHeal();
    }

    void CallHeal()
    {
        if (Input.GetButtonDown("Ability") && canHeal)
        {            
            anim.SetBool("useAbility", true);            
        }
    }

    public void Heal()
    {
        Collider2D[] characters = Physics2D.OverlapBoxAll((Vector2)transform.position + healOffset, healArea, 0, ableToHeal);
        for(int x = 0; x < characters.Length; x++)
        {
            characters[x].GetComponent<BaseStats>().health += healAmount;
        }
        StartCoroutine(HealDelay());
    }

    IEnumerator HealDelay()
    {
        canHeal = false;
        anim.SetBool("useAbility", canHeal);
        yield return new WaitForSeconds(abilityDelay);
        canHeal = true;
    }

    public void CallShot()
    {
        magic.Shoot();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + healOffset, healArea);
    }
}
