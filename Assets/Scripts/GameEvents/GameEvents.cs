using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent {}

public class ShotTakenEvent : GameEvent
{
    public Vector3 Direction;
    public float Spin;
    public float Power;

    public ShotTakenEvent(Vector3 direction, float spin, float power)
    {
        Direction = direction;
        Spin = spin;
        Power = power;
    }
}
