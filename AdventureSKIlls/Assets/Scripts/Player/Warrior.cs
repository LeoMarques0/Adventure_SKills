using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public GameObject shield;

    public float minimumShieldSize;
    float x;

    [HideInInspector]
    public float shieldHealth = 100, maxShieldHeath = 100, shieldScale;
    bool isShielding;

    private void OnEnable()
    {
        shieldHealth = maxShieldHeath;
        shieldScale = shield.transform.localScale.x;
        x = 1 - minimumShieldSize;
    }

    public override void Update()
    {
        base.Update();
        if(state == BaseState.STANDARD)
            Shield();
    }

    void Shield()
    {
        if (Input.GetButton("Ability") && isGrounded)
        {
            
            isShielding = true;
            anim.SetBool("useAbility", isShielding);

            gameObject.layer = 9;
            shield.SetActive(true);

            if (shieldHealth > 0)
                shieldHealth -= 15 * Time.deltaTime;

            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            isShielding = false;
            anim.SetBool("useAbility", isShielding);
            gameObject.layer = 8;
            shield.SetActive(false);

            if(shieldHealth < 100)
                shieldHealth += 25 * Time.deltaTime;
        }

        shield.transform.localScale = Vector3.one * (((shieldHealth/maxShieldHeath) * x) + minimumShieldSize) * shieldScale;
        Mathf.Clamp(shieldHealth, 0, maxShieldHeath);

        if (shieldHealth == 0)
            state = BaseState.HURT;
    }

}
