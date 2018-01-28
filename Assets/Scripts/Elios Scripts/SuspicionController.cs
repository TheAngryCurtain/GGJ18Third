using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YEO_CLOTHES_SUS_EVENT: GameEvent
{
    public int iSuspicion; 
}

public class YEO_SUS_CHANGED_EVENT : GameEvent
{
    public int Suspicion;
}


public class SuspicionController : MonoBehaviour
{
    private const int MAX_SUSPICION = 100;
    private const int MIN_SUSPICION = 0;

    int iSuspicionLevel = 0;

	// Use this for initialization
	void Start ()
    {
        
	}

    public void Init()
    {
        EventManager.Instance.AddEventListener<YEO_CLOTHES_SUS_EVENT>(AddSuspicion);
        YEO_SUS_CHANGED_EVENT susChangedEvent = new YEO_SUS_CHANGED_EVENT();
        susChangedEvent.Suspicion = iSuspicionLevel;
        EventManager.Instance.FireEvent(susChangedEvent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSuspicion(YEO_CLOTHES_SUS_EVENT eventData)
    {
        AddSuspicion(eventData.iSuspicion);
    }

    public void AddSuspicion(int iSus)
    {
        if(iSuspicionLevel + iSus >= MIN_SUSPICION && iSuspicionLevel + iSus <= MAX_SUSPICION)
        {
            iSuspicionLevel += iSus;
        }

        YEO_SUS_CHANGED_EVENT susChangedEvent = new YEO_SUS_CHANGED_EVENT();
        susChangedEvent.Suspicion = iSuspicionLevel;
        EventManager.Instance.FireEvent(susChangedEvent);

        //Debug.Log("SuspicionController: AddSuspicion: iSus: " + iSus + ", iSuspicionLevel: " + iSuspicionLevel);
    }

    public int GetSuspicion()
    {
        return iSuspicionLevel;
    }
}
