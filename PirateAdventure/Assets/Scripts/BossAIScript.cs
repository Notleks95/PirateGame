using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class BossAIScript : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform targetPlayer;
    public Rigidbody2D targetRB;
    //how close player will be for activation
    public float activeDistance = 50f;
    //how often A* will update
    public float pathUpdateSeconds = 0.5f;


    [Header("Physics")]
    public float maxSpeed = 1500f;
    public float walkAcceleration = 3f;
    //how far enemy is from next waypoint to move in that direction (instead of current)
    public float nextWaypointDistance = 2f;
    //how vertical next node needs to be for enemy to jump
    public float jumpNodeHeightRequirement = 2f;
    //setting how high jump is
    public float jumpModifier = 0.1f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookenabled = true;
    public float walkStopRate = 0.05f;
    public GameObject coinPrefab;
    public DetectionZone attackZone;


    private Path path;
    private Vector2 currentVelocity;
    private int currentWaypoint = 0;
    //bool onGround = true;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Collider2D coll;
    Damageable damageable;
    TouchingDirections touchingDirections;


    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, targetPlayer.transform.position) < activeDistance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        damageable = GetComponent<Damageable>();
        touchingDirections = GetComponent<TouchingDirections>();
        //keeps repeating script based on coroutine based on pathUpdateSeconds (so not running always)
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            if (TargetInDistance() && followEnabled)
            {
                PathFollow();
            }

            anim.SetFloat(AnimationsStrings.yVelocity, rb.velocity.y);
        }        

    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        
        

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private Vector2 startPathPos;

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            //enemy position, target position, 
            Vector2 dot = (Vector2)(targetPlayer.position - transform.position);
            startPathPos = rb.position - (Vector2)(transform.up * transform.localScale.y * 1.27f) + Vector2.right * (Mathf.Sign(dot.x) * 2);
            seeker.StartPath(startPathPos, targetPlayer.transform.position, OnPathComplete);
        }
    }

    public LayerMask Ground;

    private void PathFollow()
    {
        //check path not null and final waypoint isnt over
        if (path == null)
        {
            return;
        }
        //reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        //check if collision (raycast returns true if collision)
        RaycastHit2D isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, Ground);

        //calculate direction
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * maxSpeed * Time.deltaTime;

        //Jump - 
        if (jumpEnabled && isGrounded)
        {
            //direction.y > jumpNodeHeightRequirement
            //float targetYVelocity = 
            if (targetPlayer.position.y - 1f > rb.transform.position.y && targetRB.velocity.y == 0 && path.path.Count < 20)
            {
                //rb.velocity = new Vector2(rb.velocity.x, jumpModifier);
                rb.AddForce(Vector2.up * maxSpeed * jumpModifier);
                //anim.SetTrigger(AnimationsStrings.enemyJump);
            }
        }
        
        //Movement
        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                //rb.AddForce(force);
                IsMoving = force != Vector2.zero;
                if (!isGrounded)
                {
                    force.y = 0;
                    rb.AddForce(force);
                }
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }


        //Next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Direction Graphics handling if we want to flip the sprite
          if (directionLookenabled)
          {
            if (rb.velocity.x > 0.05f)
          {
            transform.localScale = new Vector3( Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
          }
           else if (rb.velocity.x < -0.05f)
          {
            transform.localScale = new Vector3(-1f *Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
          }
        }
    }

    private void OnPathComplete(Path p)
    {
        path = p;
        currentWaypoint = 0;
    }



    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            anim.SetBool(AnimationsStrings.isMoving, value);
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            anim.SetBool(AnimationsStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationsStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return anim.GetFloat(AnimationsStrings.attackCooldown);
        }
        private set
        {
            anim.SetFloat(AnimationsStrings.attackCooldown, Mathf.Max(value, 0));
        }

    }

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                //direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }

            }

            _walkDirection = value;
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left.");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        anim.SetTrigger(AnimationsStrings.hitTrigger);
    }

    public void OnDeath()
    {
        Vector3 coinposition = transform.position;

        //coin appears in place
        Instantiate(coinPrefab, coinposition, Quaternion.identity);
    }
    

}
