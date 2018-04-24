using UnityEngine.UI;
using UnityEngine;
using NPC.Ally;
using NPC.Enemy;
using System.Collections;

/// <summary>
/// Hero.
/// In this class is stored the information of the Hero, such as when he touches a zombie or a citizen, 
/// such as making him move through a class of FpsMove(for movement), 
/// how to add the camera and add the class of FpsCamera(for camera viewing), 
/// to him as to the camera, and add a "rigidbody".
/// </summary>
public class Hero : MonoBehaviour 
{   
    CitizenData _citizenData;                                                               //variable containing the struct of the citizen.
    ZombieData _zombieData;                                                                 //variable containing the struct of the zombie.
    GameObject panelUI;
    Text textDialogue;
    Text textTypeName;


	void Start () 
    {
        gameObject.AddComponent<FpsCamera>();                                               //To this Object add the scripts "FpsCamera".
        gameObject.AddComponent<FpsMove>().speed += new MoveSpeed().speed;                  //To this Object add the scripts "FpsMove", and change the variable "speed" by a random value of other class.
        gameObject.AddComponent<Rigidbody>().freezeRotation = enabled;                      //To this Object add component "Rigidvody" and freeze rotation.
        Camera.main.gameObject.transform.localPosition = gameObject.transform.position;     //The position of the Main Camera will be that of this object.
        Camera.main.transform.SetParent(gameObject.transform);                              //To make the camera a child of this object.
        Camera.main.gameObject.AddComponent<FpsCamera>();                                   //To the camera add the scripts "FpsCamera".

        textDialogue = GameObject.Find("Dialogue").GetComponent<Text>();
        textTypeName = GameObject.Find("TypeName").GetComponent<Text>();
        panelUI = GameObject.Find("PanelDialogue");
        panelUI.SetActive(false);
    }

    /// <summary>
    /// Ons the collision enter.
    /// This method, compares whether the object collided with another object,
    /// To send a message if I collide with a zombie or a citizen.
    /// </summary>
    /// <param name="collision">Collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Citizen>())                                                       //Condition (If I collide with an object with the component "Citizen").
        {
            _citizenData = collision.gameObject.GetComponent<Citizen>().CitizenID();                            //Assigns the citizen's information to use in the message.
            panelUI.SetActive(true);
            textTypeName.text = ("Citizen");
            textDialogue.text = ("Hello I am " + _citizenData.name + " and I have " + _citizenData.age + " years old");    //Message given by the citizen when making contact.
        }

        if (collision.gameObject.GetComponent<Zombie>())                                    //condition (If I collide with an object with the component "Zombie").
        {
            _zombieData = collision.gameObject.GetComponent<Zombie>().ZombieID();           //Assigns the zombie information to use in the message.
            panelUI.SetActive(true);
            textTypeName.text = ("Zombie");
            textDialogue.text = ("Waaaaarrrr I want to eat " + _zombieData._taste);                    //Message given by the zombie when he comes into contact.
        }
    }

    void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.GetComponent<Citizen>())
        {
            StartCoroutine(EnablePanel());
        }
        if (collision.gameObject.GetComponent<Zombie>())
        {
            StartCoroutine(EnablePanel());
        }

	}

    IEnumerator EnablePanel()
    {
        yield return new WaitForSeconds(2.5f);
        panelUI.SetActive(false);
    }
}

public class MoveSpeed                                                                      //Class MoveSpeed, that contains the speed of the hero.
{
    public readonly float speed;

    public MoveSpeed()                                                                       //Constructor to randomly assign the speed of movement of the hero by means of a readonly floating variable.
    {
        speed = Random.Range(0.2f, 0.5f);
    }
}
