using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MaterialsDropdown : MonoBehaviour
{
    public GameObject menu;

    public StructureUIObject uiObject;

    public Button wood;
    public Button stone;
    public Button steel;

    // Start is called before the first frame update
    void Start()
    {
        List<Button> buttons = new List<Button>();

        buttons.Add(wood);
        buttons.Add(stone);
        buttons.Add(steel);

        for(int i = 0; i < buttons.Count; i++)
        {
            StructureMaterials mat = new StructureMaterials();
            mat = (StructureMaterials)i;

            buttons[i].onClick.AddListener(() => { SetMaterial(mat); });
        }

        menu.SetActive(false);
    }
    public void SetMaterial(StructureMaterials material)
    {
        uiObject.material = material;
        menu.SetActive(false);
    }
}
