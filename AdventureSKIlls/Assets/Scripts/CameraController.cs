using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float distanceFromTarget;
    public float lerpT;
    public Vector3 followOffset;

    Vector3 followPos;

    public Collider2D confiner;
    Vector2 maxPos, minPos;
    float camHeight, camWidth;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        Vector3 camBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(distanceFromTarget)));
        Vector3 camTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(distanceFromTarget)));

        camHeight = camTopRight.y - camBottomLeft.y;
        camWidth = camTopRight.x - camBottomLeft.x;

        if (confiner != null)
        {
            maxPos = confiner.bounds.max - new Vector3(camWidth / 2, camHeight / 2);
            minPos = confiner.bounds.min + new Vector3(camWidth / 2, camHeight / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        followPos = target.position + followOffset + (Vector3.forward * distanceFromTarget);

        if (confiner != null)
        {
            followPos.x = Mathf.Clamp(followPos.x, minPos.x, maxPos.x);
            followPos.y = Mathf.Clamp(followPos.y, minPos.y, maxPos.y);
        }

        transform.position = Vector3.Lerp(transform.position, followPos, lerpT);
    }
}
