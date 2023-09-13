using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Vector3 direction;
    public Vector3 from;
    public Vector3 to;
    public Vector3[] points = new Vector3[]{};
    float speed;
    int index = 0;
    Transform transform;
    Vector3 freezePosition;

    bool freezeX;
    bool freezeY;
    bool freezeZ;

    public void MoveStrength()
    {
        Vector3 to = Move();

        if(to != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, Time.deltaTime * speed);
        }
    }

    public void MoveLerp()
    {
        Vector3 to = Move();
        
        if(to != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, to, Time.deltaTime * speed);
        }
    }

    public bool IsMovementFinish()
    {

        if (index >= points.Length-1)
        {
            return true;
        }

        return false;
    }

    public void MakeMovement(Transform transform,Vector3[] points,float speed,bool freezeX = false,bool freezeY = false,bool freezeZ= false)
    {
        this.transform = transform;
        this.freezePosition = transform.position;

        this.freezeX = freezeX;
        this.freezeY = freezeY;
        this.freezeZ = freezeZ;

        this.speed = speed;


        this.points = FreezeVectors(points);

        index = 0;
    }
    
    public int GetIndex()
    {
        if (index > points.Length-1)
        {
            return points.Length-1;
        }
        return index;
    }

    public Vector3[] FreezeVectors(Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length; i += 1)
        {
            float x = vectors[i].x;
            float y = vectors[i].y;
            float z = vectors[i].z;

            if (freezeX) x = transform.position.x;
            if (freezeY) y = transform.position.y;
            if (freezeZ) z = transform.position.z;

            vectors[i] = new Vector3(x,y,z);
            
        }

        return vectors;
    }

    Vector3 Move()
    {
        if (index <= points.Length-1 )
        {
            from = points[index];

            if (index == points.Length-1)
            {
                to = points[index];
            }else
            {
                to = points[index+1];
            }

            float staticDistance = Vector3.Distance(from,to);
            float currentDistance = Vector3.Distance(to,transform.position);

            if ((transform.position == to && index < points.Length) || (currentDistance < 5f && index != points.Length-2))
            {
                index+=1;
                transform.position = to;
            }

            direction = Utilities.CalculateDirection(from,to);

            return to;
        }
        return Vector3.zero;
    }
}
