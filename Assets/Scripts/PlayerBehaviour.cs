using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    public float thrustSpeed = 1f;
    public float turnSpeed = 1f;


    private Rigidbody2D _rigidBody;
    private float _turnDirection;
    private bool _thrusting;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        var newTurnDirection = 0.0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newTurnDirection += 1.0f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newTurnDirection -= 1.0f;
        }
        _turnDirection = newTurnDirection;
    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            // can we do better then this implicit cast?
            _rigidBody.AddForce(this.transform.up * thrustSpeed);
        }

        if (_turnDirection != 0f)
        {
            _rigidBody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }
}
