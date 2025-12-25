using UnityEngine;

public class FarmLand : MonoBehaviour
{
    public GameObject tomatoPlantPrefab; // assign the tomato prefab

    private GameObject currentPlant; // current tomato on this tile

    void Update()
    {
        // Show distance from player to tile center every frame
        if (PlayerController.instance != null)
        {
            float distance = Vector3.Distance(PlayerController.instance.transform.position, transform.position);
            Debug.Log("Distance to farmland tile: " + distance);
        }
    }

    // Check if this tile is empty
    public bool IsEmpty() => currentPlant == null;

    // Plant a tomato on this tile
    public void PlantTomato()
    {
        if (tomatoPlantPrefab != null && currentPlant == null)
        {
            currentPlant = Instantiate(tomatoPlantPrefab, transform.position, Quaternion.identity);
            SimplePlant sp = currentPlant.GetComponent<SimplePlant>();
            if (sp != null)
            {
                sp.StartGrowth();
                sp.OnHarvest += OnPlantHarvested;
            }

            Debug.Log("Tomato planted successfully!");
        }
    }

    // Called when plant is harvested
    void OnPlantHarvested()
    {
        currentPlant = null;
        Debug.Log("Farmland is empty, ready to plant again.");
    }
}
