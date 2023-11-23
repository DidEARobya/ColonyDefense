using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskRetrieve : Task
{
    public TaskRetrieve(CharacterControl owner, GameObject target) : base(owner, target)
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

        if(taskOwner.heldObject == null)
        {
            return false;
        }

        if (taskOwner.heldObject.gameObject == taskTarget)
        {
            StopTask();
            return true;
        }

        return false;
    }
}