using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<ResourceType> AllowedResoucePlacements;
    public int weight;
    public float effectiveWeight;
    public IslandLocationPlacement locationPlacement = IslandLocationPlacement.GRASS;

    Island island;
    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
    }

    public void Tick(float deltaTime)
    {

    }

    Resource thisResource;
    public void PlacedOnResource(Resource r)
    {
        thisResource = r;
        thisResource.gameObject.SetActive(false);
    }

    public void DeleteBuilding()
    {
        island.RemoveBuilding(this);
        DestroyBuilding();
    }

    public void BuildingUnderwater()
    {
        island.RemoveBuilding(this);
        DestroyBuilding();
    }

    void DestroyBuilding()
    {
        if(thisResource != null)
        {
            thisResource.gameObject.SetActive(true);
        }
    }

    public enum IslandLocationPlacement
    {
        ALL,
        GRASS,
        BEACH
    }
}
