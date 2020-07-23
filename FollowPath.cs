using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{
    #region Enums
    public enum MovementType 
    {
        MoveTowards,
        LerpTowards
    }
    #endregion 

    #region Public Variables
    public MovementType Type = MovementType.MoveTowards; 
    public MovementPath MyPath;
    public float Speed = 1; 
    public float MaxDistanceToGoal = .1f; 
    #endregion

    public float turnAngle = 90f;

    #region Private Variables
    private IEnumerator<Transform> pointInPath; 
    #endregion 

    
    #region Main Methods
    public void Start()
    {
       
        if (MyPath == null)
        {
            Debug.LogError("Movement Path cannot be null, I must have a path to follow.", gameObject);
            return;
        }

        
        pointInPath = MyPath.GetNextPathPoint();
        Debug.Log(pointInPath.Current);
        
        pointInPath.MoveNext();
        Debug.Log(pointInPath.Current);

       
        if (pointInPath.Current == null)
        {
            Debug.LogError("A path must have points in it to follow", gameObject);
            return;
        }

        
        transform.position = pointInPath.Current.position;
    }
     
    
    public void Update()
    {
        
        if (pointInPath == null || pointInPath.Current == null)
        {
            return;
        }

        if (Type == MovementType.MoveTowards)
        {
            
            transform.position =
                Vector3.MoveTowards(transform.position,
                                    pointInPath.Current.position,
                                    Time.deltaTime * Speed);
        }
        else if (Type == MovementType.LerpTowards) 
        {
            
            transform.position = Vector3.Lerp(transform.position,
                                                pointInPath.Current.position,
                                                Time.deltaTime * Speed);
        }

        //Check to see if you are close enough to the next point to start moving to the following one
        //Using Pythagorean Theorem
        //per unity suaring a number is faster than the square root of a number
        //Using .sqrMagnitude
        var distanceSquared = (transform.position - pointInPath.Current.position).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) //If you are close enough
        {
            transform.Rotate(Vector3.up, turnAngle);
            pointInPath.MoveNext(); //Get next point in MovementPath
        }
        
    }
    #endregion //Main Methods

    //(Custom Named Methods)
    #region Utility Methods 

    #endregion //Utility Methods

    //Coroutines run parallel to other fucntions
    #region Coroutines

    #endregion //Coroutines
}
