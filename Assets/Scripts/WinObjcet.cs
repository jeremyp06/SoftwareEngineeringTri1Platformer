using UnityEngine;

public class WinObject : MonoBehaviour
{
    public GameEvent onGameWon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onGameWon.TriggerEvent();
        }
    }
}
