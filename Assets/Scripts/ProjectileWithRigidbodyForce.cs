using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for a bullet with a rigidbody
public class ProjectileWithRigidbodyForce : Projectile
{
    [SerializeField] private Rigidbody rb;

    public override void Init(Vector3 _direction)
    {
        rb.velocity = _direction * projectileSpeed;
    }
}
