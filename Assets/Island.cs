using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public List<Building> buildings;

    private void Awake()
    {
        buildings = new List<Building>();
        targetRotation = transform.rotation.eulerAngles;
    }

    public bool TryPlaceBuilding(Building buildingToBePlaced, Vector3 pos)
    {
        Building b = Instantiate(buildingToBePlaced);
        Transform t = b.transform;
        t.position = pos;
        //t.up = rotation;  //This is if we pass the hit normal
        t.SetParent(this.transform);
        t.localRotation = Quaternion.identity;

        buildings.Add(b);
        float midDist = Vector3.Magnitude(new Vector3(b.transform.position.x, 0, b.transform.position.z));
        b.effectiveWeight = b.weight * midDist;// * buildings.Count * buildings.Count;
        totalWeight += b.weight;

        return true;
    }

    public float totalWeight;
    private void Update()
    {
        UpdateWeightVector();
        AlignIsland();
    }

    Vector3 targetRotation;
    void AlignIsland()
    {
        //Deliberately use the wrong lerp to get a nice ease out!
        //transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, targetRotation, Time.deltaTime));

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime);
        
        //transform.rotation = Quaternion.Euler(targetRotation);
    }

    public Vector3 averageSummedWeightVector;
    public float xTipDegrees = 20;
    public float zTipDegrees = 20;
    public float xTipDistance = 50;
    public float zTipDistance = 50;
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

        transform.position = new Vector3(0, -totalWeight/200f, 0);
    }


    public float gizmoScaleReduction = 1000f;
    private void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        foreach (var b in buildings)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(b.transform.position, b.transform.position + Vector3.down * b.effectiveWeight  / gizmoScaleReduction);
        }
        Gizmos.color = Color.black;
        
        Gizmos.DrawLine(new Vector3(averageSummedWeightVector.x, 0, averageSummedWeightVector.z), new Vector3(averageSummedWeightVector.x, 100, averageSummedWeightVector.z));

        Gizmos.color = c;
    }
}
