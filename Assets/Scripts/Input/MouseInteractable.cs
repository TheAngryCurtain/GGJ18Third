using UnityEngine;
using System.Collections;

public abstract class MouseInteractable : MonoBehaviour
{
    public abstract void OnMouseInteraction(int actionId);
}