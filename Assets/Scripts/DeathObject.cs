using UnityEngine;

public class DeathObject : MonoBehaviour
{
    public GameEvent onGameLost;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("death");
            onGameLost.TriggerEvent();
        }
    }
}
