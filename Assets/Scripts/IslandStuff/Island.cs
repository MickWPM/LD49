using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public List<Building> buildings;

    public int HouseCount { get; private set; }
    public int LumberyardCount { get; private set; }
    public int FishingHutCount { get; private set; }
    public int MineCount { get; private set; }

    public int HouseCountTarget { get; private set; }
    public int LumberyardCountTarget { get; private set; }
    public int FishingHutCountTarget { get; private set; }
    public int MineCountTarget { get; private set; }

    GameManager gameManager;
    private void Awake()
    {
        buildings = new List<Building>();
        targetRotation = transform.rotation.eulerAngles;
        gameManager = GameObject.FindObjectOfType<GameManager>();       //Better to have a central event handler
        gameManager.TaskIssuedEvent += GameManager_TaskIssuedEvent;
    }

    bool onTask = false;
    private void GameManager_TaskIssuedEvent(Task task)
    {
        HouseCountTarget += task.HouseRequirement;
        LumberyardCountTarget += task.LumberyardRequirement;
        FishingHutCountTarget += task.FishingHutRequirement;
        MineCountTarget += task.MineRequirement;
        onTask = true;
    }

    public bool TryPlaceBuilding(Building buildingToBePlaced, Vector3 pos, Resource r = null)
    {
        return TryPlaceBuilding(buildingToBePlaced, pos, Vector3.zero, r);
    }

    public event System.Action<Building.BuildingType> BuildingPlacedEvent;
    public event System.Action<Building.BuildingType> BuildingRemovedEvent;

    public bool TryPlaceBuilding(Building buildingToBePlaced, Vector3 pos, Vector3 rot, Resource r = null)
    {
        Building b = Instantiate(buildingToBePlaced);
        Transform t = b.transform;
        t.position = pos;
        t.SetParent(this.transform);
        t.localRotation = Quaternion.Euler(rot);

        buildings.Add(b);
        float midDist = Vector3.Magnitude(new Vector3(b.transform.position.x, 0, b.transform.position.z));
        b.effectiveWeight = b.weight * midDist;// * buildings.Count * buildings.Count;
        if (r != null) b.PlacedOnResource(r);

        totalWeight += b.weight;

        switch (b.buildingType)
        {
            case Building.BuildingType.HOUSE:
                HouseCount++;
                break;
            case Building.BuildingType.LUMBER_YARD:
                LumberyardCount++;
                break;
            case Building.BuildingType.FISHING_HUT:
                FishingHutCount++;
                break;
            case Building.BuildingType.MINE:
                MineCount++;
                break;
            default:
                break;
        }
        BuildingPlacedEvent?.Invoke(b.buildingType);
        return true;
    }

    public float totalWeight;
    private void Update()
    {
        UpdateWeightVector();
        AlignIsland();
        CheckBuildings();
        CheckTaskComplete();
    }


    public event System.Action TaskCompleteEvent;
    void CheckTaskComplete()
    {
        if (onTask == false) return;

        if(HouseCount >= HouseCountTarget &&
            LumberyardCount >= LumberyardCountTarget &&
            FishingHutCount >= FishingHutCountTarget &&
            MineCount >= MineCountTarget)
        {
            onTask = false;
            TaskCompleteEvent?.Invoke();
        }
    }

    public event System.Action<Building> BuildingUnderwaterEvent;
    void CheckBuildings()
    {
        List<Building> underwaterBuildings = new List<Building>();
        for (int i = buildings.Count - 1; i >= 0; i--)
        {
            Building b = buildings[i];
            if (b.transform.position.y < 0)
            {
                //underwater
                underwaterBuildings.Add(b);
                BuildingUnderwaterEvent?.Invoke(b);
            }
        }

        foreach (var b in underwaterBuildings)
        {
            b.BuildingUnderwater();
        }
    }

    public void RemoveBuilding(Building b)
    {
        switch (b.buildingType)
        {
            case Building.BuildingType.HOUSE:
                HouseCount--;
                break;
            case Building.BuildingType.LUMBER_YARD:
                LumberyardCount--;
                break;
            case Building.BuildingType.FISHING_HUT:
                FishingHutCount--;
                break;
            case Building.BuildingType.MINE:
                MineCount--;
                break;
            default:
                break;
        }
        buildings.Remove(b);
        BuildingRemovedEvent?.Invoke(b.buildingType);
        totalWeight -= b.weight;
        Destroy(b.gameObject);
        if (buildings.Count == 0) targetRotation = Vector3.zero;
    }

    Vector3 targetRotation;
    void AlignIsland()
    {
        //Deliberately use the wrong lerp to get a nice ease out!
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime);
    }

    public Vector3 averageSummedWeightVector;
    public float xTipDegrees = 20;
    public float zTipDegrees = 20;
    public float xTipDistance = 50;
    public float zTipDistance = 50;
    public float weightReductionScale = 600f;

    void UpdateWeightVector()
    {
        averageSummedWeightVector = Vector3.zero;
        if (buildings.Count == 0) return;

        foreach (var building in buildings)
        {
            //Lets just make weight scale as a function of distance to the middle squared
            //This is probably be WAY too agressive
            averageSummedWeightVector += new Vector3(building.transform.localPosition.x, building.effectiveWeight, building.transform.localPosition.z);
        }
        averageSummedWeightVector /= buildings.Count;

        //Store sign for x and z
        float xRot = Mathf.Lerp(0, zTipDegrees, Mathf.Abs(averageSummedWeightVector.z) / zTipDistance);
        float zRot = -Mathf.Lerp(0, xTipDegrees, Mathf.Abs(averageSummedWeightVector.x) / xTipDistance);
        targetRotation = new Vector3(Mathf.Sign(averageSummedWeightVector.z) * xRot, 0, Mathf.Sign(averageSummedWeightVector.x) * zRot);

        transform.position = new Vector3(0, -totalWeight / weightReductionScale, 0);
    }
}