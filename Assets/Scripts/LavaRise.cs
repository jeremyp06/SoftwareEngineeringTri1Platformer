using UnityEngine;

public class LavaRise : MonoBehaviour
{
    public float riseSpeed = 0.1f;

    void Update()
    {
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }

}