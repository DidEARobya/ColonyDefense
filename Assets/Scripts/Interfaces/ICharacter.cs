using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void Init(CharacterObject character, Vector2 spawnpos);
    public void MoveCharacterTo(Vector2 destination);
    public bool CheckIfStopped(float remainingDistance);
}
