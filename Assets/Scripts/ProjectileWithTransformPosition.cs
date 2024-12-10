using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// projectile script without rigidbody

public class ProjectileWithTransformPosition : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10;
    private Vector3 velocity;

    public void Init(Vector3 _direction)
    {
        velocity = _direction * projectileSpeed;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
