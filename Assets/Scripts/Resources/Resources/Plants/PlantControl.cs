using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantControl : ResourceControl
{
    private PlantData plantData;

    private float growthProgress;

    private float growthTime;
    private float growthDelay;

    private float tendMax;
    private float tendVal;
    private float tendDelay;
    private float tendTime;

    public bool needsTending;
    public new void Init(ResourceData data, Bounds b)
    {
        if (data == null)
        {
            return;
        }

        plantData = (PlantData)data;

        rb = this.GetComponent<Rigidbody2D>();
        worldGrid = GameHandler.GetWorldGrid_Static();
        materialsObject = GameObject.Find("Materials");
        bounds = b;

        if(plantData.resourceType == ResourceType.PLANT)
        {
            state = ResourceStates.SEED;
            isTargeted = false;

            tendDelay = plantData.tendCooldown;
            tendTime = 0;

            tendMax = 100;
            tendVal = 0;

            growthProgress = 0;
            growthTime = 0;
            growthDelay = plantData.growthDelay;
        }

        workDelay = 1.0f;
        workTime = 0.0f;
        newInteractWait = 0.5f;
        newInteractDelay = 0f;
        durability = plantData.durability;
    }

    // Update is called once per frame
    private new void Update()
    {
        if (state == ResourceStates.SEED)
        {
            SeedGrowth();
        }

        if (targetedBy == null)
        {
            isTargeted = false;
            return;
        }

        isTargeted = true;

        IsTargetted();
    }
    private void SeedGrowth()
    {
        tendTime += Time.deltaTime;

        if (tendTime >= tendDelay)
        {
            if(needsTending == false)
            {
                tendVal = 0;
            }

            needsTending = true;
        }

        if (needsTending == false)
        {
            growthTime += Time.deltaTime;

            if (growthTime >= growthDelay)
            {
                growthProgress += plantData.growthRate;
                growthTime = 0;
            }
        }

        if (growthProgress >= durability)
        {
            GameHandler.RemoveSeed_Static(this);
            GameHandler.AddPlant_Static(this);

            state = ResourceStates.SOURCE;
            this.GetComponent<SpriteRenderer>().sprite = plantData.plantSprite;
        }
    }
    protected override void IsTargetted()
    {
        newInteractDelay += Time.deltaTime;

        if (newInteractDelay >= newInteractWait)
        {
            if (targetedBy.CheckIfStopped(1.5f) == true)
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

        switch (state)
        {
            case ResourceStates.SEED:

                if (workTime >= workDelay)
                {
                    tendVal += plantData.tendRate;

                    if (tendVal >= tendMax)
                    {
                        needsTending = false;
                        tendTime = 0;
                    }

                    workTime = 0;
                }

                break;
            case ResourceStates.SOURCE:

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

        if(state == ResourceStates.SEED)
        {
            task = new TaskTend(character, gameObject);
        }
        else
        {
            task = new TaskDestroy(character, gameObject);
        }

        tasks.Add(task);

        return tasks;
    }
    public override Task Task(CharacterControl character, bool playerOverride)
    {
        if (targetedBy != null)
        {
            return null;
        }

        if(needsTending == false)
        {
            return null;
        }

        Task task = null;
        targetedBy = character;

        if (state == ResourceStates.SEED)
        {
            task = new TaskTend(character, gameObject);
        }
        else
        {
            task = new TaskDestroy(character, gameObject);
        }

        return task;
    }
    protected override void IsFarmed()
    {
        targetedBy.interactingObject = null;
        targetedBy = null;
        SpawnMaterial(plantData);
    }
    protected override void SpawnMaterial(ResourceData obj)
    {
        GameObject temp = Instantiate(obj.materialModel);
        temp.transform.name = obj.resourceName;
        temp.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(this.transform.position));
        temp.transform.SetParent(materialsObject.transform, false);

        MaterialControl control = temp.AddComponent<MaterialControl>();
        GameHandler.AddMaterial_Static(control);
        control.Init(obj, worldGrid);

        GameHandler.RemovePlant_Static(this);

        var guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        guo.addPenalty = 0;
        guo.modifyTag = true;
        guo.setTag = 0;
        AstarPath.active.UpdateGraphs(guo);

        Destroy(this.gameObject);
    }
}
