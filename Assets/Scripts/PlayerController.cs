using Unity.Netcode;
using UnityEngine;

// basic player movement controller

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    public float movementSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    void Start()
    {  
        rb.freezeRotation = true;
    }

    void Update()
    {
        if(!IsOwner)
        {
            return;
        }

        float _moveHorizontal = Input.GetAxis("Horizontal");
        float _moveVertical = Input.GetAxis("Vertical"); 

        movement = new Vector3(_moveHorizontal, 0.0f, _moveVertical);
    }

    void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }
        MovePlayer();
        HandleRotation();
    }

    void MovePlayer()
    {
        rb.MovePosition(transform.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    [SerializeField] private float _rotationSpeed = 450;
    private Plane _groundPlane = new(Vector3.up, Vector3.zero);
    private Camera cam;

    private void HandleRotation()
    {
        var _ray = cam.ScreenPointToRay(Input.mousePosition);

        if (_groundPlane.Raycast(_ray, out var _enter))
        {
            var _hitPoint = _ray.GetPoint(_enter);

            var _dir = _hitPoint - transform.position;
            var _rot = Quaternion.LookRotation(_dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, _rot, _rotationSpeed * Time.deltaTime);
        }
    }
}
