using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] float _speed;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] MovementsStats _stats;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] float distanceAvecLeSol;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    BoxCollider2D bc;

    internal int Health { get; private set; }
    internal int Ammo { get; private set; }



    internal int Walking { get; private set; }

    bool _grounded, _jumpUsable, _isTouchingAWall, _isJumping, _dead;

    bool walkingRight, walkingLeft;

    public bool Falling => m_Rigidbody.velocity.y < 0;
    public bool Jumping => m_Rigidbody.velocity.y > 0;

    public bool isWalking => Walking != 0;

    public bool Shooting { get; private set; }

    public bool TouchingWall => _isTouchingAWall;

    public bool IsJumping => _isJumping;
    public bool Dead => _dead;

    internal int LastDirectionFaced { get; set; }

    internal bool m_FacingRight = true;

    private float jumpTimeCounter;

    public float jumpTime;
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        LastDirectionFaced = 1;
        Health = 4;
        Ammo = 6;
    }
    // Update is called once per frame
    void Update()
    {
            Walking = (walkingLeft && walkingRight) ? 0 : ((walkingLeft ? -1 : 0) ^ (walkingRight ? 1 : 0));
            if (Walking != 0)
            {
                Walk();
                if (Walking != LastDirectionFaced)
                {
                    Flip();
                }
            }
            else
            {
                ResetVelocity();
            }
           
        
    }

    void FixedUpdate()
    {
            IsGrounded();
            IsTouchingAWall();
            ResetGravity();
            if (!_grounded && _isTouchingAWall)
            {
                if (Jumping)
                {
                    m_Rigidbody.velocity = Vector2.zero;
                }
                m_Rigidbody.gravityScale = 0.1f;
            }
        
    }
    private void Walk()
    {

        float targetSpeed = Mathf.Sign(Walking) * _stats.MoveSpeed;

        float vel = m_Rigidbody.velocity.x;

        float speedDif = targetSpeed - vel;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _stats.VelPower) * Mathf.Sign(speedDif);

        m_Rigidbody.AddForce(movement * Vector2.right);
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void NormalJump()
    {
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
        m_Rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);

    }
    private void GravityModifier()
    {
        if (!Falling) return;
        m_Rigidbody.AddForce(Vector2.down * _stats.FallGravityForce, ForceMode2D.Force);
    }

    private void LateUpdate()
    {
        if (Walking != 0)
        {
            LastDirectionFaced = Walking;
        }
    }

    private void ResetVelocity()
    {
        m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
    }
    public void SetWalkingRight(bool isWalkingRight)
    {
        walkingRight = isWalkingRight;
    }
    public void SetWalkingLeft(bool isWalkingLeft)
    {
        walkingLeft = isWalkingLeft;
    }

    private void IsGrounded()
    {
        bool groundedLive = Physics2D.OverlapCircle(feetPos.position, checkRadius, _groundLayerMask);
        if (groundedLive && _grounded)
        {
            _jumpUsable = true;
        }
        _grounded = groundedLive;
    }

    private void IsTouchingAWall()
    {
        float extraWidth = 0.1f;
        bool touchingWallLive = Physics2D.Raycast(bc.bounds.center, Vector2.right * LastDirectionFaced, bc.bounds.extents.x + extraWidth, _groundLayerMask);
        _isTouchingAWall = touchingWallLive;

    }
    public void Jump()
    {
        if (_jumpUsable)
        {
            if (_grounded) NormalJump();
        }
        _jumpUsable = false;
    }
    public void TakeDamage()
    {
        if (Health == 0) { return; }
        Health -= 1;
        MenuManager.Instance.HUD.UpdateHealthBar();
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healing)
    {
        if (healing + Health > 4)
        {
            Health = 4;
        }
        else
        {
            Health += healing;
        }
        MenuManager.Instance.HUD.UpdateHealthBar();
    }
    public void Shoot()
    {
        if (TouchingWall) { return; }
        if (Ammo == 0)
        {
            Reload();
            return;
        }
        Shooting = true;
        Ammo--;
        MenuManager.Instance.HUD.UpdateBullets();
    }

    private void Die()
    {
        _dead = true;
       //SceneManager.LoadScene("Assets/Scenes/SampleScene.unity", LoadSceneMode.Single);
    }
    private void Reload()
    {
        Ammo = 6;
        MenuManager.Instance.HUD.UpdateBullets();
    }

    public void SetShootingToFalse()
    {
        Shooting = false;
    }

    private void ResetGravity()
    {
        m_Rigidbody.gravityScale = m_Rigidbody.gravityScale != 1 ? 1 : m_Rigidbody.gravityScale;
    }

}
