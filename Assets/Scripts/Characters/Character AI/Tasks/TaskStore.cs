using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStore : Task
{
    public TaskStore(CharacterControl owner, GameObject target) : base(owner, target)
    {
        taskOwner = owner;
        taskTarget = target;
        taskActive = false;
        taskType = TaskTypes.STORE;

        checkpointTransform = GameObject.Find("Checkpoint").transform;
    }

    public override bool IsTargetComplete()
    {
        if (taskOwner == null)
        {
            StopTask();
            return true;
        }

        if (taskOwner.heldObject == null)
        {   
            StopTask();
            return true;
        }

        return false;
    }
}
