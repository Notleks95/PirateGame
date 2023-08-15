using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class BossAIScript : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform targetPlayer;
    //how close player will be for activation
    public float activeDistance = 50f;
    //how often A* will update
    public float pathUpdateSeconds = 0.5f;

    
    [Header("Physics")]
    public float speed = 20f;
    //how far enemy is from next waypoint to move in that direction (instead of current)
    public float nextWaypointDistance = 3f;
    //how vertical next node needs to be for enemy to jump
    public float jumpNodeHeightRequirement = 0.8f;
    //setting how high jump is
    public float jumpModifier = 0.1f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookenabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Collider2D coll;

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
        //keeps repeating script based on coroutine based on pathUpdateSeconds (so not running always)
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private Vector2 startPathPos;

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            //enemy position, target position, 
            //seeker.StartPath(rb.position, target.position, OnPathComplete);
            Vector2 dot = (Vector2)(targetPlayer.position - transform.position);
            startPathPos = rb.position - (Vector2)(transform.up * transform.localScale.y * 1.27f) + Vector2.right * (Mathf.Sign(dot.x) * 2);
            seeker.StartPath(startPathPos, targetPlayer.transform.position, OnPathComplete);
        }
    }

    public LayerMask Ground;

    private void PathFollow()
    {
        //check path not null and final waypoint isnt over
        if (path ==null)
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
        Vector2 force = direction * speed * Time.deltaTime;

        //Jump - 
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        //Movement
        rb.AddForce(force);

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
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
    

    private void OnPathComplete(Path p)
    {
        path = p;
            currentWaypoint = 0;
        //if (!path.error)
        //{
            
        //}
    }
}
