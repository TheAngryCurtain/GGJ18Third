using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : Detection
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //EventManager.Instance.FireEvent(new DetectionEvent(false));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        //EventManager.Instance.FireEvent(new DetectionEvent(true));
    }
}
