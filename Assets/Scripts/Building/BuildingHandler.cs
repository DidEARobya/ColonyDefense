using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler instance;
    private new Camera camera;

    public GameObject cursor;
    private WorldGrid worldGrid;

    private GameObject structure;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;

    private LayerMask obstacleMask;
    private LayerMask buildableArea;
    public bool isPlaceable;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            camera = Camera.main;
            worldGrid = GameHandler.GetWorldGrid_Static();

            obstacleMask = LayerMask.GetMask("Obstacle");
            buildableArea = LayerMask.GetMask("Buildable");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(structure == null)
        {
            return;
        }

        structure.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(camera.ScreenToWorldPoint(Input.mousePosition)));
        CheckIfPlaceable();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceStructure();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelStructure();
        }
    }

    private void SetStructure(GameObject obj)
    {
        structure = obj;
        structure.transform.parent = this.transform;

        collider = structure.GetComponent<BoxCollider2D>();
        renderer = structure.GetComponent<SpriteRenderer>();

        renderer.color = new Color(1f, 1f, 1f, 0.75f);
        renderer.sortingLayerName = "Building";
    }
    public static void SetStructure_Static(GameObject obj)
    {
        instance.SetStructure(obj);
    }
    private GameObject GetStructure()
    {
        return structure;
    }
    public static GameObject GetStructure_Static()
    {
        return instance.GetStructure();
    }
    private void CancelStructure()
    {
        Destroy(structure);
        structure = null;
    }
    public static void CancelStructure_Static()
    {
        instance.CancelStructure();
    }
    private void PlaceStructure()
    {
        if(isPlaceable == false)
        {
            return;
        }

        structure.transform.position = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(camera.ScreenToWorldPoint(Input.mousePosition)));
        renderer.color = new Color(1f, 1f, 1f, 1f);
        structure.layer = LayerMask.NameToLayer("Obstacle");
        renderer.sortingLayerName = "Default";

        structure.AddComponent<StructureControl>();
        structure.GetComponent<StructureControl>().Init(structure.GetComponent<StructureBase>());
        structure.GetComponent<StructureBase>().IsReady();

        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            var node = AstarPath.active.GetNearest(structure.transform.position).node;
            node.Penalty = (uint)structure.GetComponent<StructureBase>().durability * 10;
            node.Tag = 2;

            structure = null;
        }));
    }
    private void CheckIfPlaceable()
    {
        if (collider.IsTouchingLayers(obstacleMask) == true || collider.IsTouchingLayers(buildableArea) != true)
        {
            isPlaceable = false;
            renderer.color = new Color(1f, 0f, 0f, 0.75f);
        }
        else
        {
            isPlaceable = true;
            renderer.color = new Color(1f, 1f, 1f, 0.75f);
        }
    }
}

