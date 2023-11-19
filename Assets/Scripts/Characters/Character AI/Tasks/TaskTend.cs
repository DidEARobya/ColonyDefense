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
            Debug.Log("No Owner");
            return false;
        }

        if (taskTarget == null)
        {
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

        return true;
    }
}
