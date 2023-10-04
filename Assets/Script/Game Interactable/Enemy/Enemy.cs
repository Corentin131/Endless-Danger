using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Sources")]
    public NavMeshAgent navMeshAgent;
    public Gun gun;
    public Transform gunHeader;
    public List<GameObject> players;

    [HideInInspector]public GameObject targetedPlayer;

    [Header("Data")]
    public float stopDistance;
    public float goBackDistance;
    public float shootDistance;
    public float shootDelay;
    public float gunRotationSpeed;

    CurrentState currentState;
    bool canShoot = true;
    bool canMove = true;
    GameObject currentTargetGun;

    void Start()
    {
        currentState = new MovingState(this);
    }

    void Update()
    {
        currentState.ExecuteState();
    }

    public void ReceiveDamage(float delay)
    {
        currentState = new WaitingBeforeMove(this,delay);
    }

    public virtual void Shoot()
    {
        if (canMove == true && currentTargetGun != null &&currentTargetGun.tag != "Enemy")
        {
            gun.Shoot();
        }
    }

    public virtual void RotateGun()
    {
        RotateTo(gunHeader,targetedPlayer.transform.position,gunRotationSpeed,true);
    }

    void RotateTo(Transform transform , Vector3 target,float speed,bool freezeY = false)
    {
        Vector3 directionToTarget = Utilities.CalculateDirection(target,transform.position);
        float y = 0;

        if (freezeY == true)
        {
            y = directionToTarget.y;
        }

        directionToTarget = new Vector3(directionToTarget.x,directionToTarget.y,directionToTarget.z);
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, speed * Time.deltaTime);
    }


    IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public class CurrentState
    {
        public Enemy enemy;
        public float distance;

        public CurrentState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public virtual void ExecuteState()
        {
            float closestDistance = 100;
            GameObject closestPlayer = enemy.players[0];

            foreach (GameObject player in enemy.players)
            {
                float distance = Vector3.Distance(player.transform.position,enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }

            }

            enemy.targetedPlayer = closestPlayer;

            GameObject target = null;

            foreach (Shooter shooter in enemy.gun.shooters)
            {
                target = shooter.currentTarget;
            }

            enemy.currentTargetGun = target;

            distance = Vector3.Distance(enemy.transform.position , enemy.targetedPlayer.transform.position);
            enemy.RotateGun();

            if (distance < enemy.shootDistance)
            {
                enemy.Shoot();
            }
        }
    }

    public class MovingState : CurrentState
    {
        public MovingState(Enemy enemy) : base(enemy)
        {
            enemy.navMeshAgent.isStopped = false;
            enemy.canMove = true;
        }

        public override void ExecuteState()
        {
            base.ExecuteState();

            if (distance <= enemy.goBackDistance)
            {
                enemy.navMeshAgent.isStopped = false;
                enemy.navMeshAgent.SetDestination(new Vector3(0,0,0));
            }
            else if (distance <= enemy.stopDistance)
            {
                enemy.navMeshAgent.isStopped = true;
                enemy.RotateTo(enemy.transform,enemy.targetedPlayer.transform.position,5);
            }
            else 
            {
                enemy.navMeshAgent.isStopped = false;
                enemy.navMeshAgent.SetDestination(enemy.targetedPlayer.transform.position);
                enemy.RotateTo(enemy.transform,enemy.targetedPlayer.transform.position,5);
            }
        }
    }

    public class WaitingBeforeMove : CurrentState
    {
        float time = 0;
        float delay = 0;
        public WaitingBeforeMove(Enemy enemy,float delay) : base(enemy)
        {
            enemy.navMeshAgent.isStopped = true;
            enemy.canMove = false;
            this.delay = delay;
        }

        public override void ExecuteState()
        {
            enemy.RotateTo(enemy.transform,enemy.targetedPlayer.transform.position,5);
            time += Time.deltaTime;

            if (time >= delay)
            {
                enemy.currentState = new MovingState(enemy);
            }
            base.ExecuteState();
        }

    }
}
