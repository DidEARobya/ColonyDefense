using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewPlant")]
public class PlantData : ResourceData
{
    public Sprite plantSprite;

    public int growthRate;
    public int growthDelay;

    public int tendCooldown;
    public int tendRate;
}
