using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public MouseState mouseState;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) mouseState = MouseState.DEFAULT;

        switch (mouseState)
        {
            case MouseState.DEFAULT:
                DefaultMouseStateUpdate();
                break;
            case MouseState.BUILDING_PLACEMENT:
                BuildingPlacementMouseStateUpdate();
                break;
            default:
                Debug.LogError("Add mouse state for " + mouseState);
                break;
        }
    }


    void BuildingPlacementMouseStateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f)) //Add layer mask 
        {
            Vector3 placementLoc;
            Resource placementResource = null;
            bool clearGround = CanBuildingBePlacedOnResource(raycastHit, out placementLoc, out placementResource);
            //Can we place building
            if(clearGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryPlaceBuilding(placementLoc, placementResource); //We can try and pass the normal in here if we need differing rotations but for now, just use local identity
                }
            }
        }
        else
        {

        }
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

    void SetBuildingToBePlaced(Building b)
    {
        mouseState = MouseState.BUILDING_PLACEMENT;
        buildingToBePlaced = b;
    }

    public Building buildingToBePlaced;
    public Building LumberYard;
    public Building House;
    public Building FishingHut;

    bool CanBuildingBePlacedOnResource(RaycastHit raycastHit, out Vector3 placementLocation, out Resource placementResource)
    {
        hoverObject = raycastHit.collider.gameObject;
        placementResource = null;
        placementLocation = Vector3.zero;
        //eg. maybe only woodcutters can be placed in forests
        //Check tag/layer from raycast hit
        Resource resource = raycastHit.collider.gameObject.GetComponent<Resource>();
        if (resource != null)
        {
            if (buildingToBePlaced.AllowedResoucePlacements.Contains(resource.ResourceType) == false) return false;
            //check if we can place here then return
            placementLocation = resource.transform.position;
            Debug.Log("We should tell the resource it is being used here - remove resource or update graphics. Currently just destroying the resource if building placed");
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
    public GameObject hoverObject;

    public Island island;
    void TryPlaceBuilding(Vector3 pos, Resource resource)
    {
        island.TryPlaceBuilding(buildingToBePlaced, pos, resource);
    }

    void DefaultMouseStateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f)) //Add layer mask 
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Building b = raycastHit.collider.gameObject.GetComponent<Building>();
                if (b != null)
                    b.DeleteBuilding();
            }
        }
        else
        {
        }
    }
}

public enum MouseState
{
    DEFAULT,
    BUILDING_PLACEMENT
}
