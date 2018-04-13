using UnityEngine;
/// <summary>
/// Fps move.
/// This class handles the movement of the character using it, 
/// using the required speed, and making it move only as "A", "W", "S" and "D".
/// </summary>
public class FpsMove : MonoBehaviour 
{
    public float speed;                                                 //Variable public to modify the speed from another class           

    void Update()
    {
        if (Input.GetKey(KeyCode.A))                                    //when you press the "A" key, it will move left at its assigned speed.
            transform.position -= transform.right * speed;

        if (Input.GetKey(KeyCode.W))                                    //when you press the "W" key, it will move forward at its assigned speed.        
            transform.position += transform.forward * speed;   

        if (Input.GetKey(KeyCode.S))                                    //when you press the "S" key, it will move back at its assigned speed.        
            transform.position -= transform.forward * speed; 
        
        if (Input.GetKey(KeyCode.D))                                    //when you press the "D" key, it will move right at its assigned speed.       
            transform.position += transform.right * speed;             
    }
}
