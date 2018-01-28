using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEnteredEvent : GameEvent
{
    public bool Entered;

    public BuildingEnteredEvent(bool entered)
    {
        Entered = entered;
    }
}

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject mExteriorObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mExteriorObj.SetActive(false);
            EventManager.Instance.FireEvent(new BuildingEnteredEvent(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mExteriorObj.SetActive(true);
            EventManager.Instance.FireEvent(new BuildingEnteredEvent(true));
        }
    }
}
