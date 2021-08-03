using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region enums

    public enum Team
    {
        Red,
        Blue
    }

    public enum Facing
    {
        Right = 1,
        Left = -1
    }

    #endregion

    public PlayerControllerBase Controller = new PlayerControllerBase();

    public Facing PlayerFacing;

    public Team PlayerTeam;

    [SerializeField]
    private float RunForce;

    [SerializeField]
    private float MaxRunSpeed;

    [SerializeField]
    private float JumpForce;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite IdleSprite;

    [SerializeField]
    Sprite AttackSprite;

    [SerializeField]
    BoxCollider2D HitBox;

    [SerializeField]
    float KickForce;

    private Rigidbody2D body;

    private float height;

    public bool OnGround { get; private set; }

    public bool Attacking { get; private set; }

    public Action ResetPosition { get; private set; }

    private void Awake()
    {
        this.body = GetComponent<Rigidbody2D>();

        var collider = GetComponent<CapsuleCollider2D>();
        this.height = collider.size.y;

        var startPos = this.body.position;
        var startFacing = this.PlayerFacing;

        ResetPosition = () =>
        {
            this.body.position = startPos;
            this.body.velocity = Vector2.zero;
            SetAttacking(false);
            UpdateFacingDirection((int)startFacing);
        };
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static readonly int GroundPhysicsLayerMask = 1 << 7;

    float dragFactor = 0.92f;

    float lastAttackTime;

    readonly float attackLength = 0.8f;

    private void UpdateFacingDirection(float inputDirection)
    {
        if(Attacking || !OnGround || inputDirection == 0)
        {
            return;
        }

       if(inputDirection < 0)
        {
            this.PlayerFacing = Facing.Left;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            this.PlayerFacing = Facing.Right;
            this.transform.eulerAngles = Vector3.zero;
        }
    }

    private void SetAttacking(bool attacking)
    {
        this.Attacking = attacking;
        this.spriteRenderer.sprite = attacking ? this.AttackSprite : this.IdleSprite;
        this.HitBox.enabled = attacking;
        if (attacking)
        {
            lastAttackTime = Time.timeSinceLevelLoad;
        }

    }

    private void FixedUpdate()
    {
        this.Controller.UpdateController();

        // raycast to see if touching ground
        var cast = Physics2D.Raycast(this.body.position, Vector2.down, height * 0.52f, GroundPhysicsLayerMask);
        this.OnGround = cast.collider;

        // resolve running
        var horzForce = RunForce * this.Controller.StickValueX;
        if (!this.OnGround)
        {
            horzForce /= 2;
        }
        if (!Attacking)
        {
            this.body.AddForce(Vector2.right * horzForce, ForceMode2D.Force);
        }
        UpdateFacingDirection(this.Controller.StickValueX);

        var limitedVel = this.body.velocity;
        if (this.OnGround)
        {
            limitedVel.x *= dragFactor;
        }

        if (Mathf.Abs(this.body.velocity.x) > this.MaxRunSpeed)
        {
            limitedVel.x = this.MaxRunSpeed * (limitedVel.x < 0 ? -1 : 1);
        }
        this.body.velocity = limitedVel;

        //resolve jumping
        if (!Attacking && this.OnGround && this.Controller.JumpButton.WasPressedThisFrame)
        {
            this.body.AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);
        }

        //resolve attacking
        if(!Attacking && this.Controller.AttackButton.WasPressedThisFrame)
        {
            SetAttacking(true);
        }
        else if(Attacking && Time.timeSinceLevelLoad - this.lastAttackTime >= attackLength)
        {
            SetAttacking(false);
        }

        
    }

    // hit something with attack
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.isTrigger)
        {
            return;
        }
        var kickDir = new Vector2((int)this.PlayerFacing, 0.4f).normalized;
        collision.attachedRigidbody.AddForce(kickDir * this.KickForce, ForceMode2D.Impulse);
    }

}
