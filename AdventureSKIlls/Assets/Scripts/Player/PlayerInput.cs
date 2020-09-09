using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static PlayerInput singleton;

    [HideInInspector]
    public bool jumpButton;
    [HideInInspector]
    public bool attackButton;
    [HideInInspector]
    public bool abilityButton;

    public float bufferDelay = .1f;

    // Start is called before the first frame update
    void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(InputBuffer(result => jumpButton = result));
            StopCoroutine(InputBuffer(result => jumpButton = result));
        }
        if (Input.GetButtonDown("Ability"))
        {
            StartCoroutine(InputBuffer(result => abilityButton = result));
            StopCoroutine(InputBuffer(result => abilityButton = result));
        }
        if (Input.GetButtonDown("Attack"))
        {
            StartCoroutine(InputBuffer(result => attackButton = result));
            StopCoroutine(InputBuffer(result => attackButton = result));
        }
    }

    IEnumerator InputBuffer(Action<bool> input)
    {
        input(true);
        yield return new WaitForSeconds(bufferDelay);
        input(false);
    }
}
