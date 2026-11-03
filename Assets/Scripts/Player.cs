using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private Vector2 movementInput;
    
    public float attackDuration = 0.5f;

    [Header("Class")]
    public PlayerClassHandler classHandler;
    


    //Dash
    
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    
	
    private bool isDashing;

    private bool canDash = true;

    private bool isAttacking;

    

    protected int facingDir = 1;
    protected bool facingRight = true;

    private Vector2 lastNonZeroMovement = Vector2.down;

    protected override void Awake()
    {
    base.Awake();
    
    classHandler = GetComponent<PlayerClassHandler>();
    
    if(classHandler == null){
	Debug.LogError("Brak komponentu PlayerClassHandler na obiekcie gracza!");    
    }
    
    }

    protected override void Update()
    {   
        
        base.Update();
        if (isDashing) return;
        //HandleInput();
        
        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (isDashing || !canMove) return;

        HandleMovement();
    }

    //private void OnMove(InputValue value)
    //{
    //    movementInput = value.Get<Vector2>();
    //    Debug.Log("Ruch: " + movementInput);
    //}

    private void HandleInput()
    {
        if (Keyboard.current == null) return;

        float moveX = 0;
        float moveY = 0;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1;

        movementInput = new Vector2(moveX, moveY).normalized;

        if(movementInput != Vector2.zero)
        {
            lastNonZeroMovement = movementInput;
            anim.SetBool("IsWalking", false);
            anim.SetFloat("LastInputX", moveX);
            anim.SetFloat("LastInputY", moveY);
            UpdateAttackPointPosition();
        }

        

       


        
    }

    

    public IEnumerator DashRoutine(float potency, float duration)
    {
        canDash = false;
        isDashing = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        Vector2 dashDirection = movementInput;
        rb.linearVelocity = dashDirection * potency;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        
        canDash = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint != null)
        {
            attackPoint.position = (Vector2)transform.position + (lastNonZeroMovement * attackOffset);
        }
    }

    

    protected override void HandleMovement()
    {
        if (isDashing) return;
        if (canMove)
        {
            rb.linearVelocity = movementInput * SPEED;

            if (movementInput != Vector2.zero)
            {
                lastNonZeroMovement = movementInput;
                anim.SetBool("IsWalking", false);
                anim.SetFloat("InputX", movementInput.x);
                anim.SetFloat("InputY", movementInput.y);

                
                
                anim.SetFloat("LastInputX", movementInput.x);
                anim.SetFloat("LastInputY", movementInput.y);

                UpdateAttackPointPosition();
                
            }
            else
            {
                anim.SetBool("IsWalking", true);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            
        }
    }

    protected void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();

        if (movementInput == Vector2.zero)
        {
            anim.SetBool("IsWalking", false);
        }
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadious, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage(this.DMG);
        }
    }

    

    public void EndAttack()
    {
        canMove = true;
    }

    protected void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            canMove = false;
            if (anim != null)
            {
                anim.SetTrigger("attack");

            }
            DamageTargets();
        }
    }

    public void OnCastSpell_1(InputValue value)
    {
        if (value.isPressed) 
        {
            Debug.Log("Próba rzucenia czaru ze slotu 1");

            if (classHandler != null)
            {
                classHandler.CastSpellFromSlot(0);
            }
        }
    }

    public void OnCastSpell_2(InputValue value)
    {
        if (value.isPressed) 
        {
            Debug.Log("Próba rzucenia czaru ze slotu 2");


            if (classHandler != null)
            {
                classHandler.CastSpellFromSlot(1);
            }
        }
    }


    protected virtual void HandleFlip()
    {
        if (rb.linearVelocityX > 0 && facingRight == false)
            Flip();
        else if (rb.linearVelocityX < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }
}
