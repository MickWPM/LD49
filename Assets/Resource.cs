using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType ResourceType;
}

public enum ResourceType
{
    NONE,
    WOOD
}
