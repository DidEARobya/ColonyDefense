using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructureUIObject : MonoBehaviour, IButtonClickHandler
{
    private new Camera camera;
    public GameObject materialsMenu;

    public string structureType;
    public StructureBase structure;
    public StructureMaterials material;
    public StructureMaterials displayMaterial;

    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI durabilityText;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        material = StructureMaterials.WOODEN;

        image.sprite = structure.woodSprite;
        nameText.text = "Wood " + structureType;
        durabilityText.text = "HP: " + structure.woodDurability.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(material == displayMaterial)
        {
            return;
        }

        SwitchMaterial();
    }
    public void OnButtonLeftClick()
    {
        if (BuildingHandler.GetStructure_Static() != null)
        {
            BuildingHandler.CancelStructure_Static();
        }

        switch (structureType)
        {
            case "Wall":
                WallScript script = Instantiate((WallScript)structure);

                script.Init(material);
                BuildingHandler.SetStructure_Static(script.gameObject);
                break;
        }
    }
    public void OnButtonRightClick()
    {
        if (BuildingHandler.GetStructure_Static() != null)
        {
            BuildingHandler.CancelStructure_Static();
        }

        DisplayMaterialMenu();
    }
    private void SwitchMaterial()
    {
        displayMaterial = material;

        switch (displayMaterial)
        {
            case StructureMaterials.WOODEN:
                image.sprite = structure.woodSprite;
                nameText.text = "Wood " + structureType;
                durabilityText.text = "HP: " + structure.woodDurability.ToString();
                break;
            case StructureMaterials.STONE:
                image.sprite = structure.stoneSprite;
                nameText.text = "Stone " + structureType;
                durabilityText.text = "HP: " + structure.stoneDurability.ToString();
                break;
            case StructureMaterials.STEEL:
                image.sprite = structure.steelSprite;
                nameText.text = "Steel " + structureType;
                durabilityText.text = "HP: " + structure.steelDurability.ToString();
                break;
        }
    }
    private void DisplayMaterialMenu()
    {
        materialsMenu.SetActive(true);
    }
}
