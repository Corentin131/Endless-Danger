using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.InteropServices;

public class AnimationPack : MonoBehaviour
{
    public enum AnimationType
    {
        ScaleUp,
        ScaleDawn,
        OpacityDown,
        RotateRandomly,  
        Move
    }
    public AnimationType animationType;

    [Header("Global data")]
    public LeanTweenType easeType;
    public bool animateOnStart = true;
    public float time;
    public float timeBeforeStart;
    public float speed;

    public bool destroyOnFinish;

    [Header("Scale Data")]
    public float to;

    [Header("Move Data")]
    public Vector3 toVector;

    

    Vector3 originalScale;
    CurrentState currentState;

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(WaitBeforeStart());
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.ExecuteState();
        }
    }


    public void ScaleUp(Vector3 toAdd,float time)
    {
        Vector3 currentScale = transform.localScale;
        toAdd = currentScale+toAdd;
        LeanTween.scale(gameObject,toAdd,time);
    }

    public void ScaleDown(Vector3 toRemove,float time)
    {
        Vector3 currentScale = transform.localScale;
        toRemove = currentScale-toRemove;
        LeanTween.scale(gameObject,toRemove,time);
    }

    public void ScaleToOrigin(float time)
    {
        LeanTween.scale(gameObject,originalScale,time);
    }

    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(timeBeforeStart);
        if (animateOnStart)
        {
            switch(animationType)
            {
                case AnimationType.ScaleUp:
                    currentState = new ScaleUpState(this);
                    break;

                case AnimationType.OpacityDown:
                    currentState = new OpacityDownState(this);
                    break;

                case AnimationType.RotateRandomly:
                    currentState = new RotateRandomly(this);
                    break;

                case AnimationType.Move:
                    currentState = new MovingState(this);
                    break;
            }
        }

    }

    public void MoveTo(Vector3 to,float speed,float time,[Optional]LeanTweenType leanTweenType)
    {
        currentState = new MovingState(this,to,speed,time,leanTweenType);
    }

    class CurrentState
    {
        public AnimationPack animationPack;
        public virtual CurrentState ExecuteState()
        {
            return this;
        }
    }

    class ScaleUpState : CurrentState
    {
        Vector3 targetScale;
        public ScaleUpState(AnimationPack animationPack)
        {
            Vector3 toAdd = new Vector3(animationPack.to,animationPack.to,animationPack.to);
            targetScale = toAdd+animationPack.transform.localScale;
            LeanTween.scale(animationPack.gameObject,targetScale,animationPack.time).setEase(animationPack.easeType);
            this.animationPack = animationPack;
        }

        public override CurrentState ExecuteState()
        {
            if (animationPack.destroyOnFinish)
            {
                
                if (animationPack.transform.localScale == targetScale)
                {
                   Destroy(animationPack.gameObject); 
                }
                
            }
            return base.ExecuteState();
        }
    }

    class OpacityDownState : CurrentState
    {
        Image image;
        public OpacityDownState(AnimationPack animationPack)
        {
            this.animationPack = animationPack;

            image = animationPack.gameObject.GetComponent<Image>();
            LeanTween.value(animationPack.gameObject, 1, 0, animationPack.time).setOnUpdate((float val) =>
            {
                Color c = image.color;
                c.a = val;
                image.color = c;
            }).setEase(animationPack.easeType);
        }

        public override CurrentState ExecuteState()
        {
            if (animationPack.destroyOnFinish)
            {
                
                if (image.color.a == 0)
                {
                   Destroy(animationPack.gameObject); 
                }
                
            }
            return base.ExecuteState();
        }
    }

    class RotateRandomly : CurrentState
    {
        Vector3 eulers;
        public RotateRandomly(AnimationPack animationPack)
        {
            this.animationPack = animationPack;
            if (animationPack.toVector.x == 0 && animationPack.toVector.y == 0 && animationPack.toVector.z == 0)
                eulers = new Vector3(Random.Range(1,2),Random.Range(1,2),Random.Range(1,2))*animationPack.speed;
            else
                eulers = new Vector3(animationPack.toVector.x,animationPack.toVector.y,animationPack.toVector.z);
        }

        public override CurrentState ExecuteState()
        {
            animationPack.transform.Rotate(eulers*Time.deltaTime);
            return base.ExecuteState();
        }
    }

    class MovingState : CurrentState
    {
        Vector3 to;
        float speed;
        float time;
        LeanTweenType easeType;

        public MovingState(AnimationPack animationPack,Vector3 to,float speed,float time,[Optional]LeanTweenType easeType)
        {
            this.to = to;
            this.speed = speed;
            this.time = time;
            this.animationPack = animationPack;
            this.easeType = easeType;
            StartMoving();
        }

        public MovingState(AnimationPack animationPack)
        {
            this.to = animationPack.toVector;
            this.speed = animationPack.speed;
            this.time = animationPack.time;
            this.animationPack = animationPack;
            this.easeType = animationPack.easeType;
            StartMoving();
        }

        void StartMoving()
        {
            LeanTween.moveLocal(animationPack.gameObject,to,time).setEase(easeType);
        }

        public override CurrentState ExecuteState()
        {

            if (animationPack.transform.position == to)
            {
                Destroy(animationPack.gameObject);
            }
            return base.ExecuteState();
        }

    }

}

