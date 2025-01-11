using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<string, Action> eventTable = new Dictionary<string, Action>();

    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] = listener;
        }
        else
        {
            eventTable[eventName] += listener;
        }
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] -= listener;
        }
    }

    public static void Broadcast(string eventName)
    {
        if (eventTable.ContainsKey(eventName))
        {
            eventTable[eventName]?.Invoke();
        }
    }
}
