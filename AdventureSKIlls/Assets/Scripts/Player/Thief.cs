using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Player
{

    [HideInInspector]
    public float invisibilityBar = 100, maxInvisibilityBar = 100;

    public bool usingInvisibility;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Invisibility();
    }

    void Invisibility()
    {
        if (state == BaseState.STANDARD && Input.GetButtonDown("Ability"))
        {
            if (!usingInvisibility)
                StartCoroutine(TurnInvisible());
            else
            {
                StopAllCoroutines();

                DisableTransparency();

                usingInvisibility = false;
            }
        }

        if (!usingInvisibility)
        {
            invisibilityBar += Time.deltaTime;
            invisibilityBar = Mathf.Clamp(invisibilityBar, 0, maxInvisibilityBar);
        }
    }

    IEnumerator TurnInvisible()
    {     

        while (invisibilityBar > 0)
        {
            usingInvisibility = true;

            ChangeTransparency();

            invisibilityBar -= 3 * Time.deltaTime;
            yield return null;
        }

        usingInvisibility = false;
    }

    public void ChangeTransparency()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            Color srColor = sr.color;

            srColor = srColor - new Color(0, 0, 0, Time.deltaTime);
            srColor.a = Mathf.Clamp(srColor.a, Mathf.Abs(rb.velocity.magnitude) < 1 && state == BaseState.STANDARD ? 0 : .5f, 100);

            sr.color = srColor;
        }
    }

    public void DisableTransparency()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            Color srColor = sr.color;

            srColor.a = 1;
            sr.color = srColor;
        }
    }
}
