using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDestroy : Task
{
    public TaskDestroy(CharacterControl owner, GameObject target) : base(owner, target)
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
            Debug.Log("No Owner");
            return false;
        }

        if (taskTarget != null)
        {
            return false;
        }

        return true;
    }

}
