using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject solidObstaclePrefab;
    [SerializeField] private GameObject phaseableObstaclePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Positions")]
    [SerializeField] private Vector2 solidSpawnPosition = new Vector2(9.75f, -2.7f);
    [SerializeField] private Vector2 phaseableSpawnPosition = new Vector2(9.75f, -1.79f);

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnObstacle();
            timer = spawnInterval;
        }
    }

    private void SpawnObstacle()
    {
        int rand = Random.Range(0, 2); 

        if (rand == 0)
        {
            Instantiate(solidObstaclePrefab, solidSpawnPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(phaseableObstaclePrefab, phaseableSpawnPosition, Quaternion.identity);
        }
    }
}
