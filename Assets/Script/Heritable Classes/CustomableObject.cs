using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomableObject : MonoBehaviour
{
    [HideInInspector] public PlayerMovement playerMovementScript;
    [HideInInspector] public SkinManager skinManagerScript;

    public virtual void Start()
    {
        playerMovementScript.onStaticState += () => OnStaticState();
        playerMovementScript.onTransitState += () => OnTransitState();
        playerMovementScript.onStartTransitState += () => OnStartTransitState();
        playerMovementScript.onStopTransitState += () => OnStopTransitState();
        playerMovementScript.onHitTransform += OnHitTransform;
        
        PowerBar.instance.onPowerChange += OnPowerChange;
        PowerBar.instance.OnStartRecharge += OnStartRecharge;
        PowerBar.instance.OnStopRecharge += OnStopRecharge;
    }

    public virtual void OnTargeterInputHolding(Vector3 mousePosition)
    {
       
    }

    public virtual void OnTransitState()
    {
    }

    public virtual void OnStartTransitState()
    {
    }

    public virtual void OnStopTransitState()
    {
    }

    public virtual void OnStaticState()
    {
    }

    public virtual void OnPowerChange(int value)
    {

    }

    public virtual void OnHitTransform(Transform transform,Vector3 playerPosition , Vector3 vfxPosition, Vector3 direction)
    {

    }

    public virtual void OnStartRecharge()
    {

    }

    public virtual void OnStopRecharge()
    {

    }

}
