using UnityEngine;
using NPC.Enemy;

/// <summary>
/// Hero.
/// In this class is stored the information of the Hero, such as when he touches a zombie or a citizen, 
/// such as making him move through a class of FpsMove(for movement), 
/// how to add the camera and add the class of FpsCamera(for camera viewing), 
/// to him as to the camera, and add a "rigidbody".
/// </summary>
public class Hero : MonoBehaviour 
{   
    GameManager _gameManager;                                                   //Variable containing the class GameManager.

	void Start () 
    {
        gameObject.AddComponent<FpsCamera>();                                               //To this Object add the scripts "FpsCamera".
        gameObject.AddComponent<FpsMove>().speed += new MoveSpeed().speed;                  //To this Object add the scripts "FpsMove", and change the variable "speed" by a random value of other class.
        gameObject.AddComponent<Rigidbody>().freezeRotation = enabled;                      //To this Object add component "Rigidvody" and freeze rotation.
        Camera.main.gameObject.transform.localPosition = gameObject.transform.position;     //The position of the Main Camera will be that of this object.
        Camera.main.transform.SetParent(gameObject.transform);                              //To make the camera a child of this object.
        Camera.main.gameObject.AddComponent<FpsCamera>();                                   //To the camera add the scripts "FpsCamera".
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();          //start the variable "_gameManager", with the "GameManager" component.
    }

    /// <summary>
    /// Ons the collision enter.
    /// Validate if you were touched by a Zombie and happen to be dead
    /// </summary>
    /// <param name="collision">Collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Zombie>())
        {
            _gameManager.Dead();
        }
    }
}

public class MoveSpeed                                                          //Class MoveSpeed, that contains the speed of the hero.
{
    public readonly float speed;

    public MoveSpeed()                                                          //Constructor to randomly assign the speed of movement of the hero by means of a readonly floating variable.
    {
        speed = Random.Range(0.2f, 0.5f);
    }
}