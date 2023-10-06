using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBall : MonoBehaviour
{
    [Header("Source")]
    public Animator animator;
    public GameObject model;
    public List<GameObject> effectsToSpawn;
    public Transform vfxHeaderOnStart;

    [Header("Data")]
    public float speed;


    CurrentState currentState;

    void Start()
    {
        
    }

    void  Update()
    {
        if (currentState != null)
        {
            currentState.ExecuteState();
        }
    }

    public virtual void Charge()
    {
        model.SetActive(true);
        animator.SetTrigger("Charge");
        currentState = new WaitForShootDefault(this);
    }

    public virtual void Shoot()
    {
        currentState.Shoot();
        currentState = new MovingStateDefault(this);
    }

    public virtual void FinishCourse()
    {
        foreach (GameObject objectToSpawn in effectsToSpawn)
        {
            GameObject gameObject = Instantiate(objectToSpawn , transform.position , transform.rotation);
        }

        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        CollisionManager collisionManager = other.GetComponent<CollisionManager>();

        if (currentState.stateName == "MovingState")
        {
            if (collisionManager == null || collisionManager.physicBallImmunities == false)
            {
                Debug.Log(other.transform);
                FinishCourse();
            }
        }
    }

    
    class CurrentState
    {
        public string stateName = "DefaultState";
        public PhysicBall physicBall;

        public CurrentState(PhysicBall physicBall)
        {
            this.physicBall = physicBall;
        }
        public virtual void ExecuteState()
        {

        }

        public virtual void Shoot()
        {

        }
    }

    class WaitForShootDefault : CurrentState
    {
        public WaitForShootDefault(PhysicBall physicBall) : base(physicBall)
        {
            stateName = "WaitForShoot";
        }

        public override void Shoot()
        {
            physicBall.transform.parent = null;
        }

    }

    class MovingStateDefault : CurrentState
    {
        float timeFromShoot;

        public MovingStateDefault(PhysicBall physicBall) : base(physicBall)
        {
            stateName = "MovingState";
            Utilities.VFXSwitch(physicBall.vfxHeaderOnStart,true);
        }

        public override void ExecuteState()
        {
            timeFromShoot+=Time.deltaTime;
            physicBall.transform.Translate(new Vector3(0,0,1)*physicBall.speed*Time.deltaTime);
            if (timeFromShoot > 10)
            {
                Destroy(physicBall.gameObject);
            }
        }

    }
}
