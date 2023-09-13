using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AnimationPack : MonoBehaviour
{
    public enum AnimationType
    {
        ScaleUp,
        ScaleDawn,
        OpacityDown,
        RotateRandomly
    }

    public AnimationType animationType;

    [Header("Global data")]
    public float time;
    public float speed;

    public bool destroyOnFinish;

    [Header("Scale Data")]
    public float to;

    

    Vector3 originalScale;
    CurrentState currentState;

    void Start()
    {
        originalScale = transform.localScale;

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
        }
    }

    void Update()
    {
         currentState.ExecuteState();
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
            LeanTween.scale(animationPack.gameObject,targetScale,animationPack.time);
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
            });
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
            eulers = new Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1))*animationPack.speed;
        }

        public override CurrentState ExecuteState()
        {
            animationPack.transform.Rotate(eulers);
            return base.ExecuteState();
        }
    }

}

