using System.Collections.Generic;
using UnityEngine;
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

            var asteroidParameters = new AsteroidParameters
            {
                AsteroidPrefab = asteroidPrefab,
                Position = hits.point,
                Rotation = Quaternion.LookRotation(transformPosition - hits.point, Vector3.up)
            };

            Create(asteroidParameters);
            return;
        }
    }

    public static IEnumerable<AsteroidBehaviour> Create(params AsteroidParameters[] asteroidParameters)
    {
        Debug.Log($"Creating {asteroidParameters.Length} asteroids");

        var asteroids = new AsteroidBehaviour[asteroidParameters.Length];
        for (var i = 0; i < asteroidParameters.Length; i++)
        {
            var asteroidParameter = asteroidParameters[i];
            asteroids[i] = Instantiate(asteroidParameter.AsteroidPrefab, asteroidParameter.Position, asteroidParameter.Rotation);
            asteroids[i].size = asteroidParameter.Size;
            if (asteroidParameter.Sprite != null)
            {
                asteroids[i].SetSprite(asteroidParameter.Sprite);
            }
        }
        return asteroids;
    }
}

public class AsteroidParameters
{
    private float? _size;
    private Quaternion? _rotation;

    public AsteroidBehaviour AsteroidPrefab { get; set; }
    public Vector3 Position { get; set; }
    public float Size
    {
        get
        {
            _size ??= Random.Range(AsteroidPrefab.minSize, AsteroidPrefab.maxSize);
            return _size.Value;
        }
        set => _size = value;
    }
    public Quaternion Rotation
    {
        get
        {
            _rotation ??= Random.rotation;
            return _rotation.Value;
        }
        set => _rotation = value;
    }
    public Sprite Sprite { get; set; }
}
