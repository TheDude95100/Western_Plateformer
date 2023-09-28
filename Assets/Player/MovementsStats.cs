
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


    [Header("JUMP")]

    public float JumpForce = 5f;

    public float FallGravityForce = 1.9f;
}
