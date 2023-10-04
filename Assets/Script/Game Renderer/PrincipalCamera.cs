using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincipalCamera : MonoBehaviour
{
    public static PrincipalCamera instance;
    
    [Header("Source")]
    public Transform playerTransform;

    [Header("Movement Parameters")]
    public float speed = 5;
    float originalY;

    CurrentState currentState;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of PrincipalCamera is already created");
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentState = new FollowingState(this);
    }

    void Update()
    {
        currentState.ExecuteState();
    }

    public void Shake(float duration,float power)
    {
        StartCoroutine(Utilities.ShakeINumerator(duration,power,transform,Vector3.zero));
    }

    public void MoveFast(Vector3 to,float speed)
    {
        currentState = new MovingTo(this,to,speed);
    }

    class CurrentState 
    {
        public virtual void ExecuteState()
        {

        }
    }

    class FollowingState : CurrentState
    {
        PrincipalCamera principalCamera;

        public FollowingState(PrincipalCamera principalCamera)
        {
            this.principalCamera = principalCamera;
        }

        public override void ExecuteState()
        {
            Vector3 positionFreeze = principalCamera.transform.parent.position;
            principalCamera.transform.parent.position = Vector3.Lerp(principalCamera.transform.parent.position,principalCamera.playerTransform.position,principalCamera.speed*Time.deltaTime);
            Vector3 currentPosition = principalCamera.transform.parent.position;
            
            principalCamera.transform.parent.position = new Vector3(currentPosition.x,positionFreeze.y,currentPosition.z);

        }
    }
    
    class MovingTo : CurrentState
    {
        PrincipalCamera principalCamera;
        Vector3 to;
        float speed;

        public MovingTo(PrincipalCamera principalCamera,Vector3 to,float speed)
        {
            this.principalCamera = principalCamera;
            this.to = to;
            this.speed = speed;
        }

        public override void ExecuteState()
        {
            Vector3 positionFreeze = principalCamera.transform.parent.position;
            principalCamera.transform.parent.position = Vector3.Lerp(principalCamera.transform.parent.position,to,speed*Time.deltaTime);
            Vector3 currentPosition = principalCamera.transform.parent.position;
            
            principalCamera.transform.parent.position = new Vector3(currentPosition.x,positionFreeze.y,currentPosition.z);

            Vector3 cameraPosition = principalCamera.transform.parent.position;

            if (Convert.ToInt32(cameraPosition.x*100) == Convert.ToInt32(to.x*100) && Convert.ToInt32(cameraPosition.z*100) == Convert.ToInt32(to.z*100))
            {
                principalCamera.currentState = new FollowingState(principalCamera);
            }
        }
    }
    
}
