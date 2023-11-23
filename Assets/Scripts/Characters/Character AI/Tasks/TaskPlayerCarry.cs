using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPlayerCarry : Task
{
    public TaskPlayerCarry(CharacterControl owner, GameObject target) : base(owner, target)
    {
        taskOwner = owner;
        taskTarget = target;
        taskActive = false;
        taskType = TaskTypes.PLAYER;

        checkpointTransform = GameObject.Find("Checkpoint").transform;
    }

    public override bool IsTargetComplete()
    {
        if (taskOwner == null)
        {
            Debug.Log("No Owner");
            StopTask();
            return true;
        }

        if (taskTarget == null)
        {
            StopTask();
            return true;
        }

        return false;
    }
}
