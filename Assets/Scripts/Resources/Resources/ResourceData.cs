using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    ORE,
    PLANT
}
[CreateAssetMenu(menuName = "NewResource")]
public class ResourceData : ScriptableObject
{
    public string resourceName;

    public GameObject sourceModel;
    public GameObject materialModel;

    public int durability;

    public float materialValue;

    public ResourceType resourceType;
}
