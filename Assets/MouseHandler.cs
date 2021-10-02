using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public MouseState mouseState;

    void Update()
    {
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
            Resource placementResource;
            bool clearGround = CanBuildingBePlacedOnResource(raycastHit, out placementLoc, out placementResource);
            //Can we place building
            if(clearGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryPlaceBuilding(placementLoc); //We can try and pass the normal in here if we need differing rotations but for now, just use local identity
                    if (placementResource != null)
                        Destroy(placementResource.gameObject);
                }
            }
        }
        else
        {

        }
    }

    public Building buildingToBePlaced;
    bool CanBuildingBePlacedOnResource(RaycastHit raycastHit, out Vector3 placementLocation, out Resource placementResource)
    {
        placementResource = null;
        placementLocation = Vector3.zero;
        //eg. maybe only woodcutters can be placed in forests
        //Check tag/layer from raycast hit
        Resource resource = raycastHit.collider.gameObject.GetComponent<Resource>();
        if (resource != null)
        {
            //check if we can place here then return
            placementLocation = resource.transform.position;
            Debug.Log("We should tell the resource it is being used here - remove resource or update graphics. Currently just destroying the resource if building placed");
            placementResource = resource;
            return true;
        }

        Building building = raycastHit.collider.gameObject.GetComponent<Building>();
        if (building != null)
        {
            return false;
        }

        Island island = raycastHit.collider.gameObject.GetComponentInParent<Island>();
        if (island != null)
        {
            //We can place stuff on the island
            placementLocation = raycastHit.point;
            return true;
        }

        return false;
    }

    public Island island;
    void TryPlaceBuilding(Vector3 pos)//, Vector3 rotation)
    {
        island.TryPlaceBuilding(buildingToBePlaced, pos);
    }

    void DefaultMouseStateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f)) //Add layer mask 
        {
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
