using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign your projectile prefab in the Inspector
    public Transform firePoint;          // Optional: where the projectile spawns
    public float shootInterval = 2.0f;   // Time between shots

    private float shootTimer;

    void Update()
    {
        // Countdown until next shot
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }
    }

    void ShootProjectile()
    {
        // Decide where to spawn from — either firePoint or tower center
        Vector3 spawnPosition = firePoint ? firePoint.position : transform.position;
        Quaternion spawnRotation = firePoint ? firePoint.rotation : transform.rotation;

        // Spawn the projectile
        Instantiate(projectilePrefab, spawnPosition, spawnRotation);

        // Play firing sound through AudioManager
        if (AudioManager.instance && AudioManager.instance.projectileSound)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.projectileSound);
        }
    }
}
