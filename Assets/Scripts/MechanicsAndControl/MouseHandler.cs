using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHandler : MonoBehaviour
{
    public MouseState mouseState;
    BuildingPlacer buildingPlacer;
    Island island;

    public Texture2D defaultMousePointer;
    public Texture2D placingBuildingPointer;
    public Texture2D destroyBuildingPointer;
    public Texture2D cantPlaceMousePointer;
    public Vector2 cantPlacePointerOffset = new Vector2(128, 128);
    private void Awake()
    {
        buildingPlacer = gameObject.GetComponent<BuildingPlacer>();
        buildingPlacer.BuildingSetToBePlacedEvent += BuildingPlacer_BuildingSetToBePlacedEvent;
        island = GameObject.FindObjectOfType<Island>();
        island.BuildingPlacedEvent += Island_BuildingPlacedEvent;

        SetMousePointer(MousePointerStyle.NORMAL);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            SetMousePointer(MousePointerStyle.NORMAL);
            return;
        }


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
            
            SetMousePointer(clearGround ? MousePointerStyle.PLACING_BUILDING : MousePointerStyle.CANT_PLACE_BUILDING);

            //Can we place building
            if (clearGround)
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

            Building b = raycastHit.collider.gameObject.GetComponent<Building>();
            if (b != null)
            {
                SetMousePointer(MousePointerStyle.DESTROY_BUILDING);
                if(Input.GetMouseButtonDown(0))
                {
                    b.DeleteBuilding();
                }
            } else
            {
                SetMousePointer(MousePointerStyle.NORMAL);
            }
        }
        else
        {
            SetMousePointer(MousePointerStyle.NORMAL);
        }
    }

    void SetMousePointer(MousePointerStyle style)
    {
        Vector2 offset = Vector2.zero;
        Texture2D pointerTex = defaultMousePointer;
        switch (style)
        {
            case MousePointerStyle.PLACING_BUILDING:
                pointerTex = placingBuildingPointer;
                break;
            case MousePointerStyle.CANT_PLACE_BUILDING:
                pointerTex = cantPlaceMousePointer;
                offset = cantPlacePointerOffset;
                break;
            case MousePointerStyle.DESTROY_BUILDING:
                pointerTex = destroyBuildingPointer;
                break;
        }

        Cursor.SetCursor(pointerTex, offset, CursorMode.Auto);
    }
}

public enum MousePointerStyle
{
    NORMAL,
    PLACING_BUILDING,
    CANT_PLACE_BUILDING,
    DESTROY_BUILDING
}

public enum MouseState
{
    DEFAULT,
    BUILDING_PLACEMENT
}
