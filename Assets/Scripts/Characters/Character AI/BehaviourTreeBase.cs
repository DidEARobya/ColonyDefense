using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static WorkAI;

public enum TaskName
{
    MINE,
    TENDPLANTS,
    HARVEST,
    GATHER
}
public enum TaskPriority
{
    ONE,
    TWO,
    THREE,
    FOUR
}
public class BehaviourTreeBase : MonoBehaviour
{
    public Dictionary<TaskName, TaskPriority> taskDict;

    public void Init(Dictionary<TaskName, TaskPriority> priorities)
    {
        taskDict = priorities;
    }
    public Dictionary<MonoBehaviour, TargetType> GetTask(CharacterControl control)
    {
        List<TaskName> priorityList = taskDict.Keys.ToList<TaskName>();

        Dictionary<MonoBehaviour, TargetType> targetInfo = null;

        if (priorityList.Count == 0)
        {
            return null;
        }

        targetInfo = CheckForTask(priorityList, TaskPriority.ONE, control);

        if (targetInfo.Count == 0)
        {
            targetInfo = CheckForTask(priorityList, TaskPriority.TWO, control);
        }

        if (targetInfo.Count == 0)
        {
            targetInfo = CheckForTask(priorityList, TaskPriority.THREE, control);
        }

        if (targetInfo.Count == 0)
        {
            targetInfo = CheckForTask(priorityList, TaskPriority.FOUR, control);
        }

        return targetInfo;
    }

    private Dictionary<MonoBehaviour, TargetType> CheckForTask(List<TaskName> priorityList, TaskPriority priority, CharacterControl control)
    {
        Dictionary<MonoBehaviour, TargetType> targetInfo = new Dictionary<MonoBehaviour, TargetType>();

        for (int i = 0; i < priorityList.Count; i++)
        {
            if (taskDict[priorityList[i]] != priority)
            {
                break;
            }

            MonoBehaviour target = null;

            switch (priorityList[i])
            {
                case TaskName.MINE:

                    target = GameHandler.GetClosestAvailableOre_Static(control);

                    if (target != null)
                    {
                        targetInfo.Add(target, TargetType.RESOURCE);
                    }

                    break;

                case TaskName.TENDPLANTS:

                    target = GameHandler.GetClosestAvailableSeed_Static(control);

                    if (target != null)
                    {
                        targetInfo.Add(target, TargetType.RESOURCE);
                    }

                    break;

                case TaskName.HARVEST:

                    target = GameHandler.GetClosestAvailablePlant_Static(control);

                    if (target != null)
                    {
                        targetInfo.Add(target, TargetType.RESOURCE);
                    }

                    break;

                case TaskName.GATHER:

                    target = GameHandler.GetClosestAvailableMaterial_Static(control);

                    if (target != null)
                    {
                        targetInfo.Add(target, TargetType.MATERIAL);
                    }

                    break;

            }

            if (targetInfo.Count != 0)
            {
                return targetInfo;
            }
        }

        return targetInfo;
    }
}

