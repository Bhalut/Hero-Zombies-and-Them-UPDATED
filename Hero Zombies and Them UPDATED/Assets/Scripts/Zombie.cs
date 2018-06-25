using UnityEngine;
using NPC.Ally;

namespace NPC                                                                   //Library containing two libraries under "Ally" and "Enemy".
{
    namespace Enemy                                                             //Librería Enemy, belongs to the NPC library, which contains the Zombie's class.
    {
        /// <summary>
        /// Zombie.
        /// This class contains the zombie's information, such as his taste (what he wants to eat),
        /// and if they touch Citizen they turn it into a Zombie.
        /// </summary>
        public class Zombie : Npc
        {
            public ZombieData _zombieData;                                      //Variable containing the struct of the zombie.
            GameManager _gameManager;                                           //Variable containing the class GameManager.

            /// <summary>
            /// Start this instance.
            /// Defines initialized variables
            /// randomly assigns the zombie's taste, 
            /// freezes the zombie's Rigidbody rotation.
            /// </summary>
            void Start()
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();                      //start the variable "_gameManager", with the "GameManager" component.
                _zombieData.age = age;
                _zombieData._taste = (Taste)Random.Range(0, 5);
                _zombieData.color = new Color[] { Color.cyan, Color.green, Color.magenta };                     //An array is created, to save the zombie's colors, which will be saved in the color value struct.
                gameObject.GetComponent<Renderer>().material.color = _zombieData.color[Random.Range(0, 3)];
                gameObject.GetComponent<Rigidbody>().freezeRotation = enabled;
            }

            /// <summary>
            /// Update this instance.
            /// Verify states
            /// </summary>
            void Update()
            {
                Movement(_state);
                if (!_gameManager.isDead) base.Respond();
            }

            public ZombieData ZombieID()                                        //Method that returns the structure containing the citizen's information
            {
                return _zombieData;
            }

            /// <summary>
            /// Ons the collision enter.
            /// Convert to Zombie.
            /// </summary>
            /// <param name="collision">Collision.</param>
            void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.GetComponent<Citizen>())
                {
                    Citizen citizen = collision.gameObject.GetComponent<Citizen>();
                    Zombie zombie = citizen;
                    _gameManager.citizenCount--;
                    _gameManager.citizenText.text = "Citizen: " + _gameManager.citizenCount.ToString();
                    _gameManager.zombieCount++;
                    _gameManager.zombieText.text = "Zombie: " + _gameManager.zombieCount.ToString();
                }   
            }
        }
    }
}

public enum Taste { arm, nose, ear, finger, leg }                               //Enum which contains the tastes to be randomly assigned.

public struct ZombieData                                                        //Struct containing the citizen's information.
{
    public State _state;
    public Taste _taste;
    public Color[] color;
    public int age;
}