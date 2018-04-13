using UnityEngine;
/// <summary>
/// Fps camera.
/// This class controls camera movement in a "First Person Shooter" manner.
/// </summary>
public class FpsCamera : MonoBehaviour 
{
    float mouseX;                                                       //In this variable the coordinates are stored in "x".                   
    float mouseY;                                                       //In this variable the coordinates are stored in "y".                                
    float maxX = -45;                                                     //In this variable we limit the maximum of "x".
    float maxY = 45;                                                      //In this variable we limit the maximum of "y".
    GameObject body;                                                    //A "GameObject" type variable to save the body.

    void Start()                                                       
    {
        body = FindObjectOfType(typeof(GameObject)) as GameObject;     //In the variable "body" I save the object that is found with "FindObjectOfType".
    }

    void Update()                                                      
    {
        mouseX += Input.GetAxis("Mouse X");                            //saving the position of "x". 
        mouseY -= Input.GetAxis("Mouse Y");                            //saving the position of "y". 
        mouseY = Mathf.Clamp(mouseY, maxX, maxY);                      //With "Mathf.Clamp" I limit the movement with the previously created variables.
        transform.eulerAngles = new Vector3(mouseY, mouseX, 0);        //I move the camera with angles in "x" and "y".
        body.transform.eulerAngles = new Vector3(0, mouseX, 0);        //"body" broken with respect to the camera only at "y".
    }
}
