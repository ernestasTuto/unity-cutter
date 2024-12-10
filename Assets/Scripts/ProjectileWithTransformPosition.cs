using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for a bullet without rigidbody
public class ProjectileWithTransformPosition : Projectile
{
    private Vector3 velocity;

    public override void Init(Vector3 _direction)
    {
        velocity = _direction * projectileSpeed;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
