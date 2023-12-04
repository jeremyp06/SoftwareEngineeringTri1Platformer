using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    public Transform player; 

    void Update()
    {
        if (player != null)
        {
            Vector3 newPosition = transform.position;

            newPosition.x = player.position.x;
            newPosition.y = player.position.y;

            transform.position = newPosition;
        }
    }
}