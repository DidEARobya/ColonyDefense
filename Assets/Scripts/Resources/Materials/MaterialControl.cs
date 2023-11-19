using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialControl : CarryableObject, IInteractable
{
    private WorldGrid worldGrid;
    private ResourceData resourceData;

    private GameObject checkpoint;
    public ResourceStates state;

    private float newInteractWait;
    private float newInteractDelay;

    public void Init(ResourceData data, WorldGrid grid)
    {
        if(data == null)
        {
            return;
        }

        worldGrid = grid;
        resourceData = data;
        state = ResourceStates.MATERIAL;

        isTargeted = false;

        newInteractWait = 0.5f;
        newInteractDelay = 0f;

        checkpoint = GameObject.Find("Checkpoint");
    }
    // Update is called once per frame
    void Update()
    {
        if(targetedBy == null)
        {
            isTargeted = false;
            return;

        }

        isTargeted = true;
        newInteractDelay += Time.deltaTime;

        if (newInteractDelay >= newInteractWait)
        {
            if (targetedBy.CheckIfStopped(0.05f) == true)
            {
                if(targetedBy.heldObject != this)
                {
                    Carry(targetedBy);
                }
            }
        }

        if (targetedBy.heldObject == this)
        {
            this.transform.position = targetedBy.transform.position + new Vector3(0, 0.25f, 0);

            if(targetedBy.isSelected == false)
            {
                targetedBy.MoveCharacterTo(checkpoint.transform.position);
            }
        }
    }
    public CharacterControl GetCharacterControl()
    {
        return targetedBy;
    }
    public override void Store()
    {
        if (targetedBy != null)
        {
            Drop();  
        }

        GameHandler.RemoveMaterial_Static(this);
        PlayerHandler.StoreMaterial_Static(resourceData.materialModel.GetComponent<SpriteRenderer>().sprite, 1);

        Destroy(this.gameObject);
    }
    public void Interact(CharacterControl character)
    {
        if (targetedBy.heldObject != null)
        {
            targetedBy.heldObject.Drop();
        }

        targetedBy.aiPath.endReachedDistance = 0;
        targetedBy.MoveCharacterTo(this.transform.position);
        newInteractDelay = 0;
    }
    public Task Task(CharacterControl character)
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
        if (targetedBy == null)
        {
            return;
        }

        if (newObj.name.Contains("World") == true)
        {
            if (targetedBy.heldObject != this)
            {
                Drop();
                this.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(this.transform.position));
            }
        }
        else
        {
            if (targetedBy != null)
            {
                Drop();
                this.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(this.transform.position));
            }
        }
    }
}
