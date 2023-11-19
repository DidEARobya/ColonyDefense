using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Type
{
    RESOURCE,
    MATERIAL
}
public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    private WorldGrid worldGrid;

    private List<ResourceControl> worldOres;
    private List<PlantControl> worldSeeds;
    private List<PlantControl> worldPlants;
    private List<MaterialControl> worldMaterials;
    //private List<CharacterControl> worldEnemies;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            worldOres = new List<ResourceControl>();
            worldSeeds = new List<PlantControl>();
            worldPlants = new List<PlantControl>();
            worldMaterials = new List<MaterialControl>();

            worldGrid = new WorldGrid(80, 60, 1, this.transform.position.x, this.transform.position.y);
            instance = this;
        }
    }
    private WorldGrid GetWorldGrid()
    {
        return worldGrid;
    }
    public static WorldGrid GetWorldGrid_Static()
    {
        return instance.GetWorldGrid();
    }
    private ResourceControl GetClosestAvailableOre(CharacterControl character)
    {
        if(worldOres.Count == 0)
        {
            return null;
        }

        ResourceControl nearestResource = null;

        for (int i = 0; i < worldOres.Count; i++)
        {
            if (nearestResource == null)
            {
                if (worldOres[i].isTargeted == false)
                {
                    nearestResource = worldOres[i];
                }
            }
            else
            {
                if (worldOres[i].isTargeted == false)
                {
                    float last = Vector2.Distance(character.transform.position, nearestResource.transform.position);
                    float current = Vector2.Distance(character.transform.position, worldOres[i].transform.position);

                    if (current < last)
                    {
                        nearestResource = worldOres[i];
                    }
                }
            }
        }

        if (nearestResource != null && nearestResource.isTargeted == false)
        {
            return nearestResource;
        }

        return null;
    }
    public static ResourceControl GetClosestAvailableOre_Static(CharacterControl character)
    {
        return instance.GetClosestAvailableOre(character);
    }
    private PlantControl GetClosestAvailableSeed(CharacterControl character)
    {
        if (worldSeeds.Count == 0)
        {
            return null;
        }

        PlantControl nearestResource = null;

        for (int i = 0; i < worldSeeds.Count; i++)
        {
            if (nearestResource == null)
            {
                if (worldSeeds[i].isTargeted == false && worldSeeds[i].needsTending == true)
                {
                    nearestResource = worldSeeds[i];
                }
            }
            else
            {
                if (worldSeeds[i].isTargeted == false && worldSeeds[i].needsTending == true)
                {
                    float last = Vector2.Distance(character.transform.position, nearestResource.transform.position);
                    float current = Vector2.Distance(character.transform.position, worldSeeds[i].transform.position);

                    if (current < last)
                    {
                        nearestResource = worldSeeds[i];
                    }
                }
            }
        }

        if (nearestResource != null && nearestResource.isTargeted == false && nearestResource.needsTending == true)
        {
            return nearestResource;
        }

        return null;
    }
    public static PlantControl GetClosestAvailableSeed_Static(CharacterControl character)
    {
        return instance.GetClosestAvailableSeed(character);
    }
    private PlantControl GetClosestAvailablePlant(CharacterControl character)
    {
        if (worldPlants.Count == 0)
        {
            return null;
        }

        PlantControl nearestResource = null;

        for (int i = 0; i < worldPlants.Count; i++)
        {
            if (nearestResource == null)
            {
                if (worldPlants[i].isTargeted == false)
                {
                    nearestResource = worldPlants[i];
                }
            }
            else
            {
                if (worldPlants[i].isTargeted == false)
                {
                    float last = Vector2.Distance(character.transform.position, nearestResource.transform.position);
                    float current = Vector2.Distance(character.transform.position, worldPlants[i].transform.position);

                    if (current < last)
                    {
                        nearestResource = worldPlants[i];
                    }
                }
            }
        }

        if (nearestResource != null && nearestResource.isTargeted == false)
        {
            return nearestResource;
        }

        return null;
    }
    public static PlantControl GetClosestAvailablePlant_Static(CharacterControl character)
    {
        return instance.GetClosestAvailablePlant(character);
    }
    private MaterialControl GetClosestAvailableMaterial (CharacterControl character)
    {
        if (worldMaterials.Count == 0)
        {
           return null;
        }

        MaterialControl nearestMaterial = null;

        for (int i = 0; i < worldMaterials.Count; i++)
        {
            if (nearestMaterial == null)
            {
                if (worldMaterials[i].isTargeted == false)
                {
                    nearestMaterial = worldMaterials[i];
                }
            }
            else
            {
                if (worldMaterials[i].isTargeted == false)
                {
                    float last = Vector2.Distance(character.transform.position, nearestMaterial.transform.position);
                    float current = Vector2.Distance(character.transform.position, worldMaterials[i].transform.position);

                    if (current < last)
                    {
                        nearestMaterial = worldMaterials[i];
                    }
                }
            }
        }

        if (nearestMaterial != null && nearestMaterial.isTargeted == false)
        {
            return nearestMaterial;
        }

        return null;
    }
    public static MaterialControl GetClosestAvailableMaterial_Static(CharacterControl character)
    {
        return instance.GetClosestAvailableMaterial(character);
    }

    private void AddOre(ResourceControl resource)
    {
        if (worldOres.Contains(resource) != true)
        {
            worldOres.Add(resource);
        }
    }
    public static void AddOre_Static(ResourceControl resource)
    {
        instance.AddOre(resource);
    }
    private void AddSeed(PlantControl resource)
    {
        if (worldSeeds.Contains(resource) != true)
        {
            worldSeeds.Add(resource);
        }
    }
    public static void AddSeed_Static(PlantControl resource)
    {
        instance.AddSeed(resource);
    }
    private void AddPlant(PlantControl resource)
    {
        if (worldPlants.Contains(resource) != true)
        {
            worldPlants.Add(resource);
        }
    }
    public static void AddPlant_Static(PlantControl resource)
    {
        instance.AddPlant(resource);
    }

    private void AddMaterial(MaterialControl material)
    {
        if (worldMaterials.Contains(material) != true)
        {
            worldMaterials.Add(material);
        }
    }
    public static void AddMaterial_Static(MaterialControl material)
    {
        instance.AddMaterial(material);
    }

    private void RemoveOre(ResourceControl resource)
    {
        if (worldOres.Contains(resource))
        {
            worldOres.Remove(resource);
        }
    }
    public static void RemoveResource_Static(ResourceControl resource)
    {
        instance.RemoveOre(resource);
    }
    private void RemoveSeed(PlantControl resource)
    {
        if (worldSeeds.Contains(resource))
        {
            worldSeeds.Remove(resource);
        }
    }
    public static void RemoveSeed_Static(PlantControl resource)
    {
        instance.RemoveSeed(resource);
    }
    private void RemovePlant(PlantControl resource)
    {
        if (worldPlants.Contains(resource))
        {
            worldPlants.Remove(resource);
        }
    }
    public static void RemovePlant_Static(PlantControl resource)
    {
        instance.RemovePlant(resource);
    }
    private void RemoveMaterial(MaterialControl material)
    {
        if (worldMaterials.Contains(material))
        {
            worldMaterials.Remove(material);
        }
    }
    public static void RemoveMaterial_Static(MaterialControl material)
    {
        instance.RemoveMaterial(material);
    }

    private List<ResourceControl> GetOres()
    {
        return worldOres;
    }
    public static List<ResourceControl> GetOres_Static()
    {
        return instance.GetOres();
    }
    private List<PlantControl> GetSeeds()
    {
        return worldSeeds;
    }
    public static List<PlantControl> GetSeeds_Static()
    {
        return instance.GetSeeds();
    }
    private List<PlantControl> GetPlants()
    {
        return worldPlants;
    }
    public static List<PlantControl> GetPlants_Static()
    {
        return instance.GetPlants();
    }
    private List<MaterialControl> GetMaterials()
    {
        return worldMaterials;
    }
    public static List<MaterialControl> GetMaterials_Static()
    {
        return instance.GetMaterials();
    }
    private int GetOreCount()
    {
        return worldOres.Count;
    }
    public static int GetOreCount_Static()
    {
        return instance.GetOreCount();
    }
    private int GetSeedCount()
    {
        return worldSeeds.Count;
    }
    public static int GetSeedCount_Static()
    {
        return instance.GetSeedCount();
    }
    private int GetPlantCount()
    {
        return worldPlants.Count;
    }
    public static int GetPlantCount_Static()
    {
        return instance.GetPlantCount();
    }

    private int GetResourceCount()
    {
        return worldOres.Count + worldPlants.Count + worldSeeds.Count;
    }

    public static int GetResourceCount_Static()
    {
        return instance.GetResourceCount();
    }
}
