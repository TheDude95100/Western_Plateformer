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
    [SerializeField] TrailRenderer tr;
    BoxCollider2D bc;

    internal int Health { get; private set; }
    internal int Ammo { get; private set; }

    internal int JumpAvailable { get; private set; }



    internal int Walking { get; private set; }

    bool _grounded, _jumpUsable, _isTouchingAWall, _isJumping, _dead, _canDash, _isWallSliding;

    bool walkingRight, walkingLeft;
    public bool Falling => m_Rigidbody.velocity.y < 0;
    public bool Jumping => m_Rigidbody.velocity.y > 0;

    public PlayerInput playerInput { get; private set; }
    public bool isWalking => Walking != 0;
    public bool Shooting { get; private set; }
    public bool Dashing { get; private set; }

    public bool TouchingWall => _isTouchingAWall;

    public bool IsJumping => _isJumping;
    public bool Dead => _dead;

    internal int LastDirectionFaced { get; set; }

    internal bool m_FacingRight = true;
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        LastDirectionFaced = 1;
        Health = 4;
        Ammo = 6;
        JumpAvailable = 2;
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
            if (Walking != LastDirectionFaced)
            {
                Flip();
            }
        }

    }

    void FixedUpdate()
    {
        if (Dashing) { return; }
        IsGrounded();
        IsTouchingAWall();
        ResetGravity();
        WallSlide();
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
        Debug.Log("Jump");
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
        m_Rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);

    }

    private void DoubleJump()
    {
        Debug.Log("Double Jump");
        m_Rigidbody.AddForce(Vector2.up * _stats.JumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        Debug.Log("Walljump");
        float wallJumpingDirection = LastDirectionFaced;
        m_Rigidbody.velocity = new Vector2(wallJumpingDirection * _stats.WallJumpForce.x, _stats.WallJumpForce.y);
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
            JumpAvailable = 2;
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
        _jumpUsable = (JumpAvailable > 0) ? true : false;
        if (_jumpUsable)
        {
            if (_grounded) { NormalJump(); }
            else if ((Falling || Jumping) && !TouchingWall) { DoubleJump(); }
            else if(_isWallSliding) { WallJump(); }

        }
        JumpAvailable--;
    }

    private void WallSlide()
    {
        if (!_grounded && _isTouchingAWall && m_Rigidbody.velocity.y !=0)
        {
            _isWallSliding= true;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, Mathf.Clamp(m_Rigidbody.velocity.y, -_stats.WallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
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
        this.enabled = false;
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

    private void ResetYSpeed()
    {
        m_Rigidbody.velocity = Vector2.up * 0;
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
