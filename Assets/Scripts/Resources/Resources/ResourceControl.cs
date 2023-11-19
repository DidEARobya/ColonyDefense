using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourceStates
{
    SEED,
    SOURCE,
    MATERIAL
}
public class ResourceControl : MonoBehaviour, IInteractable
{
    //public BaseScript playerScript;
    protected WorldGrid worldGrid;
    protected GameObject materialsObject;

    private ResourceData resourceData;
    protected CharacterControl targetedBy;

    public ResourceStates state;
    protected float durability;
    public bool isTargeted;
    protected Bounds bounds;

    protected float workDelay;
    protected float workTime;

    protected float newInteractWait;
    protected float newInteractDelay;

    protected Rigidbody2D rb;

    public void Init(ResourceData data, Bounds b)
    {
        if(data == null)
        {
            return;
        }

        resourceData = data;

        rb = this.GetComponent<Rigidbody2D>();
        worldGrid = GameHandler.GetWorldGrid_Static();
        materialsObject = GameObject.Find("Materials");
        bounds = b;

        if (resourceData.resourceType == ResourceType.ORE)
        {
            state = ResourceStates.SOURCE;
            isTargeted = false;
        }

        workDelay = 1.0f;
        workTime = 0.0f;
        newInteractWait = 0.5f;
        newInteractDelay = 0f;
        durability = resourceData.durability;
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if (state != ResourceStates.SOURCE) 
        {
            return;
        }

        if (targetedBy == null)
        {
            isTargeted = false;
            return;
        }

        isTargeted = true;
        IsTargetted();
    }

    public CharacterControl GetCharacterControl()
    {
        return targetedBy;
    }
    protected virtual void IsFarmed()
    {
        targetedBy.interactingObject = null;
        targetedBy = null;
        SpawnMaterial(resourceData);
    }

    protected virtual void IsTargetted()
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

        if(targetedBy == null)
        {
            return;
        }

        if (targetedBy.interactingObject != this)
        {
            return;
        }

        DoWork();
    }
    protected virtual void DoWork()
    {
        workTime += Time.deltaTime;

        if (workTime >= workDelay)
        {
            durability -= targetedBy.characterObject.workSpeed;

            if (durability <= 0.0f)
            {
                IsFarmed();
                state = ResourceStates.MATERIAL;
            }

            workTime = 0;
        }
    }
    public void Interact(CharacterControl character)
    {
        if (targetedBy.heldObject != null)
        {
            targetedBy.heldObject.Drop();
        }

        targetedBy.aiPath.endReachedDistance = 1f;
        targetedBy.MoveCharacterTo(this.transform.position);
        newInteractDelay = 0;
    }
    public virtual Task Task(CharacterControl character)
    {
        if (targetedBy != null)
        {
            return null;
        }

        targetedBy = character;

        TaskDestroy task = new TaskDestroy(character, gameObject);

        return task;
    }
    public void Cancel(GameObject newObj)
    {
        if (targetedBy != null)
        {
            targetedBy.interactingObject = null;
            targetedBy = null;
            isTargeted = false;
        }
    }
    protected virtual void SpawnMaterial(ResourceData obj)
    {
        GameObject temp = Instantiate(obj.materialModel);
        temp.transform.name = obj.resourceName;
        temp.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(this.transform.position));
        temp.transform.SetParent(materialsObject.transform, false);

        MaterialControl control = temp.AddComponent<MaterialControl>();
        GameHandler.AddMaterial_Static(control);
        control.Init(obj, worldGrid);

        GameHandler.RemoveResource_Static(this);

        var guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        guo.addPenalty = 0;
        guo.modifyTag = true;
        guo.setTag = 0;
        AstarPath.active.UpdateGraphs(guo);

        Destroy(this.gameObject);
    }
}
