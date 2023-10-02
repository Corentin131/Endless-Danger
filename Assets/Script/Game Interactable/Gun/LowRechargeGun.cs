using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowRechargeGun : Gun
{
    int rechargedObjectIndex = -1; 
    private void Update() 
    {
        int index = Convert.ToInt32(Math.Abs(currentBullet))-1;
        if (isCharging && index != rechargedObjectIndex)
        {
            shooters[index].ChargePhysicObject();
            rechargedObjectIndex = index;
        }else if (isCharging == false)
        {
            rechargedObjectIndex = -1;
        }
    }
   
}
