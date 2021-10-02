using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public Building buildingToBePlaced;
    public Building LumberYard;
    public Building House;
    public Building FishingHut;
    public Building Mine;

    Vector3 spawnRotation;
    public Vector3 SpawnRotation { get => spawnRotation; private set => spawnRotation = value; }


    public Island island;
    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
    }

    public void TryPlaceBuilding(Vector3 pos, Resource resource)
    {
        Vector3 rotToPlace = spawnRotation;
        SpawnRotation = new Vector3(0, buildingToBePlaced.GetRandomRotation(), 0);
        island.TryPlaceBuilding(buildingToBePlaced, pos, rotToPlace, resource);
    }

    public void SetBuildingHouse()
    {
        SetBuildingToBePlaced(House);
    }
    public void SetBuildingLumberyard()
    {
        SetBuildingToBePlaced(LumberYard);
    }

    public void SetBuildingFishingHut()
    {
        SetBuildingToBePlaced(FishingHut);
    }
    public void SetBuildingMine()
    {
        SetBuildingToBePlaced(Mine);
    }

    public event System.Action<Building> BuildingSetToBePlacedEvent;
    void SetBuildingToBePlaced(Building b)
    {
        buildingToBePlaced = b;
        SpawnRotation = new Vector3(0, b.GetRandomRotation(), 0);

        BuildingSetToBePlacedEvent?.Invoke(b);
    }


    public bool CanBuildingBePlacedOnResource(RaycastHit raycastHit, out Vector3 placementLocation, out Resource placementResource)
    {
        placementResource = null;
        placementLocation = Vector3.zero;
        //eg. maybe only woodcutters can be placed in forests
        //Check tag/layer from raycast hit
        Resource resource = raycastHit.collider.gameObject.GetComponent<Resource>();
        if (resource != null)
        {
            if (buildingToBePlaced.AllowedResoucePlacements.Contains(resource.ResourceType) == false) return false;
            //check if we can place here then return
            placementLocation = buildingToBePlaced.setResourceInactiveOnPlacement ? resource.transform.position : raycastHit.point;
            placementResource = resource;
            return true;
        }

        if (buildingToBePlaced.AllowedResoucePlacements.Count > 0 && buildingToBePlaced.AllowedResoucePlacements.Contains(ResourceType.NONE) == false) return false;

        Building building = raycastHit.collider.gameObject.GetComponent<Building>();
        if (building != null)
        {
            return false;
        }

        Island island = raycastHit.collider.gameObject.GetComponentInParent<Island>();
        if (island != null && (buildingToBePlaced.locationPlacement == Building.IslandLocationPlacement.ALL || buildingToBePlaced.locationPlacement == Building.IslandLocationPlacement.GRASS))
        {
            //We can place stuff on the island
            placementLocation = raycastHit.point;
            return true;
        }

        Beach beach = raycastHit.collider.gameObject.GetComponentInParent<Beach>();
        if (beach != null && (buildingToBePlaced.locationPlacement == Building.IslandLocationPlacement.ALL || buildingToBePlaced.locationPlacement == Building.IslandLocationPlacement.BEACH))
        {
            //We can place stuff on the island
            placementLocation = raycastHit.point;
            return true;
        }

        return false;
    }
}
