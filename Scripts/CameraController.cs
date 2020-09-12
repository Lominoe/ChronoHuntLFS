using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //The speed of the movement
    public float cameraSpeed = 0.5f;
    public float speedOfIncrease = 3;

    //Min and max-values the camera can move to.
    private float MAX_X = 6;
    private float MIN_X = -6;
    private float MAX_Y = 3;
    private float MIN_Y = -3;

    //The current x and y movement of the cursor
    private float Xmouse;
    private float Ymouse;

    public Camera MainCam;
    public GameObject player;

    private void FixedUpdate()
    {
        MainCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, MainCam.transform.position.z);
    }

    void Update()
    {

        Xmouse = Input.GetAxis("Mouse X");
        Ymouse = Input.GetAxis("Mouse Y");

        Vector3 v3 = Input.mousePosition;
        v3.z = transform.position.z;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        Vector3 newPos = transform.position;
        newPos.x += Xmouse * speedOfIncrease;
        newPos.y += Ymouse * speedOfIncrease;

        //Making sure that the camera doesn't exceed the min/max values it's allowed to move to!
        if (newPos.x > MAX_X)
        {
            newPos.x = MAX_X;
        }
        if (newPos.x < MIN_X)
        {
            newPos.x = MIN_X;
        }

        if (newPos.y > MAX_Y)
        {
            newPos.y = MAX_Y;
        }
        if (newPos.y < MIN_Y)
        {
            newPos.y = MIN_Y;
        }

        //Moves the Camera
        transform.position = Vector3.Lerp(transform.position, newPos, cameraSpeed * Time.deltaTime);



    }

}

