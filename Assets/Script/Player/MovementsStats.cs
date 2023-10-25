
using UnityEngine;

[CreateAssetMenu]
public class MovementsStats : ScriptableObject
{
    [Header("MOVEMENT")]

    public float MoveSpeed = 20f;

    public float Acceleration = 5f;

    public float Deceleration = 5f;

    public float VelPower = 1.2f;

    public float Friction = 0.7f;

    public float WallSlidingSpeed = 1.0f;


    [Header("JUMP")]

    public float JumpForce = 5f;

    public float DoubleJumpForce = 3f;

    public float FallGravityForce = 1.9f;

    [Header("DASH")]

    public float DashForce = 24f;
    public float DashingTime = 2f;
    public float DashingCooldown = 1f;

    [Header("WALL JUMP")]

    public Vector2 WallJumpForce = new Vector2(3f,5f);
    public float WallJumpTime = 0.2f;
    public float WallJumpDuration = 0.4f;
}
