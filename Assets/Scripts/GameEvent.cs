using System.Collections.Generic;
using UnityEngine;

// Creates a new asset menu item to easily create instances of this ScriptableObject
[CreateAssetMenu(menuName ="Game Event")]
public class GameEvent : ScriptableObject
{
    // List to hold the listeners that will respond to this event
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // Method to trigger the event, invoking all registered listeners
    public void TriggerEvent()
    {
        // Iterates through the list of listeners in reverse order to avoid issues when 
        // removing elements
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            // Calls the OnEventTriggered method for each listener
            listeners[i].OnEventTriggered();
        }
    }

    // Method to add a listener to the event
    public void AddListener(GameEventListener listener)
    {
        // Adds the provided listener to the list of listeners for this event
        listeners.Add(listener);
    }

    // Method to remove a listener from the event
    public void RemoveListener(GameEventListener listener)
    {
        // Removes the provided listener from the list of listeners for this event
        listeners.Remove(listener);
    }
}
