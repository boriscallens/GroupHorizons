using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehaviour : MonoBehaviour
{
    public float speed = 500.0f;
    public float maxLifeTime = 10.0f;

    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _rigidBody2D.AddForce(direction * speed);
        Destroy(this.gameObject, this.maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
