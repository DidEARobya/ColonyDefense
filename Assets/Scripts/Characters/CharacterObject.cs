using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public CharacterData characterData;

    [HideInInspector]
    public string characterName;
    [HideInInspector]
    public GameObject characterModel;

    [HideInInspector]
    public int health;
    [HideInInspector]
    public int attack;
    [HideInInspector]
    public int cost;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public int workSpeed;
    [HideInInspector]
    public Dictionary<TaskName, TaskPriority> taskPriorities;

    // Start is called before the first frame update
    public void Init()
    {
        if (characterData == null)
        {
            return;
        }

        characterName = characterData.characterName;
        characterModel = characterData.characterModel;

        health = characterData.health;
        attack = characterData.attack;
        cost = characterData.cost;
        moveSpeed = characterData.moveSpeed;
        workSpeed = characterData.workSpeed;
    }
    public GameObject LoadCharacter()
    {
        GameObject temp = characterModel;
        temp.name = characterName;

        return temp;
    }
    public Dictionary<TaskName, TaskPriority> GetPriorities()
    {
        taskPriorities = new Dictionary<TaskName, TaskPriority>();

        for (int i = 0; i < characterData.taskNames.Count; i++)
        {
            taskPriorities.Add(characterData.taskNames[i], characterData.defaultPriorities[i]);
        }

        return taskPriorities;
    }
}
