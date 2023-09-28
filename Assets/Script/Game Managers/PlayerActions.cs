using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions instance;

    //Action
    public Action<Vector3> onTargeterInputHolding;
    public Action<float> onScroll;
    public Action onTargeterInputUp;
    public Action onFireInputHolding;
    public Action onFireInputDawn;
    public Action onFireInputUp;
    
    //Mouse holding
    bool isHoldingLeft = false;
    bool isHoldingRight = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }else
        {
            Debug.Log("Instance of PlayerStats is already instantiate");
        }
    }

    void Update()
    {
        Vector3 mousePosition = Utilities.GetWorldMousePos();

        //for phone
        if(Input.touchCount == 1)
		{	
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Utilities.GetWorldTouchPos(touch);

            onTargeterInputHolding(touchPos);
        }


        //for pc

        if (Input.GetMouseButtonDown(0)) isHoldingLeft = true;
        
        if (Input.GetMouseButtonUp(0))
        {
            isHoldingLeft = false;
            TargeterInputUp();
        }
        
        if (isHoldingLeft == true) TargeterInputHolding(mousePosition);
        

        if (Input.GetMouseButtonDown(1) && onFireInputDawn != null )
        {
            isHoldingRight = true;
            onFireInputDawn();
        }
        if (Input.GetMouseButtonUp(1) && onFireInputUp != null)
        {
            isHoldingRight = false;
            onFireInputUp();
        }

        //if (isHoldingRight == true) Fire();


        float scrollValue = Input.mouseScrollDelta.y;

        if (scrollValue != 0 && onScroll != null) onScroll(scrollValue);
        
    }

    //'onFireInputPressed' is in an function because this function is add in a event 
    public void Fire()
    {
        if (onFireInputHolding != null)
        {
            onFireInputHolding();
        }
    }

    public void TargeterInputUp()
    {
        if (onTargeterInputUp != null)
        {
            onTargeterInputUp();
        }
    }

    public void TargeterInputHolding(Vector3 mousePosition)
    {
        if (onTargeterInputHolding != null)
        {
            onTargeterInputHolding(mousePosition);
        }
    }



}
