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

    public Material buildingPreviewMat;
    public void SetAsPreview()
    {
        foreach (var mr in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = buildingPreviewMat;
        }
        Collider c = gameObject.GetComponent<Collider>();
        c.enabled = false;
    }

    public bool RotationFacesOut = false;
    public float GetRandomRotation()
    {
        if (RotationFacesOut == false)
            return Random.Range(-180f, 180f);

        Debug.LogWarning("This is not functioning correcty");
        float signedAngle = Vector2.SignedAngle(Vector2.zero, new Vector2(transform.position.x, transform.position.z));
        return signedAngle;
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
