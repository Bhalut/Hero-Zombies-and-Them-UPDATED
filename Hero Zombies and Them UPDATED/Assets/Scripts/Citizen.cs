using UnityEngine;
using NPC.Enemy;

namespace NPC                                                                   //Library containing two libraries under "Ally" and "Enemy".
{
    namespace Ally                                                              //Librería Ally, belongs to the NPC library, which contains the Citizen's class.
    {
        /// <summary>
        /// Citizen.
        /// This class contains the citizen's information, such as age and name.
        /// </summary>
        public class Citizen : Npc
        {
            CitizenData _citizenData;                                           //variable containing the struct of the citizen.
            GameManager _gameManager;                                           //Variable containing the class GameManager.

            /// <summary>
            /// Start this instance.
            /// Defines initialized variables.
            /// </summary>
            void Start()
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();          //start the variable "_gameManager", with the "GameManager" component.
                _citizenData.age = age;                                                             //It initializes the variable "age" of the struct "CitizenData" by assigning a random value to it.
                _citizenData.name = (CitizenName)Random.Range(0, 20);                               //It initializes the variable "name" of the enum "CitizenName" by assigning a random value to it(casting a "cast").
                gameObject.GetComponent<Rigidbody>().freezeRotation = enabled;                      //To this Object add component "Rigidvody" and freeze rotation.
            }

            public CitizenData CitizenID()                                      //Method that returns the structure containing the citizen's information.
            {
                return _citizenData;
            }

            /// <summary>
            /// Respond this instance.
            /// Method of overwriting so that the citizen goes to the opposite side of the Zombie when it is in the range.
            /// </summary>
            public override void Respond()
            {
                foreach (GameObject go in GameManager.npc)
                {
                    if (go.GetComponent<Zombie>())
                    {
                        float dist = Vector3.Distance(go.transform.position, transform.position);
                        if (dist <= closeDistance) transform.position = Vector3.MoveTowards(transform.position, go.transform.position, -(speed * Time.deltaTime));
                    }
                }
            }

            /// <summary>
            /// Ops the implicit.
            /// Validate the change from Citizen to Zombie
            /// </summary>
            /// <returns>The implicit.</returns>
            /// <param name="citizen">Citizen.</param>
            public static implicit operator Zombie(Citizen citizen)
            {
                Zombie zombie = citizen.gameObject.AddComponent<Zombie>();
                zombie._zombieData.age = citizen._citizenData.age;
                Destroy(citizen);
                return zombie;
            }

            /// <summary>
            /// Update this instance.
            /// Verify states
            /// </summary>
            void Update()
            {
                Movement(_state);
                if (!_gameManager.isDead) Respond();
            }
        }
    }
}

public enum CitizenName                                                         //Enum which contains the names to be randomly assigned.
{
    Adolfo, Jimmy, Fabio, Lola, Marta, Rodolfo, Jesús, Cristian, Santiago, Samuel,
    Ricardo, José, Armando, María, Mónica, Manuel, Andrés, Pablo, Samael, Kratos
}

public struct CitizenData                                                       //Struct containing the citizen's information.
{
    public CitizenName name;
    public State _state;
    public int age;

    static public explicit operator ZombieData(CitizenData citizen)
    {
        return new ZombieData();
    }
}