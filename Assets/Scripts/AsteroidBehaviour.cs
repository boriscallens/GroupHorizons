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
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        _rigidbody.mass = size;
        _rigidbody.AddForce(_transform.forward * speed);
        _transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        _transform.localScale = Vector3.one * size;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
