using CVStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Move : MonoBehaviour {

    [SerializeField] private bool setRandomSpeed = false;
    [SerializeField] private float speed = 10;
    [SerializeField] private Vector2 randomSpeedRange = new Vector2(1,5);

    [SerializeField] private float despawnTimer = 2;
    [SerializeField] private MoveDirection moveDirection = MoveDirection.Forward;
    
    private float timer = 0;
    private Vector3 moveDir;

    public enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backwards
    }
    Vector3 moveVelocity;

    // Update is called once per frame
    void Update()
    {
        SetMoveDirection();
        if (setRandomSpeed)
        {
            moveVelocity = Random.Range(randomSpeedRange.x, randomSpeedRange.y) * Time.deltaTime * moveDir;
        }
        else
        {
            moveVelocity = speed * Time.deltaTime * moveDir;
        }

        transform.position += moveVelocity;

        timer += Time.deltaTime;
        if (despawnTimer > 0)
        {
            if (timer >= despawnTimer)
            {
                timer = 0;
                // SPManager.instance.DisablePoolObject(transform);
            }
        }
    }

    internal void SetSpeed(float value)
    {
        speed = value;
    }

    internal void SetDespawnTimer(float value)
    {
        despawnTimer = value;
    }

    void SetMoveDirection()
    {
        switch (moveDirection)
        {
            case MoveDirection.Left:
                moveDir = -transform.right;
                break;
            case MoveDirection.Right:
                moveDir = transform.right;
                break;
            case MoveDirection.Up:
                moveDir = transform.up;
                break;
            case MoveDirection.Down:
                moveDir = -transform.up;
                break;
            case MoveDirection.Forward:
                moveDir = transform.forward;
                break;
            case MoveDirection.Backwards:
                moveDir = -transform.forward;
                break;
            default:
                break;
        }
    }

}