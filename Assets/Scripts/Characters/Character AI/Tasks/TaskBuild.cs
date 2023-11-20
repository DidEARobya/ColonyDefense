using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBuild : Task
{
    public TaskBuild(CharacterControl owner, GameObject target) : base(owner, target)
    {
        taskOwner = owner;
        taskTarget = target;
        taskActive = false;

        checkpointTransform = GameObject.Find("Checkpoint").transform;
    }

    public override bool IsTargetComplete()
    {
        if (taskOwner == null)
        {
            return false;
        }

        if (taskTarget == null)
        {
            return true;
        }

        if (taskTarget.GetComponent<StructureControl>() == null)
        {
            return false;
        }

        if (taskTarget.GetComponent<StructureControl>().needsBuilding == true)
        {
            return false;
        }

        return true;
    }
}
