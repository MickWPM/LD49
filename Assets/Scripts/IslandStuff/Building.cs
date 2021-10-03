using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<ResourceType> AllowedResoucePlacements;
    public int weight;
    public float effectiveWeight;
    public IslandLocationPlacement locationPlacement = IslandLocationPlacement.GRASS;
    public BuildingType buildingType;
    AudioSource audioSource;
    public AudioClip splashClip;
    AudioHelper audioHelper;
    Island island;
    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioHelper = GameObject.FindObjectOfType<AudioHelper>();
    }

    public Material buildingPreviewMat;
    public void SetAsPreview()
    {
        if (audioSource != null) audioSource.enabled = false;

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

    Resource thisResource;
    public bool setResourceInactiveOnPlacement = true;
    public void PlacedOnResource(Resource r)
    {
        thisResource = r;
        if (setResourceInactiveOnPlacement)
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

        if (splashClip != null)
        {
            audioHelper.PlayClip(splashClip);
        }
        DestroyBuilding();
    }

    void DestroyBuilding()
    {
        if(thisResource != null)
        {
            thisResource.gameObject.SetActive(true);
        }
    }

    public enum BuildingType
    {
        HOUSE,
        LUMBER_YARD,
        FISHING_HUT,
        MINE
    }

    public enum IslandLocationPlacement
    {
        ALL,
        GRASS,
        BEACH
    }
}
