using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{

    [SerializeField]
    private Slider healthSlider = null;

    [SerializeField]
    private BaseStats main = null;

    void Update()
    {
        if (main == null)
        {
            Destroy(gameObject);
            return;
        }
        healthSlider.value = main.health;
    }
}
