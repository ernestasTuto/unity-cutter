using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// bullet that moves without rigidbody

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Vector3 _direction;

    private void Start()
    {      
        StartCoroutine(BulletSelfDestruct(5f));
    }

    private void Update()
    {
        transform.position += _direction * Time.deltaTime * speed;
    }

    IEnumerator BulletSelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
}
