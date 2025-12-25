using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float harvestRange = 3f; // max harvest distance

    CharacterController controller;
    float verticalVelocity;
    float xRotation = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // WASD movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Plant nearest empty farmland
        if (Input.GetKeyDown(KeyCode.G))
            PlantNearestTile();

        // Harvest nearest stage 3 plant
        if (Input.GetKeyDown(KeyCode.E))
            HarvestNearestPlant();
    }

    void PlantNearestTile()
    {
        float minDistance = Mathf.Infinity;
        FarmLand nearestTile = null;

        FarmLand[] tiles = FindObjectsOfType<FarmLand>();
        foreach (FarmLand tile in tiles)
        {
            if (!tile.IsEmpty()) continue;

            float distance = Vector3.Distance(transform.position, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }

        if (nearestTile != null)
        {
            nearestTile.PlantTomato();
            Debug.Log("Planted on nearest tile at distance: " + minDistance);
        }
    }

    void HarvestNearestPlant()
    {
        float minDistance = Mathf.Infinity;
        SimplePlant nearestPlant = null;

        SimplePlant[] plants = FindObjectsOfType<SimplePlant>();
        foreach (SimplePlant plant in plants)
        {
            if (!plant.IsStage3()) continue; // only stage 3 can be harvested

            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlant = plant;
            }
        }

        if (nearestPlant != null && minDistance <= harvestRange)
        {
            nearestPlant.HarvestPlant();
            Debug.Log("Harvested nearest plant at distance: " + minDistance);
        }
    }
}
