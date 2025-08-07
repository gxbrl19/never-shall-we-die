using UnityEngine;

public class HiveSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject oculothoraxPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxSpawnCount = 10;

    [Header("Hive Health")]
    [SerializeField] private int maxHealth = 20;
    private int currentHealth;
    private float timer;
    private int currentSpawned = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentSpawned >= maxSpawnCount) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnOculothorax();
            timer = 0;
        }
    }

    private void SpawnOculothorax()
    {
        GameObject enemy = Instantiate(oculothoraxPrefab, spawnPoint.position, Quaternion.identity);
        currentSpawned++;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Destroy(gameObject); // Hive destruída, para de spawnar
    }
}
