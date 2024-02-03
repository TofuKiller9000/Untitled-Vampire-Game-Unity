using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a generic singleton class that can be used for any script that derives from the Component Class
public class Singleton<T> : MonoBehaviour where T : Component //we will only be passing in classes that derive from component (the base class for everything attached to a gameobject)
{
    private static T _instance; //the instance of the class

    public static T Instance
    {
        get 
        { 
            if (_instance == null) 
            {
                GameObject obj = new GameObject(); 
                obj.name = typeof(T).Name; 
                obj.hideFlags = HideFlags.HideAndDontSave; //this hides this gameobject from the inspector
                _instance = obj.AddComponent<T>();
            }
            return _instance; 
        }

    }

    private void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null; 
        }
    }
}

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    private static T _instance; //the instance of the class

    //public virtual void Awake() //we would need to ovveride this in the class this is implemented in
    //{
    //    if( _instance == null)
    //    {
    //        _instance = this as T; //casting it to match our class
    //        DontDestroyOnLoad( gameObject );
    //    }
    //    else
    //    {
    //        Destroy(this);
    //    }
    //}
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave; //this hides this gameobject from the inspector
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }

    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
