using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float harvestRange = 3f; // distance to nearest plant

    CharacterController controller;
    float verticalVelocity;
    float xRotation = 0f;

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

        // Movement (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Harvest nearest plant
        if (Input.GetKeyDown(KeyCode.E))
        {
            HarvestNearestPlant();
        }
    }

    void HarvestNearestPlant()
    {
        SimplePlant[] allPlants = FindObjectsOfType<SimplePlant>();
        SimplePlant nearestPlant = null;
        float minDistance = harvestRange;

        foreach (SimplePlant plant in allPlants)
        {
            if (!plant.IsStage3()) continue; // only fully grown
            float dist = Vector3.Distance(transform.position, plant.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestPlant = plant;
            }
        }

        if (nearestPlant != null)
        {
            nearestPlant.HarvestPlant();
            Debug.Log("Harvested nearest plant at distance: " + minDistance);
        }
        else
        {
            Debug.Log("No plant nearby to harvest.");
        }
    }
}
