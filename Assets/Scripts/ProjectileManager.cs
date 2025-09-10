using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileManager : MonoBehaviour
{
    public float speed = 3.0f;          // How fast the projectile moves
    public float lifetime = 2.0f;       // Time before the projectile is automatically destroyed
    public float rotationSpeed = 10.0f; // How quickly it turns to follow the player (optional smoothness)

    private Transform player;           // Reference to the player

    void Start()
    {
        // Find the player using the Player tag (safer than using GameObject.Find by name)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Projectile will not home correctly.");
        }

        // Destroy the projectile automatically after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (player == null) return;

        // Calculate the direction to the player each frame
        Vector3 direction = (player.position - transform.position).normalized;

        // Optional: Smoothly rotate toward the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward in the direction we're facing
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the projectile when it hits the player
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("DeathMenu");
        }
    }
}
