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
    public float shootDistance;
    public float shootDelay;
    public float gunRotationSpeed;

    CurrentState currentState;
    bool canShoot = true;
    bool canMove = true;
    

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
        if (canMove == true)
        {
            StartCoroutine(WaitingBeforeMoving(delay));
        }
    }

    public virtual void Shoot()
    {
        if (canMove == true)
        {
            gun.Shoot();
        }
    }

    public virtual void RotateGun()
    {
        Vector3 directionToTarget = Utilities.CalculateDirection(targetedPlayer.transform.position,transform.position);
        directionToTarget = new Vector3(directionToTarget.x,0,directionToTarget.z);
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        gunHeader.rotation = Quaternion.Slerp(gunHeader.rotation, rotationToTarget, gunRotationSpeed * Time.deltaTime);
    }

    IEnumerator WaitingBeforeMoving(float delay)
    {
        currentState = new DamageReceived(this);
        yield return new WaitForSeconds(delay);
        currentState = new MovingState(this);
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

            if (distance <= enemy.stopDistance)
            {
                enemy.navMeshAgent.isStopped = true;
            }
            else 
            {
                enemy.navMeshAgent.isStopped = false;
                enemy.navMeshAgent.SetDestination(enemy.targetedPlayer.transform.position);
            }
        }
    }

    public class DamageReceived : CurrentState
    {
        public DamageReceived(Enemy enemy) : base(enemy)
        {
            enemy.navMeshAgent.isStopped = true;
            enemy.canMove = false;
        }

        public override void ExecuteState()
        {
            base.ExecuteState();
        }

    }
}
