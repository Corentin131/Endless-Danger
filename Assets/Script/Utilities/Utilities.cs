using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{

     struct BounceData
     {
          public Vector3 vector3;
          public Transform transform;
     }

     public static Vector3 GetWorldMousePos()
     {
          Vector3 mousePos;

          mousePos = Input.mousePosition;
          mousePos.z = Camera.main.transform.position.y;
          mousePos = Camera.main.ScreenToWorldPoint(mousePos);

          return mousePos;
     }

     public static Vector3 GetWorldTouchPos(Touch touch)
     {
          Vector3 touchPos;

          touchPos = touch.position;
          touchPos.z = Camera.main.transform.position.y;
          touchPos = Camera.main.ScreenToWorldPoint(touchPos);

          return touchPos;
     }
     
     public static void VerifyIfNull(Action toVerify)
     {
          if (toVerify != null)
          {
               toVerify();
          }
     }
     
     public static List<Vector3> CalculateBounce(Vector3 startPoint,Vector3 direction,float maxDistance = 100,int maxBounce = 5,bool drawLine = false,float adhesion = 1)
     {
          List<Vector3> vectors = new List<Vector3>(){};

          List<BounceData> bounceDatas = CalculateGlobalBounce(startPoint,direction,maxDistance,maxBounce,drawLine,adhesion);

          foreach (BounceData bounceData in  bounceDatas)
          {
               vectors.Add(bounceData.vector3);
          }

          return  vectors;


     }

     public static List<Transform> GetTransformFromBounce(Vector3 startPoint,Vector3 direction,float maxDistance = 100,int maxBounce = 5,bool drawLine = false,float adhesion = 1)
     {
          
          List<Transform> transforms = new List<Transform>(){};

          List<BounceData> bounceDatas = CalculateGlobalBounce(startPoint,direction,maxDistance,maxBounce,drawLine,adhesion);

          foreach (BounceData bounceData in  bounceDatas)
          {
               transforms.Add(bounceData.transform);
          }

          return  transforms;
     }

     static List<BounceData> CalculateGlobalBounce(Vector3 startPoint,Vector3 direction,float maxDistance = 100,int maxBounce = 5,bool drawLine = false,float adhesion = 1)
     {
          List<BounceData> bounceData = new List<BounceData>(){new BounceData{vector3 = startPoint,transform = null} };

          RaycastHit hit;

          Physics.Raycast(startPoint, direction, out hit, maxDistance);
          Vector3 currentStartPosition = startPoint;

          foreach (int bounceIndex in Enumerable.Range(0,maxBounce))
          {
               if (Physics.Raycast(currentStartPosition, direction, out hit, 100))
               {
                    if (drawLine == true)
                    {
                         Debug.DrawLine(currentStartPosition, hit.point, Color.red);
                    }
                    
                    //Calculate adhesion
                    Vector3 adhesionDir = hit.normal;
                    adhesionDir = new Vector3(adhesionDir.x,0,adhesionDir.z);
                    Vector3 finalPoint = (hit.point+(adhesionDir*adhesion));

                    Vector3 normal = new Vector3(hit.normal.x,0,hit.normal.z);
                    direction = Vector3.Reflect(direction, normal).normalized;
                    
                    currentStartPosition = finalPoint;

                    bounceData.Add(new BounceData
                    {
                         vector3 = finalPoint,
                         transform = hit.transform

                    });

                    Bounce bounce = hit.transform.GetComponent<Bounce>();

                    if(bounce == null || bounce.bounce == false)
                    {
                         break;
                    }
               }
          }

          return bounceData;

     }

     public static Vector3 CalculateDirection(Vector3 pointA ,Vector3 pointB)
     {
          Vector3 dir = (pointA- pointB).normalized;
          
          return dir;
     }

     public static void VFXSwitch(Transform parent,bool switchOn)
     {
          if (parent != null)
          {
               foreach (TrailRenderer element in parent.GetComponentsInChildren<TrailRenderer>())
               {
                    if (switchOn == true)
                    {
                         element.emitting = true;
                    }else
                    {
                         element.emitting = false;    
                    }
               }

               foreach (ParticleSystem element in parent.GetComponentsInChildren<ParticleSystem>())
               {
                    if (switchOn == true)
                    {
                         Debug.Log("Play");
                         element.Play();
                    }else
                    {
                         element.Stop();
                    }
               }
               foreach (Light element in parent.GetComponentsInChildren<Light>())
               {
                    if (switchOn == true)
                    {
                         element.enabled = true;
                    }else
                    {
                         element.enabled = false;
                    }
               }
          }
     }

     public static IEnumerator ShakeINumerator(float duration,float power,Transform transform,Vector3 startPosition)
     {
          Vector3 initialPosition = transform.localPosition;

          float timer = 0;

          while (timer <  duration)
          {
               float x = UnityEngine.Random.Range(-1,1)*power;
               float z = UnityEngine.Random.Range(-1,1)*power;

               transform.localPosition = new Vector3(x,initialPosition.y,z);

               timer += Time.deltaTime;

               yield return null;
          }

          transform.localPosition = startPosition;
     }
}
