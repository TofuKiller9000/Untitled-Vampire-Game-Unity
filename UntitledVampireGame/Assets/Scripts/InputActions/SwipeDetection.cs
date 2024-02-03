using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{

    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f; 

    private InputManager _inputManager;
    private Vector2 _startPosition;
    private float _startTime;

    private Vector2 _endPosition;
    private float _endTime;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd; 
    }

    private void OnDisable()
    {
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd; 
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        print("In DetectSwipe()");
        if(Vector3.Distance(_startPosition, _endPosition) >= minimumDistance && (_endTime - _startTime) <= maximumTime)
        {
            print("Detected Swipe");
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
        }
    }
}
