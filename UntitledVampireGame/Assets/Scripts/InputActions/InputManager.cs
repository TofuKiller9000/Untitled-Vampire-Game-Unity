using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch; 


[DefaultExecutionOrder(-1)] //changes the execution order of our script. Negative numbers are typically for things such as the event system and UI stuff
public class InputManager : Singleton<InputManager>
{
    private TouchControls touchControls;
    private Camera _cameraMain;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private void Awake()
    {
        touchControls = new TouchControls();
        _cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        touchControls.Enable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        touchControls.Disable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);//this is the syntax for subscribing to an event.
                                                                         //This is simply one of many different ways we can set up inputs
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);

        
    }

    private void FingerDown(Finger finger)
    {
        if(OnStartTouch != null)
        {
            OnStartTouch(finger.screenPosition, Time.time);
        }
    }

    public void StartTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch started: " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());

        if(OnStartTouch != null) //checking to see if any scripts are currenting listening for this input.
                                 //If no scripts are listening for the input, then it is a waste of time sending the input out
        {
            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.startTime); //here we are calling the vent, which if any scripts are listening for this event, their functions tied to it will fire off
        }
    }

    public void EndTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch ended");
        if (OnEndTouch != null) 
        {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time); //if we wanted to get the amount of time between starting to touch and stopping touch
                                                                                                 //(time - starTime)
        }
    }

    public Vector2 TouchPosition()
    {
        Vector3 screenCoordinates = new Vector3(touchControls.Touch.TouchPosition.ReadValue<Vector2>().x, touchControls.Touch.TouchPosition.ReadValue<Vector2>().y, _cameraMain.nearClipPlane);
        Vector3 worldCoordinates = _cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
        return worldCoordinates;
    }
}
