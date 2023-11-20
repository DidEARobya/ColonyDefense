using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallScript : StructureBase
{
    private void Awake()
    {
        woodDurability = 300;
        stoneDurability = 400;
        steelDurability = 500;
    }
    public override void Init(StructureMaterials material)
    {
        InitComponents();

        switch(material)
        {
            case StructureMaterials.WOODEN:
                renderer.sprite = woodSprite;
                structureName = "Wood Wall";
                durability = woodDurability;
                break;
            case StructureMaterials.STONE:
                renderer.sprite = stoneSprite;
                structureName = "Stone Wall";
                durability = stoneDurability;
                break;
            case StructureMaterials.STEEL:
                renderer.sprite = steelSprite;
                structureName = "Steel Wall";
                durability = steelDurability;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isReady == false)
        {
            return;
        }
    }
}
