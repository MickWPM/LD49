using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObjectUIUpdater : MonoBehaviour
{
    Island island;
    public TMPro.TextMeshProUGUI placedBuildingText;

    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
    }

    private void Start()
    {
        island.BuildingPlacedEvent += Island_BuildingPlacedEvent;
        island.BuildingRemovedEvent += Island_BuildingRemovedEvent;
        UpdateBuildingPlacedUI();
    }

    private void Island_BuildingRemovedEvent(Building.BuildingType obj)
    {
        UpdateBuildingPlacedUI();
    }

    private void Island_BuildingPlacedEvent(Building.BuildingType obj)
    {
        UpdateBuildingPlacedUI();
    }

    void UpdateBuildingPlacedUI()
    {
        placedBuildingText.text = $"Houses: {island.HouseCount}\nLumberyards: {island.LumberyardCount}\nFishing Huts: {island.FishingHutCount}\nMines: {island.MineCount}";
    }
}
