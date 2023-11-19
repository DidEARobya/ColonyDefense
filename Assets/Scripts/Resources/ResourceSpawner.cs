using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject worldBase;
    private WorldGrid worldGrid;

    public List<ResourceData> resources;
    public List<int> resourceSpawnChance;

    private ResourceData toSpawn;
    private Vector2 spawnPosition;

    public float maxSpawnDelay;
    private float spawnDelay;
    private float spawnTimer;

    private float boundsX;
    private float boundsY;

    public float spawnLimit;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector2.zero;

        worldGrid = GameHandler.GetWorldGrid_Static();
        spawnPosition = Vector2.zero;
        spawnDelay = Random.Range(0, maxSpawnDelay);
        spawnTimer = 0;

        boundsX = (worldBase.transform.localScale.x / 2) - 5f;
        boundsY = (worldBase.transform.localScale.y / 2) - 5f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            if(GameHandler.GetResourceCount_Static() < spawnLimit)
            {
                SpawnResource();
            }
        }
    }

    private void SpawnResource()
    {
        GeneratePos();

        if (spawnPosition == Vector2.zero)
        {
            return;
        }

        GenerateToSpawn();

        if (toSpawn != null)
        {
            Spawn();
        }

    }
    private void Spawn()
    {
        GameObject obj = Instantiate(toSpawn.sourceModel);
        obj.transform.name = toSpawn.resourceName + " Source";

        Vector2 worldToCell = worldGrid.GetWorldToCell(spawnPosition);
        Vector2 cellCentre = worldGrid.GetCellCentre(worldToCell);

        obj.transform.position = cellCentre;

        obj.transform.SetParent(this.transform, false);

        Bounds bounds = new Bounds();
        Vector2 bSize = new Vector2(3, 3);
        bounds.size = bSize;
        bounds.center = obj.transform.position;

        var guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        guo.addPenalty = 10000;
        guo.modifyTag = true;
        guo.setTag = 1;
        AstarPath.active.UpdateGraphs(guo);

        if (toSpawn.resourceType == ResourceType.ORE)
        {
            ResourceControl temp = obj.AddComponent<ResourceControl>();
            GameHandler.AddOre_Static(temp);

            temp.Init(toSpawn, bounds);
        }
        else if (toSpawn.resourceType == ResourceType.PLANT)
        {
            PlantControl temp = obj.AddComponent<PlantControl>();
            GameHandler.AddSeed_Static(temp);

            temp.Init(toSpawn, bounds);
        }

        spawnDelay = Random.Range(0, maxSpawnDelay);
        spawnTimer = 0;
    }
    private void GeneratePos()
    {
        float spawnY = Random.Range(-boundsY, boundsY);
        float spawnX = Random.Range(-boundsX, boundsX);

        spawnPosition = new Vector2(spawnX, spawnY);

        CheckForObjects(spawnPosition);
    }

    private void GenerateToSpawn()
    {
        int random = Random.Range(0, 100);

        int lowRange;
        int highRange = 0;

        for (int i = 0; i < resources.Count; i++)
        {
            lowRange = highRange;
            highRange += resourceSpawnChance[i];

            if (random >= lowRange && random <= highRange)
            {
                toSpawn = resources[i];
                return;
            }
        }
    }

    private void CheckForObjects(Vector2 position)
    {
        LayerMask mask = LayerMask.GetMask("Obstacle");
        mask += LayerMask.GetMask("Checkpoint");

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, 1.75f, mask);

        if (hits.Length != 0)
        {
            spawnPosition = Vector2.zero;
        }
    }
}
