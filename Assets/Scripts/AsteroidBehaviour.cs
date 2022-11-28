using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Transform))]
public class AsteroidBehaviour : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float speed = 500.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _rigidbody.mass = size;
        _rigidbody.AddForce(_transform.forward * speed);
        _rigidbody.AddTorque(12 * size);
        _transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        _transform.localScale = Vector3.one * size;
        SetSprite(sprites[Random.Range(0, sprites.Length)]);
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isDespawnCollision = collision.gameObject.layer == LayerMask.NameToLayer("DespawnBoundary");
        var isPlayerCollision = collision.gameObject.layer == LayerMask.NameToLayer("Player");
        var isBulletCollision = collision.gameObject.layer == LayerMask.NameToLayer("Bullet");

        if (isDespawnCollision)
        {
            Destroy(this.gameObject);
        }
        if (isBulletCollision)
        {
            SplitAsteroid();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void SplitAsteroid()
    {
        const int splitNumber = 2;
        var splitSize = size / splitNumber;
        if (splitSize < minSize) return;

        var transformPosition = transform.position;
        var parameters = Enumerable.Range(0, splitNumber)
            .Select(_ => (Vector3)Random.insideUnitCircle)
            .Select(displacement => new AsteroidParameters
            {
                AsteroidPrefab = this,
                Position = transformPosition + displacement,
                Size = splitSize,
                Sprite = this._spriteRenderer.sprite
            }).ToArray();
        AsteroidSpawnBehaviour.Create(parameters);
    }
}
