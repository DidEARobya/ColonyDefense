using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureStates
{
    STABLE,
    WORN,
    DAMAGED
}
public class StructureControl : MonoBehaviour, IStructure, IInteractable
{
    private StructureBase structureData;

    public StructureStates state;
    public int durability;
    public bool targetted;

    private float newInteractWait;
    private float newInteractDelay;

    public void Init(StructureBase data)
    {
        if(data == null)
        {
            return;
        }

        newInteractWait = 0.5f;
        newInteractDelay = 0f;

        structureData = data;
        durability = structureData.durability;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public CharacterControl GetCharacterControl()
    {
        return null;
    }
    public void Interact(CharacterControl character)
    {

    }
    public Task Task(CharacterControl character)
    {
        return null;
    }
    public void Cancel(GameObject obj)
    {

    }
    public void Repair()
    {

    }
    public void Destroy()
    {
        
    }
}
