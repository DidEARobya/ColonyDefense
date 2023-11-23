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
            StopTask();
            return true;
        }

        if (taskTarget != null)
        {
            return false;
        }

        StopTask();
        return true;
    }

}
