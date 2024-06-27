using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_2 : MonoBehaviour
{
    private float _startingPosX; //This is starting position of the sprites.
    private float _lengthOfSprite;    //This is the length of the sprites.
    public float AmountOfParallax;  //This is amount of parallax scroll. 
    public Camera MainCamera;   //Reference of the camera.


    // Start is called before the first frame update
    void Start()
    {
        //Getting the starting X position of sprite.
        _startingPosX = transform.position.x;    
        //Getting the length of the sprites.
        _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the camera's position
        float camPosX = MainCamera.transform.position.x;

        // Calculate the distance to move the background based on parallax amount
        float distanceX = (camPosX - _startingPosX) * AmountOfParallax;

        // Calculate the new position of the background
        float newPosX = _startingPosX + distanceX;

        // Update the background's position
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);


        // Check if we need to reposition the starting position of the background
        if (camPosX > _startingPosX + _lengthOfSprite)
        {
            _startingPosX += _lengthOfSprite;
        }
        else if (camPosX < _startingPosX - _lengthOfSprite)
        {
            _startingPosX -= _lengthOfSprite;
        }
        // Debug logs
        // Debug.Log("Background Position: " + transform.position);
        // Debug.Log("Background Rotation: " + transform.rotation);
    }
    
}
