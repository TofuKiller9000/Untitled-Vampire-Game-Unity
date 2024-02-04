using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{

    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;
    [SerializeField] private GameObject trail; 

    private InputManager _inputManager;
    private Vector2 _startPosition;
    [SerializeField]
    private float _startTime;

    private Vector2 _endPosition;
    private float _endTime;

    private Coroutine _coroutine; 

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
        _startTime = 0f;
        _startPosition = position;
        //_startTime = time;
        _startTime += Time.deltaTime;
        trail.SetActive(true);
        trail.transform.position = position;

        _coroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while(true)
        {
            trail.transform.position = _inputManager.PrimaryPosition();
            yield return null;
        }
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        StopCoroutine(_coroutine);
        trail.SetActive(false);
        _endPosition = position;
        //_endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector2.Distance(_startPosition, _endPosition) >= minimumDistance && (_startTime) <= maximumTime)
        {
            //print("Detected Swipe");
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
            Vector3 direction = _endPosition - _startPosition;
            Vector2 direction2d = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2d);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if(Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
        }
        else if(Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
        }
    }
}
