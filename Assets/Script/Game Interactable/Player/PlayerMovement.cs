using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Renderer Utilities")]
    [SerializeField] private LineRenderer trajectoryLineRenderer;

    [Header("Movement Parameters")]
    public float speed;
    public CurrentLocalState currentState;

    public Action onTransitState;
    public Action onStartTransitState;
    public Action onStopTransitState;
    public Action onStaticState;
    public Action<Transform,Vector3,Vector3,Vector3> onHitTransform;

    [HideInInspector]public Vector3 lastTargetPosition;

    SkinManager skinManagerScript;


    public struct Transition
    {
        public Vector3 direction;
    }

    void Start()
    {
        currentState = new StaticState(this);
        skinManagerScript = GetComponent<SkinManager>();
    }

    void Update()
    {
        currentState.ExecuteState();
    }

    public void Transit(Transition transition)
    {
        //Change state
        currentState = new TransitState(this,transition);
    }


    public class CurrentLocalState 
    {
        public PlayerMovement playerMovement;

        public Transition transition;

        public CurrentLocalState(PlayerMovement playerMovement)
        {
            this.playerMovement = playerMovement;
        }

        public virtual CurrentLocalState ExecuteState()
        {
            return this;
        }

        public virtual void CalculateTrajectory(Vector3 position){}
    }

    class StaticState : CurrentLocalState
    {
        List<Vector3> points;

        public StaticState(PlayerMovement playerMovement) : base(playerMovement)
        {
            this.playerMovement = playerMovement;

            PlayerActions.instance.onTargeterInputHolding += CalculateTrajectory;
            PlayerActions.instance.onTargeterInputUp += MakeATransition;
        }

        void MakeATransition()
        {
            playerMovement.trajectoryLineRenderer.positionCount = 0;
            PlayerActions.instance.onTargeterInputHolding -= CalculateTrajectory;
            playerMovement.Transit(transition);
        }

        public override CurrentLocalState ExecuteState()
        {
            Utilities.VerifyIfNull(playerMovement.onStaticState);
            return base.ExecuteState();
        }

        public override void  CalculateTrajectory(Vector3 inputPosition)
        {
            inputPosition = new Vector3(inputPosition.x, playerMovement.transform.position.y, inputPosition.z);

            Vector3 dir = Utilities.CalculateDirection(playerMovement.transform.position , inputPosition);

            transition.direction = dir;

            List<Vector3> newPoints = Utilities.CalculateBounce(playerMovement.transform.position,dir,adhesion:0);

            if (newPoints != points)
            {
                playerMovement.trajectoryLineRenderer.positionCount = newPoints.Count();
                playerMovement.trajectoryLineRenderer.SetPositions(newPoints.ToArray());
                points = newPoints;
            }

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            playerMovement.skinManagerScript.currentGunScript.RotateTo(rotation);
            playerMovement.skinManagerScript.currentBaseScript.RotateTo(rotation);
            

        }
    }

    class TransitState : CurrentLocalState
    {
        Movement movement = new Movement();
        Transform[] transforms;
        Transform currentTargetTransform;

        Vector3[] points;
        Vector3[] hitPoints;
        Vector3 direction;

        public TransitState(PlayerMovement playerMovement,Transition transition) : base(playerMovement)
        {
            this.playerMovement = playerMovement;
            this.transition = transition;

            float adhesion = PlayerStats.instance.currentSaw.adhesion;
            points = Utilities.CalculateBounce(playerMovement.transform.position,transition.direction,adhesion:adhesion).ToArray();
            hitPoints = Utilities.CalculateBounce(playerMovement.transform.position,transition.direction).ToArray();
            
            transforms = Utilities.GetTransformFromBounce(playerMovement.transform.position,transition.direction,adhesion:adhesion).ToArray();

            movement.MakeMovement(playerMovement.transform,points,playerMovement.speed);

            Utilities.VerifyIfNull(playerMovement.onStartTransitState);
            
        }

        public override CurrentLocalState ExecuteState()
        {
            if (movement.IsMovementFinish())
            {
                Utilities.VerifyIfNull(playerMovement.onStopTransitState);
                playerMovement.currentState = new StaticState(playerMovement);

                playerMovement.lastTargetPosition = points[points.Length-1];
            }
            
            VerifyBounce();

            movement.MoveStrength();

            Utilities.VerifyIfNull(playerMovement.onTransitState);

            direction = Utilities.CalculateDirection(points[points.Length-2],playerMovement.transform.position);
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            playerMovement.skinManagerScript.currentGunScript.RotateTo(rotation);
            playerMovement.skinManagerScript.currentBaseScript.RotateTo(rotation);

            return base.ExecuteState();
        }

        void VerifyBounce()
        {
            Transform newCurrentTargetTransform = transforms[movement.GetIndex()];

            if (currentTargetTransform != newCurrentTargetTransform)
            {
                if (newCurrentTargetTransform != null)
                {
                    Debug.Log("Bounce on :  "+newCurrentTargetTransform);

                    if (playerMovement.onHitTransform != null)
                    {
                       playerMovement.onHitTransform(newCurrentTargetTransform,points[movement.GetIndex()],hitPoints[movement.GetIndex()],direction);
                    }

                }

                currentTargetTransform = newCurrentTargetTransform;
            }
        }
    }
}
