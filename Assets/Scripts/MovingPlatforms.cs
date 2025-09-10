using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [Header("Platform Settings")]
    public float speed = 2f;           // Movement speed
    public float distance = 3f;        // How far the platform moves before turning around

    private Vector3 startPosition;     // Starting position
    private Vector3 direction;         // Direction based on tag
    private float travelDistance;      // Tracks how far we've moved

    void Start()
    {
        // Store the platform's starting position
        startPosition = transform.position;

        // Decide initial movement direction based on tag
        switch (gameObject.tag)
        {
            case "LR":  // Left to Right
                direction = Vector3.right;
                break;

            case "RL":  // Right to Left
                direction = Vector3.left;
                break;

            case "UD":  // Up and Down
                direction = Vector3.up;
                break;

            default:
                Debug.LogWarning($"{gameObject.name} has no valid tag (LR, RL, UD). Defaulting to no movement.");
                direction = Vector3.zero;
                break;
        }
    }

    void Update()
    {
        // Move the platform
        transform.Translate(direction * speed * Time.deltaTime);

        // Track how far we've traveled from the start position
        travelDistance = Vector3.Distance(startPosition, transform.position);

        // If we've reached the max distance, reverse direction
        if (travelDistance >= distance)
        {
            direction *= -1; // Flip movement direction
            startPosition = transform.position; // Reset reference point
        }
    }
}
