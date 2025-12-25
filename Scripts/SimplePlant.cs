using UnityEngine;
using System;

public class SimplePlant : MonoBehaviour
{
    public GameObject tomatoFruit;  // prefab to drop when harvested
    public int fruitAmount = 3;     // number of fruits to drop
    private bool isStage3 = false;  // is fully grown

    public event Action OnHarvest;  // notify farmland when harvested

    void Start()
    {
        // Hide all stages at start
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    // Start growth sequence
    public void StartGrowth()
    {
        ShowStage(0);               // Stage 1
        Invoke("GrowToStage2", 1f); // Stage 2 after 1s
        Invoke("GrowToStage3", 2f); // Stage 3 after 2s
    }

    void ShowStage(int stageIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        if (stageIndex < transform.childCount)
        {
            transform.GetChild(stageIndex).gameObject.SetActive(true);
            Debug.Log("Showing stage: " + (stageIndex + 1));
        }
    }

    void GrowToStage2() => ShowStage(1);

    void GrowToStage3()
    {
        ShowStage(2);
        isStage3 = true;
        Debug.Log("Stage 3 reached! Press E to harvest.");
    }

    public bool IsStage3() => isStage3;

    // Harvest plant
    public void HarvestPlant()
    {
        DropFruits();
        OnHarvest?.Invoke();
        Destroy(gameObject);
        Debug.Log("Plant harvested!");
    }

    void DropFruits()
    {
        if (tomatoFruit == null) return;

        for (int i = 0; i < fruitAmount; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                0.2f,
                UnityEngine.Random.Range(-0.5f, 0.5f)
            );
            Instantiate(tomatoFruit, pos, Quaternion.identity);
        }
    }
}
