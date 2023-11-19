using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathCustom : AIPath
{
    public int GetPathLength()
    {
        return path.vectorPath.Count;
    }
    public Vector3 GetWaypointPos(int index)
    {
        return path.vectorPath[index];
    }
}
