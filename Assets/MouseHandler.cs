using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHandler : MonoBehaviour
{
    public MouseState mouseState;
    BuildingPlacer buildingPlacer;
    Island island;
    private void Awake()
    {
        buildingPlacer = gameObject.GetComponent<BuildingPlacer>();
        buildingPlacer.BuildingSetToBePlacedEvent += BuildingPlacer_BuildingSetToBePlacedEvent;
        island = GameObject.FindObjectOfType<Island>();
        island.BuildingPlacedEvent += Island_BuildingPlacedEvent;
    }

    private void Island_BuildingPlacedEvent(Building.BuildingType obj)
    {
        previewGO.transform.rotation = Quaternion.Euler(buildingPlacer.SpawnRotation);
    }

    private void BuildingPlacer_BuildingSetToBePlacedEvent(Building buildingToBePlaced)
    {
        mouseState = MouseState.BUILDING_PLACEMENT;
        if (previewGO != null)
            Destroy(previewGO.gameObject);

        Building previewBuilding = Instantiate(buildingToBePlaced);
        previewBuilding.SetAsPreview();
        previewGO = previewBuilding.gameObject;
        previewGO.transform.rotation = Quaternion.Euler(buildingPlacer.SpawnRotation);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(1)) ReturnToDefaultState();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildingPlacer.SetBuildingHouse();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buildingPlacer.SetBuildingLumberyard();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            buildingPlacer.SetBuildingFishingHut();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            buildingPlacer.SetBuildingMine();
        }

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

    GameObject previewGO;

    void BuildingPlacementMouseStateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        previewGO.SetActive(false);
        if (Physics.Raycast(ray, out raycastHit, 1000f)) //Add layer mask 
        {
            Vector3 placementLoc;
            Resource placementResource = null;
            bool clearGround = buildingPlacer.CanBuildingBePlacedOnResource(raycastHit, out placementLoc, out placementResource);
            //Can we place building
            if(clearGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    buildingPlacer.TryPlaceBuilding(placementLoc, placementResource); 
                } else
                {
                    previewGO.SetActive(true);
                    previewGO.transform.position = placementLoc;
                }
            }
        }
        else
        {
        }
    }

    public void ReturnToDefaultState()
    {
        mouseState = MouseState.DEFAULT;
        if (previewGO != null)
            Destroy(previewGO.gameObject);
    }

    public GameObject hoverObject;


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
