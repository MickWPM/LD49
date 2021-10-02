using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObjectUIUpdater : MonoBehaviour
{
    Island island;
    GameManager gameManager;
    public TMPro.TextMeshProUGUI placedBuildingText;

    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        island.BuildingPlacedEvent += Island_BuildingPlacedEvent;
        island.BuildingRemovedEvent += Island_BuildingRemovedEvent;
        gameManager.TaskIssuedEvent += GameManager_TaskIssuedEvent;
        UpdateBuildingPlacedUI();
    }

    private void GameManager_TaskIssuedEvent(Task obj)
    {
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
        placedBuildingText.text = $"Houses: {island.HouseCount} / {island.HouseCountTarget}\n" +
            $"Lumberyards: {island.LumberyardCount} / {island.LumberyardCountTarget}\n" +
            $"Fishing Huts: {island.FishingHutCount} / {island.FishingHutCountTarget}" +
            $"\nMines: {island.MineCount} / {island.MineCountTarget}\n\n" +
            $"Total Tasks Complete: {gameManager.TasksCompleted}";
    }
}
