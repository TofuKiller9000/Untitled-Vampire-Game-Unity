using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{

    private InputManager _inputManager;
    private Camera _cameraMain; 

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        _inputManager.OnStartTouch += Move; //this is subscribing to the event
    }

    private void OnDisable()
    {
        _inputManager.OnEndTouch -= Move; 
    }

    public void Move(Vector2 screenPosition, float time)
    {
        //print("In Move");
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, _cameraMain.nearClipPlane);
        Vector3 worldCoordinates = _cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0; 
        transform.position = worldCoordinates;
    }
}
