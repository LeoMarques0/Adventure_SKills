using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flickering : MonoBehaviour
{

    Light2D myLight;
    float targetRadius;

    public float minRadius = 7, maxRadius = 9;
    public float flickeringEffect = 5;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light2D>();
        targetRadius = myLight.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if(myLight.pointLightOuterRadius == targetRadius)
            targetRadius = Random.Range(minRadius, maxRadius);

        myLight.pointLightOuterRadius = Mathf.MoveTowards(myLight.pointLightOuterRadius, targetRadius, flickeringEffect);
    }
}
