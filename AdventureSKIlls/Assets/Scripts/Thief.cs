using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Player
{

    float invisibilityBar = 100;

    bool usingInvisibility;

    public SpriteRenderer[] sprites;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Invisibility();
    }

    void Invisibility()
    {
        if (Input.GetButtonDown("Ability"))
        {
            if (!usingInvisibility)
                StartCoroutine(TurnInvisible());
            else
            {
                StopAllCoroutines();

                foreach (SpriteRenderer sr in sprites)
                {
                    Color srColor = sr.color;

                    srColor.a = 1;
                    sr.color = srColor;
                }

                usingInvisibility = false;
            }
        }

        if (!usingInvisibility)
            invisibilityBar += Time.deltaTime;
    }

    IEnumerator TurnInvisible()
    {

       

        while (invisibilityBar > 0)
        {
            usingInvisibility = true;

            foreach (SpriteRenderer sr in sprites)
            {
                Color srColor = sr.color;

                srColor = srColor - new Color(0, 0, 0, Time.deltaTime);
                srColor.a = Mathf.Clamp(srColor.a, Mathf.Abs(rb.velocity.magnitude) < 1 ? 0 : .5f, 100);

                sr.color = srColor;
            }

            invisibilityBar -= 3 * Time.deltaTime;
            yield return null;
        }

        usingInvisibility = false;
    }
}
