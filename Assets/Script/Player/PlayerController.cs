using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] float _speed;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] MovementsStats _stats;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] LayerMask _iceLayerMask;
    [SerializeField] float distanceAvecLeSol;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] TrailRenderer tr;
    [SerializeField] BulletControler _bulletController;
    [SerializeField] GunControler _gunControler;
    BoxCollider2D bc;

    internal int Health { get; private set; }
    internal int Ammo { get; private set; }

    internal int JumpAvailable { get; private set; }
    internal int Walking { get; private set; }

    public bool Hurting { get; set; }

    bool _grounded, _jumpUsable, _isTouchingAWall, _isJumping, _dead, _canDash, _isWallSliding;

    bool walkingRight, walkingLeft;

    #region Interface values
    public bool Falling => m_Rigidbody.velocity.y < -0.05;
    public bool Jumping => m_Rigidbody.velocity.y > 0.05;

    public PlayerInput playerInput { get; private set; }
    public bool isWalking => Walking != 0;
    public bool Shooting { get; private set; }
    public bool Dashing { get; private set; }

    public bool TouchingWall => _isTouchingAWall;

    public bool IsJumping => _isJumping;
    public bool Dead => _dead;

    public bool Swinging { get; set; }
    public bool isSwinging { get; set; }
    #endregion


    internal bool m_FacingRight;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        m_FacingRight = true;
        Health = 4;
        Ammo = 6;
        JumpAvailable = 0;
        playerInput = GetComponent<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Dashing) { return; }
        Walking = (walkingLeft && walkingRight) ? 0 : ((walkingLeft ? -1 : 0) ^ (walkingRight ? 1 : 0));
        if (Walking != 0)
        {
            Walk();
        }
        

    }

    void FixedUpdate()
    {
        if (Dashing) { return; }
        DoINeedToFlip();
        IsGrounded();
        IsTouchingAWall();
        ResetGravity();
        WallSlide();
        GravityModifier();

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
    #region Jump

    public void Jump()
    {
        _jumpUsable = (JumpAvailable > 0) ? true : false;
        if(isSwinging) { isSwinging= false; }
        if (_jumpUsable)
        {
            if (_grounded) { NormalJump(); }
            else if ((Falling || Jumping) && !TouchingWall) { DoubleJump(); }
            else if (_isWallSliding) { WallJump(); }

        }
        JumpAvailable--;

    }
    private void NormalJump()
    {
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
        m_Rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);

    }

    private void DoubleJump()
    {
        m_Rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        float wallJumpingDirection = m_FacingRight? 1 : -1;
        m_Rigidbody.velocity = new Vector2(wallJumpingDirection * _stats.WallJumpForce.x, _stats.WallJumpForce.y);

    }

    #endregion
    private void ResetYSpeed()
    {
        m_Rigidbody.velocity *= Vector2.up;
    }
    private void ResetVelocity()
    {
        if (Physics2D.OverlapCircle(feetPos.position, checkRadius, _iceLayerMask)) return;
        m_Rigidbody.velocity *= 0;
    }
    public void SetWalkingRight(bool isWalkingRight)
    {
        walkingRight = isWalkingRight;
    }
    public void SetWalkingLeft(bool isWalkingLeft)
    {
        walkingLeft = isWalkingLeft;
    }

    #region Physics Checks
    private void IsGrounded()
    {
        bool groundedLive = Physics2D.OverlapCircle(feetPos.position, checkRadius, _groundLayerMask);
        if (groundedLive && _grounded)
        {
            JumpAvailable = 1;
        }
        _grounded = groundedLive;
    }
    private void GravityModifier()
    {
        if (!Falling) { return; }
            m_Rigidbody.AddForce(Vector3.down * _stats.FallGravityForce, ForceMode2D.Force);

    }

    private void DoINeedToFlip()
    {
        if(Mathf.Abs(m_Rigidbody.velocity.x) > 0.1)
        {
            if (m_Rigidbody.velocity.x > 0 && !m_FacingRight)
            {
                Flip();
            }
            if (m_Rigidbody.velocity.x < 0 && m_FacingRight)
            {
                Flip();
            }
        }


    }

    public void JumpRelease()
    {
        m_Rigidbody.gravityScale = 3;     
    }

    private void IsTouchingAWall()
    {
        float extraWidth = 0.1f;
        bool touchingWallLive = false;
        if (m_FacingRight)
        {
            touchingWallLive = Physics2D.Raycast(bc.bounds.center, Vector2.right, bc.bounds.extents.x + extraWidth, _groundLayerMask);
        }
        else
        {
            touchingWallLive = Physics2D.Raycast(bc.bounds.center, -Vector2.right, bc.bounds.extents.x + extraWidth, _groundLayerMask);
        }
        _isTouchingAWall = touchingWallLive;

    }

    private void WallSlide()
    {
        
        if (!_grounded && _isTouchingAWall && m_Rigidbody.velocity.y != 0)
        {
            _isWallSliding = true;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, Mathf.Clamp(m_Rigidbody.velocity.y, -_stats.WallSlidingSpeed, float.MaxValue));
            JumpAvailable++;
        }
        else
        {
            _isWallSliding = false;
        }
    }
    #endregion

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

    #region Fighting Stuff
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
    private void Reload()
    {
        Ammo = 6;
        MenuManager.Instance.HUD.UpdateBullets();
    }

    public void SetShootingToFalse()
    {
        Shooting = false;
    }
    public void TakeDamage(int damage)
    {
        if (Health == 0) { return; }
        Health -= damage;
        MenuManager.Instance.HUD.UpdateHealthBar();
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _dead = true;
        Time.timeScale = 0.5f;
    }

    #endregion
    private void ResetGravity()
    {
        if (!Jumping)
        {
            m_Rigidbody.gravityScale = m_Rigidbody.gravityScale != 1 ? 1 : m_Rigidbody.gravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Hurting = true;
        }
    }
    public void ShootBullet()
    {
        _gunControler.ShootBullet();
    }

    public void SetHurtingToFalse()
    {
        Hurting = false;
    }
    public IEnumerator Dash()
    {
        _canDash = false;
        Dashing = true;
        float originalGravity = m_Rigidbody.gravityScale;
        m_Rigidbody.gravityScale = 0;
        m_Rigidbody.velocity = new Vector2(transform.localScale.x * _stats.DashForce, 0);
        tr.emitting = true;
        yield return new WaitForSeconds(_stats.DashingTime);
        tr.emitting = false;
        m_Rigidbody.gravityScale = originalGravity;
        Dashing = false;
        yield return new WaitForSeconds(_stats.DashingCooldown);
        _canDash = true;
    }

}
