using System.Collections;
using UnityEngine;
using NPC.Ally;

namespace NPC
{
    /// <summary>
    /// Npc.
    /// NPC class, which will be the parent class of the classes that inherit it
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Npc : MonoBehaviour
    {
        GameManager _gameManager;
        public float closeDistance = 5.0F;
        public float speed;
        public int age;
        public State _state;
        float rotateSpeed;
        bool run;

        /// <summary>
        /// Awake this instance.
        /// Defines initialized variables
        /// </summary>
        void Awake()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();      //start the variable "_gameManager", with the "GameManager" component.
            age = Random.Range(15, 100);
            if (age >= 15 && age <= 25) speed = 0.5f;
            else if (age >= 26 && age <= 35) speed = 0.4f;
            else if (age >= 36 && age <= 50) speed = 0.3f;
            else if (age >= 51 && age <= 75) speed = 0.2f;
            else if (age >= 76 && age <= 100) speed = 0.15f;
            StartCoroutine(States());
        }

        /// <summary>
        /// Movement this instance.
        /// Checks the NPC status, whether it is "Idle", "Moving", "Rotating" or "Running".
        /// </summary>
        public void Movement(State s)
        {
            switch (s)
            {
                case State.Idle:
                    transform.position += new Vector3(0, 0, 0);
                    transform.Rotate(Vector3.zero);
                    goto case State.Running;
                case State.Moving:
                    transform.position += transform.forward * (speed * Time.deltaTime);
                    transform.Rotate(Vector3.zero);
                    goto case State.Running;
                case State.Rotating:
                    rotateSpeed = Random.Range(0.2f, 0.35f);
                    transform.position += new Vector3(0, 0, 0);
                    transform.Rotate(Vector3.up * rotateSpeed);
                    goto case State.Running;
                case State.Running:
                    if (!_gameManager.isDead) Respond();
                    break;
                default:
                    goto case State.Idle;
            }
        }

        /// <summary>
        /// Respond this instance.
        /// So that the Zombie look for the Citizen and the Hero being in the range.
        /// </summary>
        public virtual void Respond()
        {
            foreach (GameObject go in GameManager.npc)
            {
                if (go.GetComponent<Citizen>() || go.GetComponent<Hero>())
                {
                    float dist = Vector3.Distance(go.transform.position, transform.position);
                    if (dist <= closeDistance)
                    {
                       run = true;
                       transform.position = Vector3.MoveTowards(transform.position, go.transform.position, (speed * Time.deltaTime));
                       _state = State.Running;
                    }else
                    {
                        run = false;
                    }
                }
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
            if (!run)
            {
                yield return new WaitForSeconds(3);
                _state = (State)Random.Range(0, 3);
                Movement(_state);
                StartCoroutine(States());
            }
        }
    }
}

public enum State { Idle, Moving, Rotating, Running }                           //Enum which contains the states to be randomly assigned.