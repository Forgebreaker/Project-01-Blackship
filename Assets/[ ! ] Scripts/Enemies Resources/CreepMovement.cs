using System.Collections;
using UnityEngine;

public class CreepMovement : MonoBehaviour
{
    [Header("Monster Types")]
    [SerializeField] private bool GoingForwardMonster;
    [SerializeField] private bool GoingZigZagMonster;
    [SerializeField] private bool LockTargetAndDashMonster;
    [SerializeField] private bool PlayerFollowMonster;

    [Header("Movement Settings")]
    [SerializeField] private float ForwardSpeed;

    private GameObject Player_GO;
    private GameObject Player;

    private Vector3 GameObjectPosition;
    private Vector3 GameObjectHorizontal;
    private float ZigzagFrequency;
    private float ZigzagMagnitude;

    private Vector3 LockedDirection;
    private bool HasLockedDirection = false;

    private void Start()
    {
        ZigzagFrequency = Random.Range(2, 3);
        ZigzagMagnitude = Random.Range(2, 3);
        GameObjectPosition = transform.position;
        GameObjectHorizontal = transform.right;

        PlayerDetection();

        if (LockTargetAndDashMonster && Player != null)
        {
            LockDirectionToPlayer();
        }
    }

    private void Update()
    {
        if (GoingForwardMonster)
        {
            MoveStraightDown();
        }
        else if (GoingZigZagMonster)
        {
            MoveZigZagDown();
        }
        else if (LockTargetAndDashMonster)
        {
            MoveAlongLockedDirection();
        }
        else if (PlayerFollowMonster && Player != null)
        {
            FollowPlayer();
            LockDirectionToPlayer();
        }

        if (this.gameObject.transform.position.y >= 20f
            || this.gameObject.transform.position.y <= -20f
            || this.gameObject.transform.position.x <= -20f
            || this.gameObject.transform.position.x >= 20f
           )
        {
            Destroy(this.gameObject);
        }

        PlayerDetection();
    }

    private void PlayerDetection()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");

        if (Player_GO != null)
        {
            if (Player_GO.transform.localScale != Vector3.zero)
            {
                Player = Player_GO;
            }
            else
            {
                Player = null;
            }
        }
    }

    /************************************************************ Monster Moves ***********************************************************/
    private void MoveStraightDown()
    {
        this.gameObject.transform.Translate( Vector3.down * ForwardSpeed * Time.deltaTime, Space.Self);
    }

    private void MoveZigZagDown()
    {
        GameObjectPosition += Vector3.down * ForwardSpeed * Time.deltaTime;
        transform.position = GameObjectPosition + GameObjectHorizontal * Mathf.Sin(Time.time * ZigzagFrequency) * ZigzagMagnitude;
    }

    private void LockDirectionToPlayer()
    {
        LockedDirection = (Player.transform.position - transform.position).normalized;
        HasLockedDirection = true;
    }

    private void MoveAlongLockedDirection()
    {
        if (HasLockedDirection)
        {
            transform.position += LockedDirection * ForwardSpeed * Time.deltaTime;
            this.gameObject.transform.up = LockedDirection * -1;
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        this.gameObject.transform.up = direction * -1;
        transform.position += direction * ForwardSpeed * Time.deltaTime;
    }
}
