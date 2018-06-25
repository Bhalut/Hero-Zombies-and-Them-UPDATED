using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NPC.Ally;
using NPC.Enemy;

/// <summary>
/// Game manager.
/// This class is in charge of making instance of cubes in the scene, 
/// and each cube is given a citizen or zombie component, 
/// and only one of the cubes will get the Hero component, 
/// each zombie gets a random color and a taste, 
/// show messages of the Zombie or Citizen if it is in the distance range with the Hero,
/// and check if the Hero is dead to finish the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static List<GameObject> npc = new List<GameObject>();                //List where the existing NPCs in the scene are added.
    public Text citizenText;                                                    //Text that indicates the number of citizens in the scene.
    public Text zombieText;                                                     //Text that indicates the number of zombies in the scene.
    const int max = 25;                                                         //Constant variable that has the max number of cubes to create.
    public int citizenCount;                                                    //Counter for Citizens.
    public int zombieCount;                                                     //Counter for Zombies.
    public bool isDead;                                                         //whether or not he is dead.
    const float closeDistance = 5.0F;                                           //Minimal distance.
    CitizenData _citizenData;                                                   //variable containing the struct of the citizen.
    ZombieData _zombieData;                                                     //variable containing the struct of the zombie.
    GameObject buttonReturn;
    GameObject panelUI;                                                         //Dialog panel.
    Image panelDead;                                                            //Dead panel.
    Text textDialogue;                                                          //Dialog text.
    Text textTypeName;                                                          //Citizen or Zombie name text.
    Text textYouDied;                                                           //Game Over text.
    Hero hero;                                                                  //Variable containing the class Hero.

    /// <summary>
    /// Start this instance.
    /// Defines initialized variables
    /// </summary>
    void Start()
    {
        citizenText = GameObject.Find("TextCitizen").GetComponent<Text>();
        zombieText = GameObject.Find("TextZombie").GetComponent<Text>();
        textDialogue = GameObject.Find("Dialogue").GetComponent<Text>();
        textTypeName = GameObject.Find("TypeName").GetComponent<Text>();
        textYouDied = GameObject.Find("YouDied").GetComponent<Text>();
        panelDead = GameObject.Find("PanelDead").GetComponent<Image>();
        panelUI = GameObject.Find("PanelDialogueObject");
        buttonReturn = GameObject.Find("Return");
        textYouDied.enabled = false;
        panelDead.enabled = false;
        panelUI.SetActive(false);
        buttonReturn.SetActive(false);
        int spawn = 0;                                                                      //variable to assign components to each cube, initialized at 0.

        for (int i = 0; i < Random.Range(new MinValue().minValue, max); i++)                //we use a "for" cycle, to create the cubes and add each component, this cycle will start from 0 to a random one from between (a random min of(5, 15), up to a Max of 25).                
        {
            GameObject character = GameObject.CreatePrimitive(PrimitiveType.Cube);          //we initialize a variable of the "GameObject" type, in which we save the creation of the cubes.
            Vector3 pos = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));     //we initialize a variable of type "Vector3", in which we will place a random position for each cube, (so they will have different coordinates).
            character.transform.position = pos;                                             //The "character" position shall be equal to "pos".

            switch (spawn)                                                                  //We create a "switch" that depends on the "spawn" variable, in order to pass to each case and add the components.
            {
                case 0:                                                                     //Case number 0, contains the components of the Hero. (Like object name, tag, color and script).
                    character.name = "Hero";
                    character.gameObject.tag = "Player";
                    character.AddComponent<Hero>();
                    character.GetComponent<Renderer>().material.color = Color.blue;
                    hero = GameObject.Find("Hero").GetComponent<Hero>();
                    break;
                case 1:                                                                     //Case number 1, contains the Citizen's components. (Like the name of the object, and the script).
                    character.name = "Citizen";
                    character.AddComponent<Citizen>();
                    break;
                case 2:                                                                     //Case number 2, contains the components of the Zombie. (Like the name of the object, the color (which is randomly assigned by the Array) and the script).
                    character.name = "Zombie";
                    character.AddComponent<Zombie>();
                    break;
                default:                                                                    //And a Default case which will have the Citizen component.
                    goto case 2;
            }
            spawn = Random.Range(1, 3);                                                     //"Spawn" will be random between (1, 3), so it will grab the other components and only once will it grab Hero.
            if (character != null) npc.Add(character);                                      //Condition to add npc to the list and Add npc to the list.                                            
        }

        foreach (GameObject go in npc)                                          //For each item in the list, the citizen or zombie counter increases.
        {
            if (go != null)
            {
                if (go.name == "Citizen")
                {
                    citizenCount++;
                    citizenText.text = "Citizen: " + citizenCount.ToString();
                }
                else if (go.name == "Zombie")
                {
                    zombieCount++;
                    zombieText.text = "Zombie: " + zombieCount.ToString();
                }
            }
        }
    }

    /// <summary>
    /// Update this instance.
    /// Check if the "Hero" is near an NPC.
    /// Check if the "Hero" is dead.
    /// </summary>
    void Update()
    {
        Message();

        if (citizenCount <= 0) Dead();
    }

    /// <summary>
    /// Message this instance.
    /// Check if the "Hero" is near an NPC.
    /// </summary>
    void Message()
    {
        if (!isDead)
        {
            foreach (GameObject other in npc)
            {
                if (other)
                {
                    Vector3 offset = other.transform.position - hero.transform.position;
                    float sqrLen = offset.sqrMagnitude;
                    if (sqrLen < closeDistance * closeDistance)
                    {
                        if (other.GetComponent<Citizen>())                                                       //Condition (If I collide with an object with the component "Citizen").
                        {
                            _citizenData = other.GetComponent<Citizen>().CitizenID();                            //Assigns the citizen's information to use in the message.
                            panelUI.SetActive(true);
                            textTypeName.text = ("Citizen");
                            textDialogue.text = ("Hello I am " + _citizenData.name + " and I have " + _citizenData.age + " years old");    //Message given by the citizen when making contact.
                            StartCoroutine(EnablePanel());
                        }

                        if (other.GetComponent<Zombie>())                                                       //condition (If I collide with an object with the component "Zombie").
                        {
                            _zombieData = other.GetComponent<Zombie>().ZombieID();                              //Assigns the zombie information to use in the message.
                            panelUI.SetActive(true);
                            textTypeName.text = ("Zombie");
                            textDialogue.text = ("Waaaaarrrr I want to eat " + _zombieData._taste);             //Message given by the zombie when he comes into contact.
                            StartCoroutine(EnablePanel());
                        }
                    }
                }
            }
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("scene");
    }

    /// <summary>
    /// Dead this instance.
    /// Check if the "Hero" is dead.
    /// </summary>
    public void Dead()
    {
        hero.GetComponent<FpsMove>().enabled = false;
        isDead = true;
        panelDead.enabled = true;
        StartCoroutine(Fade());
    }

    /// <summary>
    /// Fade this instance.
    /// </summary>
    /// <returns>The fade.</returns>
    IEnumerator Fade()
    {
        for (float f = 0f; f <= 0.5f; f += .15f)
        {
            panelDead.color = new Color((169f/255), (169f/255), (169f/255), (f));
            yield return null;
        }
        textYouDied.enabled = true;
        textYouDied.text = "YOU DIED";
        StartCoroutine(Size());
    }

    /// <summary>
    /// Size this instance.
    /// </summary>
    /// <returns>The size.</returns>
    IEnumerator Size()
    {
        for (int f = 14; f <= 50; f++)
        {
            textYouDied.fontSize = f;
            yield return null;
        }
        buttonReturn.SetActive(true);
    }

    /// <summary>
    /// Enables the panel.
    /// </summary>
    /// <returns>The panel.</returns>
    IEnumerator EnablePanel()
    {
        yield return new WaitForSeconds(2.5f);
        panelUI.SetActive(false);
    }
}

public class MinValue                                                           //Class MinValue, containing the minimum character creation value.
{
    public readonly int minValue;

    public MinValue()                                                           //Constructor to obtain the minimum character creation value, by means of an integer readonly variable.
    {
        minValue = Random.Range(5, 15);
    }
}