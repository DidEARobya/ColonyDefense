using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    CharacterControl GetCharacterControl();
    void Interact(CharacterControl character);
    Task Task(CharacterControl character);
    void Cancel(GameObject newObj);
}

