using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum TaskTypes
{
    STORE,
    PLAYER,
    OTHER
}
public class Task
{
    protected CharacterControl taskOwner;
    protected GameObject taskTarget;

    protected Transform checkpointTransform;

    protected bool taskActive;
    public TaskTypes taskType = TaskTypes.OTHER;

    public Task(CharacterControl owner, GameObject target)
    {
        taskOwner = owner;
        taskTarget = target;
        taskActive = false;

        checkpointTransform = GameObject.Find("Checkpoint").transform;
    }

    public void StartTask()
    {
        if(taskTarget == null)
        {
            return;
        }

        if (taskTarget.TryGetComponent(out IInteractable interactableObject))
        {
            interactableObject.Interact(taskOwner);
        }
        else
        {
            Debug.Log("Invalid Target");
        }

        taskActive = true;
    }
    public void StopTask()
    {
        if(taskOwner != null)
        {
            taskOwner.workAi.ResetAI();

            taskOwner = null;
        }    

        taskActive = false;

        if (taskTarget == null)
        {
            return;
        }

        if (taskTarget.TryGetComponent(out IInteractable interactableObject))
        {
            interactableObject.Cancel(taskTarget);
        }
        else
        {
            Debug.Log("Invalid Target");
        }

        taskTarget = null;
    }
    public virtual bool IsTargetComplete() { return false; }

    public GameObject GetTaskTarget()
    {
        return taskTarget;
    }
}
