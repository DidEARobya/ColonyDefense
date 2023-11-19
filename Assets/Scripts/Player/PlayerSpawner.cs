using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public CharacterObject testCharacter, testCharacter2, testCharacter3, testCharacter4;

    private List<CharacterObject> toSpawn;

    private float delay, spawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = 2.0f;
        delay = 2.0f;

        toSpawn = new List<CharacterObject>();
        toSpawn.Add(testCharacter);
        toSpawn.Add(testCharacter2);
        toSpawn.Add(testCharacter3);
        toSpawn.Add(testCharacter4);
    }

    // Update is called once per frame
    void Update()
    {
        delay += Time.deltaTime;

        if (delay >= spawnDelay)
        {
            if (toSpawn.Count > 0)
            {
                SpawnCharacter(toSpawn[0]);
                toSpawn.RemoveAt(0);
                delay = 0.0f;
            }
        }
    }

    public void AddCharacter(CharacterObject character)
    {
        toSpawn.Add(character);
    }
    private void SpawnCharacter(CharacterObject character)
    {
        character.Init();
        GameObject temp = Instantiate(character.LoadCharacter());
        temp.transform.SetParent(this.transform, false);

        CharacterControl control = temp.AddComponent<CharacterControl>();
        control.Init(character, this.transform.position);
    }
}
