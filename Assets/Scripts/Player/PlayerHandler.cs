using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;

    public Canvas listCanvas;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI toggleText;

    public GameObject listPanel;
    public GameObject itemDisplayPrefab;

    public GameObject structuresPanel;

    private Dictionary<Sprite, int> storedDict;

    private List<Sprite> materialSprites;
    private List<int> amounts;

    private List<GameObject> displayListItems;

    private int money;
    private bool isListActive;
    private bool isStructuresActive;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            storedDict = new Dictionary<Sprite, int>();
            materialSprites = new List<Sprite>();
            amounts = new List<int>();
            displayListItems = new List<GameObject>();

            money = 0;

            isStructuresActive = false;
            structuresPanel.SetActive(isStructuresActive);
            isListActive = false;
            listCanvas.gameObject.SetActive(isListActive);

            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + money.ToString();

        if(isListActive == false)
        {
            return;
        }

        if (amounts.Count != storedDict.Count)
        {
            Display();
        }
    }
    public void ToggleStructures()
    {
        isStructuresActive = !isStructuresActive;
        structuresPanel.SetActive(isStructuresActive);
    }
    public void ToggleList()
    {
        isListActive = !isListActive;
        SetListActive();
    }
    private void SetListActive()
    {
        listCanvas.gameObject.SetActive(isListActive);   
    }

    private void Display()
    {
        for (int i = 0; i < displayListItems.Count; i++)
        {
            Destroy(displayListItems[i].gameObject);
        }

        displayListItems.Clear();

        if (storedDict.Count == 0)
        {
            return;
        }

        materialSprites = storedDict.Keys.ToList();
        amounts = storedDict.Values.ToList();


        for (int i = 0; i < amounts.Count; i++)
        {
            GameObject obj = Instantiate(itemDisplayPrefab);

            obj.transform.Find("ItemSprite").GetComponent<UnityEngine.UI.Image>().sprite = materialSprites[i];
            obj.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text = amounts[i].ToString();

            obj.transform.SetParent(listPanel.transform, false);
            displayListItems.Add(obj);
        }
    }

    private void StoreMaterial(Sprite material, int amount)
    {
        if (storedDict.ContainsKey(material))
        {
            storedDict[material] += amount;
            Display();
        }
        else
        {
            storedDict.Add(material, amount);
        }
    }

    public static void StoreMaterial_Static(Sprite material, int amount)
    {
        instance.StoreMaterial(material, amount);
    }
    private void RemoveMaterial(Sprite material, int amount)
    {
        if (storedDict.ContainsKey(material))
        {
            storedDict[material] -= amount;

            if (storedDict[material] <= 0)
            {
                storedDict.Remove(material);
            }

            Display();
        }
    }

    public static void RemoveMaterial_Static(Sprite material, int amount)
    {
        instance.RemoveMaterial(material, amount);
    }

    private void AddMoney(int  amount)
    {
        money += amount;
    }

    public static void AddMoney_Static(int amount)
    {
        instance.AddMoney(amount);
    }
    private void RemoveMoney(int amount)
    {
        money -= amount;
    }
    public static void RemoveMoney_Static(int amount)
    {
        instance.RemoveMoney(amount);
    }
}
