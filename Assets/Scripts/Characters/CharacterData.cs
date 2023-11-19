using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewCharacter")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject characterModel;

    public int health;
    public int attack;
    public int cost;
    public float moveSpeed;
    public int workSpeed;

    public List<TaskName> taskNames;
    public List<TaskPriority> defaultPriorities;
}
