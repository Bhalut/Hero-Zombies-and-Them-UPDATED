using System.Collections;
using UnityEngine;

namespace NPC                                                                   //Library containing two libraries under "Ally" and "Enemy".
{
    namespace Enemy                                                             //Librería Enemy, belongs to the NPC library, which contains the Zombie's class.
    {
        /// <summary>
        /// Zombie.
        /// This class contains the zombie's information, such as his taste (what he wants to eat), 
        /// his color and his state (whether he is still, moving or rotating).
        /// </summary>
        [RequireComponent(typeof(Rigidbody))]
        public class Zombie : MonoBehaviour
        {
            int move;                                                                   //variable to assign a action to each zombie.
            ZombieData _zombieData;                                                     //variable containing the struct of the zombie.

            /// <summary>
            /// Start this instance.
            /// starts the coroutine "States", randomly assigns the zombie's taste, 
            /// freezes the zombie's Rigidbody rotation.
            /// </summary>
            void Start()
            {
                StartCoroutine(States());
                _zombieData._taste = (Taste)Random.Range(0, 5);
                gameObject.GetComponent<Rigidbody>().freezeRotation = enabled;
            }

            /// <summary>
            /// Update this instance.
            /// Contains a switch that is randomly decided to move the zombie 
            /// in a random direction if the state is to "moving" or "rotating".
            /// </summary>
            void Update()
            {
                switch (move)
                {
                    case 0:
                        transform.position += transform.forward * 3f * Time.deltaTime;
                        transform.Rotate(Vector3.zero);
                        break;
                    case 1:
                        transform.position -= transform.forward * 2f * Time.deltaTime;
                        transform.Rotate(Vector3.zero);
                        break;
                    case 2:
                        transform.position += transform.right * 3f * Time.deltaTime;
                        transform.Rotate(Vector3.zero);
                        break;
                    case 3:
                        transform.position -= transform.right * 2f * Time.deltaTime;
                        transform.Rotate(Vector3.zero);
                        break;
                    case 4:
                        transform.position += new Vector3(0, 0, 0);
                        transform.Rotate(Vector3.zero);
                        break;
                    case 5:
                        transform.position += new Vector3(0, 0, 0);
                        transform.Rotate(Vector3.up * _zombieData.rotateSpeed);
                        break;
                    case 6:
                        transform.position += new Vector3(0, 0, 0);
                        transform.Rotate(Vector3.down * _zombieData.rotateSpeed);
                        break;
                    default:
                        transform.position += transform.forward * 3f * Time.deltaTime;
                        transform.Rotate(Vector3.zero);
                        break;
                }
            }

            /// <summary>
            /// Movement this instance.
            /// Checks the zombie's status, whether it is "Idle", "Moving" or "Rotating".
            /// </summary>
            void Movement()
            {
                switch(_zombieData._state)
                {
                    case State.Idle:
                        move = 4;
                        StartCoroutine(States());
                        break;
                    case State.Moving:
                        move = Random.Range(0, 4);
                        StartCoroutine(States());
                        break;
                    case State.Rotating:
                        move = Random.Range(5, 7);
                        _zombieData.rotateSpeed = Random.Range(0.5f, 3f);
                        StartCoroutine(States());
                        break;
                    default:
                        move = 4;
                        StartCoroutine(States());
                        break;
                }
            }

            /// <summary>
            /// States this instance.
            /// Coroutine that randomly chooses the behavior of the zombie coming from an enumerator. 
            /// It also calls the method that checks the assigned status.
            /// </summary>
            /// <returns>The states.</returns>
            IEnumerator States()
            {
                yield return new WaitForSeconds(3);
                _zombieData._state = (State)Random.Range(0, 3);
                Movement();
                yield return new WaitForSeconds(3);
            }

            public ZombieData ZombieID()                                        //Method that returns the structure containing the citizen's information
            {
                return _zombieData;
            }
        }
    }
}

public enum State { Idle, Moving, Rotating }                                    //Enum which contains the states to be randomly assigned.
public enum Taste { arm, nose, ear, finger, leg }                               //Enum which contains the tastes to be randomly assigned.

public struct ZombieData                                                        //Struct containing the citizen's information.
{
    public State _state;
    public Taste _taste;
    public Color[] color;
    public float rotateSpeed;
}
