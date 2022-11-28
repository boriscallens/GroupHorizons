using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class AsteroidSpawnBehaviour : MonoBehaviour
{
    public AsteroidBehaviour asteroidPrefab;
    public PlayerBehaviour player;
    public float spawnRate = 1.0f;
    public float spawnMinimumDistanceToPlayer = 1.0f;

    private int _boundaryLayer;
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = player.transform;
        _boundaryLayer = LayerMask.GetMask("Boundary");
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        var transformPosition = (Vector2)_playerTransform.position;
        for (var attempts = 0; attempts < 10; attempts++)
        {
            var towards = Random.insideUnitCircle.normalized;
            var hits = Physics2D.Raycast(transformPosition, towards, float.PositiveInfinity, _boundaryLayer);

            if (hits.collider == null) continue;
            if (hits.distance < spawnMinimumDistanceToPlayer) continue;

            var position = hits.point;
            var rotation = Quaternion.LookRotation(transformPosition - position, Vector3.up);
            var asteroid = Instantiate(asteroidPrefab, position, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            return;
        }
    }
}
