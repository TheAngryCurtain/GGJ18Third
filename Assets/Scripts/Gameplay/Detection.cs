using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionEvent : GameEvent
{
    public bool Detected;

    public DetectionEvent(bool detected)
    {
        Detected = detected;
    }
}

public class Detection : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO
        // probably should get the players current detection level before entering, so it can be set back to that when they leave
        EventManager.Instance.FireEvent(new DetectionEvent(false));
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.Instance.FireEvent(new DetectionEvent(true));
    }
}
