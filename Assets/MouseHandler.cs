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
            bool clearGround = CanBuildingBePlacedOnResource(raycastHit);
            //Can we place building
            if(clearGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryPlaceBuilding(raycastHit.point, raycastHit.normal);
                }
            }
        }
        else
        {

        }
    }


    bool CanBuildingBePlacedOnResource(RaycastHit raycastHit)
    {
        //eg. maybe only woodcutters can be placed in forests
        //Check tag/layer from raycast hit
        return raycastHit.collider.gameObject.CompareTag("Island") || raycastHit.collider.gameObject.CompareTag("Resource");
        return true;
    }

    public Transform island;
    public Transform BuildingPrefab;
    void TryPlaceBuilding(Vector3 pos, Vector3 rotation)
    {
        Transform t = Instantiate(BuildingPrefab);
        t.position = pos;
        t.up = rotation;
        t.SetParent(island);
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
