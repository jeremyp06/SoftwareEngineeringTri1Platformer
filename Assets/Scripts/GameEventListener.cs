using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // Reference to the GameEvent this listener is associated with
    public GameEvent gameEvent;

    // UnityEvent to be invoked when the associated GameEvent is triggered
    public UnityEvent onEventTriggered;

    // Called when this component is enabled
    void OnEnable()
    {
        // Adds this listener to the GameEvent's list of listeners
        gameEvent.AddListener(this);
    }

    // Called when this component is disabled
    void OnDisable()
    {
        // Removes this listener from the GameEvent's list of listeners
        gameEvent.RemoveListener(this);
    }

    // Method called when the associated GameEvent is triggered
    public void OnEventTriggered()
    {
        // Invokes the UnityEvent to trigger responses to the GameEvent being fired
        onEventTriggered.Invoke();
    }
}
