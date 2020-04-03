using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public Transform healthBar;
    Vector3 healthBarScale;

    Player main;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarScale = healthBar.localScale;
        healthBarScale.x = main.health / 100;
        healthBar.localScale = healthBarScale;
    }
}
