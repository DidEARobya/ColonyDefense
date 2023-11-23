using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTend : Task
{
    public TaskTend(CharacterControl owner, GameObject target) : base(owner, target)
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

        if (taskTarget == null)
        {
            StopTask();
            return true;
        }

        if(taskTarget.GetComponent<PlantControl>() == null)
        {
            return false;
        }

        if(taskTarget.GetComponent<PlantControl>().needsTending == true)
        {
            return false;
        }

        StopTask();
        return true;
    }
}
