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
    private int _boundaryCollisionCount;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        _rigidbody.mass = size;
        _rigidbody.AddForce(_transform.forward * speed);
        _rigidbody.AddTorque(12 * size);
        _transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        _transform.localScale = Vector3.one * size;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Asteroid colliding");

        var isBoundaryCollision = collision.collider.IsTouchingLayers(LayerMask.GetMask("Boundary"));
        if (isBoundaryCollision)
        {
            _boundaryCollisionCount++;
        }

        if (_boundaryCollisionCount > 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var isBoundaryCollision = collision.gameObject.layer == LayerMask.NameToLayer("Boundary");
        if (isBoundaryCollision)
        {
            _boundaryCollisionCount++;
        }
        if (_boundaryCollisionCount > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
