using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    Transform[] movementTargets;
    Transform platform;

    int targetIndex = 0;
    int direction = 1;

    public float platformSpeed;
    public bool isLoop, isRandomTarget;

    // Start is called before the first frame update
    void Start()
    {
        platform = transform.GetChild(0);

        movementTargets = new Transform[transform.childCount - 1];
        for(int x = 1; x < transform.childCount; x++)
        {
            movementTargets[x - 1] = transform.GetChild(x);
        }

        if (isRandomTarget)
            targetIndex = Random.Range(0, movementTargets.Length);
        else
            targetIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (platform.position != movementTargets[targetIndex].position)
            platform.position = Vector3.MoveTowards(platform.position, movementTargets[targetIndex].position, platformSpeed * Time.deltaTime);
        else
        {
            if (isRandomTarget)
            {
                targetIndex = Random.Range(0, movementTargets.Length);
                return;
            }

            if (targetIndex == movementTargets.Length - 1)
            {
                if (isLoop)
                {
                    targetIndex = 0;
                    return;
                }
                else if (direction == 1)
                    direction = -1;
            }
            else if (targetIndex == 0 && direction == -1)
                direction = 1;

            targetIndex += direction;
        }
    }
}
