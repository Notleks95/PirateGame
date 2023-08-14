using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 10f;
    public float airWalkSpeed = 5f;
    public float jumpImpulse = 10f;
    //public GameObject characterPlayer;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    Rigidbody2D rb;
    Animator animator;
    //CharacterDiedScript characterDiedScript;



    public float CurrentMoveSpeed
    { get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        //AirMove
                        return airWalkSpeed;
                    }   
                }
                else
                {
                    //idle speed is 0
                    return 0;
                }
            }
            else
            {
                //movement locked
                return 0;
            }
            
            
        }
    }


    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationsStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationsStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight 
    { 
        get 
        { 
            return _isFacingRight; 
        }
        private set 
        {
            if (_isFacingRight != value)
            {
                //flip local scale to make player face other direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        } }

    public bool CanMove 
    {
        get
        {
            return animator.GetBool(AnimationsStrings.canMove);
        }
    }

    public bool _isAlive = true;
    public bool IsAlive
    { 
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.GetBool(AnimationsStrings.isAlive);
        }
    }

    //public bool IsAlive
    //{
    //    get
    //    {
    //        return animator.GetBool(AnimationsStrings.isAlive);
    //    }
   // }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        

    }

   
    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
            

        animator.SetFloat(AnimationsStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
            

        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //Face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            //Face left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        } else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    //could add double jump here later
    public void OnJump(InputAction.CallbackContext context)
    {
        //todo check if alive too
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationsStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationsStrings.attackTrigger);
        }
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    void Update()
    {
        if (animator.GetBool(AnimationsStrings.isAlive) == false)
        {
            animator.SetBool(AnimationsStrings.isAlive, true);
            StartCoroutine(OnDeath());
            
        }
            
    }
    //IsAlive = false;
      //          IsDead = true;
    public CharacterDiedScript characterDied;


    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(2);
        transform.position = InteractionScript.respawnPoint;
        animator.SetBool(AnimationsStrings.lockVelocity, false);
        animator.SetBool(AnimationsStrings.isAlive, true);
        characterDied.RespawnDeath();
            
        //characterDied.hasRespawned = false;
        yield break;
        
        
    }
}
