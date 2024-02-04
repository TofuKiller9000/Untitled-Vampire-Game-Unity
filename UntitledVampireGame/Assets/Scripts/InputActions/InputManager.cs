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
    }

    private void OnDisable()
    {
        touchControls.Disable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Disable();
    }

    private void Start()
    {
        print("In Start");
        touchControls.Touch.PrimaryContact.started += ctx => StartTouch(ctx);//this is the syntax for subscribing to an event.
        //                                                                     //This is simply one of many different ways we can set up inputs
        touchControls.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx);
    }

    public void StartTouch(InputAction.CallbackContext ctx)
    {

        //Debug.Log("Touch started: " + Utils.ScreenToWorld(_cameraMain, touchControls.Touch.TouchPosition.ReadValue<Vector2>()));

        if (OnStartTouch != null) //checking to see if any scripts are currenting listening for this input.
                                  //If no scripts are listening for the input, then it is a waste of time sending the input out
        {
            OnStartTouch(Utils.ScreenToWorld(_cameraMain, touchControls.Touch.TouchPosition.ReadValue<Vector2>()), (float)ctx.startTime);
            //here we are calling the vent, which if any scripts are listening for this event, their functions tied to it will fire off
        }
    }

    public void EndTouch(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Touch ended: " + Utils.ScreenToWorld(_cameraMain, touchControls.Touch.TouchPosition.ReadValue<Vector2>()));
        if (OnEndTouch != null)
        {
            OnEndTouch(Utils.ScreenToWorld(_cameraMain, touchControls.Touch.TouchPosition.ReadValue<Vector2>()), (float)ctx.time);
            //if we wanted to get the amount of time between starting to touch and stopping touch
            //(time - starTime)
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(_cameraMain, touchControls.Touch.TouchPosition.ReadValue<Vector2>());
    }
}
