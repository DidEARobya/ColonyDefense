using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum StructureStates
{
    STABLE,
    DAMAGED,
    TODESTROY
}
public class StructureControl : ResourceControl, IStructure, IInteractable
{
    private StructureBase structureData;

    public StructureStates structureState;

    private float distanceOffset;

    public bool needsBuilding;
    public bool isBuilt;
    public void Init(StructureBase data)
    {
        if(data == null)
        {
            return;
        }
        needsBuilding = true;
        isBuilt = false;

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.75f);

        structureState = StructureStates.DAMAGED;

        newInteractWait = 0.5f;
        newInteractDelay = 0f;

        distanceOffset = 1f;

        structureData = data;
        durability = 0;

        workTime = 0;
        workDelay = 1;
    }

    // Update is called once per frame
    new void Update()
    {

        if(durability <= structureData.durability && targetedBy == null)
        {
            needsBuilding = true;
        }

        if (targetedBy == null)
        {
            isTargeted = false;
            return;
        }

        isTargeted = true;
        IsTargetted();
    }

    private void CheckForPlayer()
    {
        LayerMask mask = LayerMask.GetMask("Obstacle");

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2((transform.localScale.x + 0.25f), (transform.localScale.y + 0.25f)), 0f);

        if (hits.Length != 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<CharacterControl>() == true)
                {
                    if(targetedBy == hits[i].GetComponent<CharacterControl>())
                    {

                    }
                }
            }
        }
    }
    public void Repair()
    {
        if (needsBuilding == false)
        {
            return;
        }

        durability += targetedBy.characterObject.workSpeed;

        if (durability >= structureData.durability)
        {
            durability = structureData.durability;
            structureState = StructureStates.STABLE;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            needsBuilding = false;
            isBuilt = true;
        }
    }
    protected override void IsTargetted()
    {
        newInteractDelay += Time.deltaTime;

        if (newInteractDelay >= newInteractWait)
        {
            if (targetedBy.CheckIfStopped(0.5f) == true)
            {
                targetedBy.interactingObject = this;
            }
            else
            {
                targetedBy.interactingObject = null;
            }
        }

        if (targetedBy == null)
        {
            return;
        }

        if (targetedBy.interactingObject != this)
        {
            return;
        }

        DoWork();
    }
    protected override void DoWork()
    {
        workTime += Time.deltaTime;

        switch (structureState)
        {
            case StructureStates.DAMAGED:

                if (workTime >= workDelay)
                {
                    Repair();

                    workTime = 0;
                }

                break;

            case StructureStates.STABLE:
                break;

            case StructureStates.TODESTROY:

                if (workTime >= workDelay)
                {
                    IsFarmed();

                    workTime = 0;
                }

                break;

        }
    }
    public override List<Task> Task(CharacterControl character)
    {
        if (targetedBy != null)
        {
            return null;
        }

        List<Task> tasks = new List<Task>();

        Task task = null;
        targetedBy = character;

        switch(structureState)
        {
            case StructureStates.DAMAGED:

                task = new TaskBuild(character, gameObject);
                tasks.Add(task);
                break;

            case StructureStates.STABLE:
                break;

            case StructureStates.TODESTROY:
                task = new TaskDestroy(character, gameObject);
                tasks.Add(task);
                break;
        }

        return tasks;
    }
    public override Task Task(CharacterControl character, bool playerOverride)
    {
        if (targetedBy != null)
        {
            return null;
        }

        Task task = null;

        targetedBy = character;

        switch (structureState)
        {
            case StructureStates.DAMAGED:

                task = new TaskBuild(character, gameObject);
                break;

            case StructureStates.STABLE:
                break;

            case StructureStates.TODESTROY:
                task = new TaskDestroy(character, gameObject);
                break;
        }

        return task;
    }
    public void Destroy()
    {
        GameHandler.RemoveStructure_Static(this);
        Destroy(gameObject);
    }
    protected override void IsFarmed()
    {
        if(durability <= 0)
        {
            return;
        }

        durability -= targetedBy.characterObject.workSpeed;

        if (durability <= 0.0f)
        {
            durability = 0.0f;
            Destroy();
        }
    }
    public new void Interact(CharacterControl character)
    {
        if (targetedBy.heldObject != null)
        {
            targetedBy.heldObject.Drop();
        }

        targetedBy.aiPath.endReachedDistance = 1f;
        targetedBy.MoveCharacterTo(transform.position, 1);
        newInteractDelay = 0;
    }
}
