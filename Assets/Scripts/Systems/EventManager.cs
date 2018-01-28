using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    //////////////////////////////////////////////////////////////////////
    /// Delegate
    //////////////////////////////////////////////////////////////////////

    public delegate void EventDelegate<T>(T gameEvent) where T : GameEvent;
    private delegate void EventDelegate(GameEvent gameEvent);

    //////////////////////////////////////////////////////////////////////
    /// Member Variables
    //////////////////////////////////////////////////////////////////////
    private Dictionary<System.Type, EventDelegate> mEventDelegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> mEventDelegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    //////////////////////////////////////////////////////////////////////
    /// Singleton<MonoBehavior> Implementation
    //////////////////////////////////////////////////////////////////////

    public override void Awake()
    {
        mEventDelegates = new Dictionary<System.Type, EventDelegate>();
        mEventDelegateLookup = new Dictionary<System.Delegate, EventDelegate>();
        base.Awake();
    }

    public override void OnDestroy()
    {
        Clear();
        base.OnDestroy();
    }

    //////////////////////////////////////////////////////////////////////
    /// EventManager Implementation
    //////////////////////////////////////////////////////////////////////

    public void AddEventListener<T>(EventDelegate<T> eventDelegate) where T : GameEvent
    {
        // If we've already added this listener, assert.
        if (mEventDelegateLookup.ContainsKey(eventDelegate))
        {
            Debug.AssertFormat(false, "EventManager.AddEventListener<T> : Attempting to add an event listener with name: " + eventDelegate.Method.Name + " but it is already added.");
            return;
        }

        // Create a lookup delegate for this handler
        EventDelegate lookupDelegate = (GameEvent) => { eventDelegate((T)GameEvent); };
        mEventDelegateLookup[eventDelegate] = lookupDelegate;

        // Check to see if we already have an entry for this type. if we do, add our new handler to the list.
        EventDelegate existingDelegate;
        if (mEventDelegates.TryGetValue(typeof(T), out existingDelegate))
        {
            mEventDelegates[typeof(T)] = existingDelegate += lookupDelegate;
        }
        else
        {
            mEventDelegates[typeof(T)] = lookupDelegate;
        }
    }

    public void RemoveEventListener<T>(EventDelegate<T> eventDelegate) where T : GameEvent
    {
        EventDelegate lookupDelegate;
        if (mEventDelegateLookup.TryGetValue(eventDelegate, out lookupDelegate))
        {
            EventDelegate existingDelegate;
            if (mEventDelegates.TryGetValue(typeof(T), out existingDelegate))
            {
                existingDelegate -= lookupDelegate;
                if (existingDelegate == null)
                {
                    mEventDelegates.Remove(typeof(T));
                }
                else
                {
                    mEventDelegates[typeof(T)] = existingDelegate;
                }
            }
        }
        else
        {
            // This delegate is not in the list.
        }
    }

    public void FireEvent(GameEvent gameEvent)
    {
        EventDelegate eventDelegate;
        if (mEventDelegates.TryGetValue(gameEvent.GetType(), out eventDelegate))
        {
            eventDelegate.Invoke(gameEvent);
        }
        else
        {
            Debug.LogWarning("Event: " + gameEvent.GetType() + " has no listeners");
        }
    }

    public void Clear()
    {
        mEventDelegates.Clear();
        mEventDelegateLookup.Clear();
    }
}