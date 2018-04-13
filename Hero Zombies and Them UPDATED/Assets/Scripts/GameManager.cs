using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Ally;
using NPC.Enemy;

/// <summary>
/// Game manager.
/// This class is in charge of making instance of cubes in the scene, 
/// and each cube is given a citizen or zombie component, 
/// and only one of the cubes will get the Hero component, 
/// each zombie gets a random color and a taste, 
/// and each citizen gets a name and an age. 
/// </summary>
public class GameManager : MonoBehaviour 
{
    ZombieData _zombieData;                                                             //variable containing the struct of the zombie.
    List<GameObject> npc = new List<GameObject>();                                      //List where the existing NPCs in the scene are added.
    [SerializeField]
    Text citizenText;                                                                   //Text that indicates the number of citizens in the scene.
    [SerializeField]
    Text zombieText;                                                                    //Text that indicates the number of zombies in the scene.
    const int max = 25;                                                                 //Constant variable that has the max number of cubes to create.
    int citizenCount;                                                                   //Counter for Citizens.
    int zombieCount;                                                                    //Counter for Zombies.

	void Start () 
    {
        int spawn = 0;                                                                 //variable to assign components to each cube, initialized at 0.
        _zombieData.color = new Color[] { Color.cyan, Color.green, Color.magenta };    //An array is created, to save the zombie's colors, which will be saved in the color value struct.
        for (int i = 0; i < Random.Range(new MinValue().minValue, max); i++)           //we use a "for" cycle, to create the cubes and add each component, this cycle will start from 0 to a random one from between (a random min of(5, 15), up to a Max of 25).                
        {
            GameObject character = GameObject.CreatePrimitive(PrimitiveType.Cube);     //we initialize a variable of the "GameObject" type, in which we save the creation of the cubes.
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));//we initialize a variable of type "Vector3", in which we will place a random position for each cube, (so they will have different coordinates).
            character.transform.position = pos;                                        //The "character" position shall be equal to "pos".

            switch (spawn)                                                             //We create a "switch" that depends on the "spawn" variable, in order to pass to each case and add the components.
            {
                case 0:                                                                //Case number 0, contains the components of the Hero. (Like object name, tag, color and script).
                    character.name = "Hero";
                    character.gameObject.tag = "Player";
                    character.AddComponent<Hero>();
                    character.GetComponent<Renderer>().material.color = Color.blue;
                    break;
                case 1:                                                                //Case number 1, contains the Citizen's components. (Like the name of the object, and the script).
                    character.name = "Citizen";
                    character.AddComponent<Citizen>();
                    break;
                case 2:                                                                //Case number 2, contains the components of the Zombie. (Like the name of the object, the color (which is randomly assigned by the Array) and the script).
                    character.name = "Zombie";
                    character.AddComponent<Zombie>();
                    character.GetComponent<Renderer>().material.color = _zombieData.color[Random.Range(0, 3)];
                    break;
                default:                                                               //And a Default case which will have the Citizen component.
                    character.name = "Citizen";
                    character.AddComponent<Citizen>();
                    break;
            }
            spawn = Random.Range(1, 3);                                                //"Spawn" will be random between (1, 3), so it will grab the other components and only once will it grab Hero.
            if (character.gameObject.tag != "Player")                                  //Condition to add npc to the list.
                npc.Add(character);                                                    //Add npc to the list.
        }
        foreach (GameObject go in npc)                                                 //For each item in the list, the citizen or zombie counter increases.
        {
            if (go.name == "Citizen")
            {
                citizenCount++;
                citizenText.text = "Citizen: " + citizenCount.ToString();
            }else if (go.name == "Zombie")
            {
                zombieCount++;
                zombieText.text = "Zombie: " + zombieCount.ToString();
            }
        }
	}
}

public class MinValue                                                                  //Class MinValue, containing the minimum character creation value.
{
    public readonly int minValue;

    public MinValue()                                                                  //Constructor to obtain the minimum character creation value, by means of an integer readonly variable.
    {
        minValue = Random.Range(5, 15);
    }
}
