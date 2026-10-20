using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private Vector2 movementInput;

    public PlayerClassHandler classHandler;


    //Dash
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing;

    private bool canDash = true;

    private bool isAttacking;

    public float attackDuration = 0.5f;

    protected int facingDir = 1;
    protected bool facingRight = true;

    private Vector2 lastNonZeroMovement = Vector2.down;

    

    protected override void Update()
    {   
        
        base.Update();
        if (isDashing) return;
        HandleInput();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (isDashing || !canMove) return;

        rb.linearVelocity = movementInput * SPEED;
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

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && movementInput != Vector2.zero && canDash) {

            Debug.Log($"Kliknięto Shift! Czy canDash: {canDash}, Czy moveInput to zero: {movementInput == Vector2.zero}");

            if (canDash && movementInput != Vector2.zero)
            {
                StartCoroutine(DashRoutine());
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleAttack();

        }


        if (Keyboard.current.qKey.isPressed)
        {
            CastSpell(0);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            CastSpell(0);
        }
    }

    private void CastSpell(int slotIndex)
    {
        if (classHandler != null && slotIndex < classHandler.equippedSpells.Count)
        {
            SpellData spellToCast = classHandler.equippedSpells[slotIndex];
            if (spellToCast != null)
            {
                spellToCast.Cast(transform);
            }
        }
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint != null)
        {
            attackPoint.position = (Vector2)transform.position + (lastNonZeroMovement * attackOffset);
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Vector2 dashDirection = movementInput;

        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    protected override void HandleMovement()
    {
        if (isDashing) return;
        if (canMove)
        {
            rb.linearVelocity = movementInput * SPEED;


            anim.SetBool("IsWalking", true);
            anim.SetFloat("InputX", movementInput.x);
            anim.SetFloat("InputY", movementInput.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
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

    protected virtual void HandleAttack()
    {
        canMove = false;
        if (anim != null)
        {
            anim.SetTrigger("attack");
            
        }
        DamageTargets();
    }

    public void EndAttack()
    {
        canMove = true;
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
