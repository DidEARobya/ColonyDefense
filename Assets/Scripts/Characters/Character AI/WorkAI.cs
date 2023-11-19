using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum AIStates
{
    IDLE,
    ACTIVE
}
public enum TargetType
{
    RESOURCE,
    MATERIAL
}

public class WorkAI : MonoBehaviour
{
    protected CharacterControl characterControl;

    protected GameObject checkpoint;

    protected BehaviourTreeBase behaviourTree;

    protected AIStates state;

    protected Vector2 idlePos;

    public List<Task> tasks = new List<Task>();
    private Task activeTask;

    public bool isReady;

    protected bool isIdle;
    protected bool workCheck;
    // Start is called before the first frame update
    public void Init(CharacterControl character)
    {
        if (character == null)
        {
            return;
        }

        state = AIStates.IDLE;
        isIdle = false;
        characterControl = character;

        checkpoint = GameObject.Find("Checkpoint");

        behaviourTree = GetComponent<BehaviourTreeBase>();

        workCheck = false;
        isReady = true;
    }

    // Update is called once per frame
    public void AIUpdate()
    {
        if (isReady == false)
        {
            return;
        }

        if(tasks.Count > 0 )
        {
            state = AIStates.ACTIVE;

            if(activeTask == null)
            {
                activeTask = tasks[0];
                activeTask.StartTask();
            }
            else if(activeTask != tasks[0])
            {
                activeTask.StopTask();
                activeTask = tasks[0];
                activeTask.StartTask();
            }
        }
        else
        {
            state = AIStates.IDLE;
        }

        if(state == AIStates.IDLE)
        {
            if (isIdle == false)
            {
                idlePos = characterControl.transform.position;
                isIdle = true;
            }

            characterControl.IdleCharacter(idlePos);

            if (workCheck == false)
            {
                workCheck = true;
                Invoke("FindWork", 0.2f);
            }
        }
        else
        {
            isIdle = false;

            if(activeTask == null)
            {
                return;
            }

            if(activeTask.IsTargetComplete() == true)
            {
                tasks[0].StopTask();
                tasks.RemoveAt(0);
                activeTask = null;
            }
        }
    }

    public void ResetAI()
    {
        isIdle = false;

        state = AIStates.IDLE;
    }
    protected void FindWork()
    {
        workCheck = false;

        Dictionary<MonoBehaviour, TargetType> targetInfo = behaviourTree.GetTask(characterControl);

        if (targetInfo.Count == 0)
        {
            return;
        }

        Task task = ((IInteractable)targetInfo.First().Key).Task(characterControl);

        if(task == null)
        {
            return;
        }

        tasks.Add(task);
    }
}
