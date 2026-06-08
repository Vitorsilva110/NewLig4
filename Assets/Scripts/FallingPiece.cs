using UnityEngine;

public class FallingPiece : MonoBehaviour
{
    public Vector3 targetPosition;

    public float speed = 8f;

    void Update()
    {
        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
    }
}
