using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}

public class EventManager : MonoBehaviour {

    private Dictionary<string, IntEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be an active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, IntEvent>();
        }
    }
	
    public static void StartListening (string eventName, UnityAction<int> listener)
    {
        IntEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new IntEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction<int> listener)
    {
        if (eventManager == null) return;
        IntEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent (string eventName, int valueToAdd)
    {
        IntEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(valueToAdd);
        }
    }
}
