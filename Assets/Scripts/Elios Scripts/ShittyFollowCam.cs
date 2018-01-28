using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyFollowCam : MonoBehaviour
{
    public Vector3 cameraOffset;
    public PlayerController player;

    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float bottomBoundary;

    public void StartCamera()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update ()
    {
        if (player != null)
        {
            Vector3 position = player.transform.position + cameraOffset;
            position.x = Mathf.Clamp(position.x, leftBoundary, rightBoundary);
            position.y = Mathf.Clamp(position.y, bottomBoundary, topBoundary);

            transform.position = position;
            
        }
    }
}
