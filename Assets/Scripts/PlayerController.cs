using Mirror;
using TMPro;
using UnityEngine;

// simple player controller class
// on spawn chooses random name with color and synchornizes them using SyncVar

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    public float movementSpeed = 5f;

    [SerializeField] 
    private TextMeshProUGUI playerInfo;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))] 
    public Color playerColor;

    private Rigidbody rb;
    private Vector3 movement;
    private Material playerMaterialClone;

    private void OnNameChanged(string oldName, string newName)
    {
        if (playerInfo != null)
        {
            playerInfo.text = newName;
        }
    }

    private void OnColorChanged(Color oldCol, Color newCol)
    {
        if (playerInfo != null)
        {
            playerInfo.color = newCol;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            playerMaterialClone = new Material(renderer.material);
            playerMaterialClone.color = newCol;
            renderer.material = playerMaterialClone;
        }
    }

    [Client]
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        playerInfo = GetComponentInChildren<TextMeshProUGUI>();
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            playerMaterialClone = new Material(renderer.material);
        }
    }

    [Client]
    void Start()
    {
        rb.freezeRotation = true;
        if (!isLocalPlayer) return;

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string name, Color col)
    {
        playerName = name;
        playerColor = col;
    }

    [Client]
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    }

    [Client]
    void FixedUpdate()
    {
        if (!isLocalPlayer)
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
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (_groundPlane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);

            var dir = hitPoint - transform.position;
            dir.y = 0;

            var targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
