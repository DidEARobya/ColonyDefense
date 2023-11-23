using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    CharacterControl GetCharacterControl();
    void Interact(CharacterControl character);
    List<Task> Task(CharacterControl character);

    Task Task(CharacterControl character, bool playerOverride);
    void Cancel(GameObject newObj);
}

