using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;


    [Header("Statistics")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currenHP;
    [SerializeField] protected float DMG;
    [SerializeField] protected float DEF;
    [SerializeField] protected float SPEED;


    [Header("Collision")]
    [SerializeField] private bool ObstacleDetection;
    [SerializeField] private LayerMask Obstacles;


    [Header("Attack details")]
    [SerializeField] protected float attackRadious;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected float attackOffset;



    protected bool canMove = true;

    protected virtual void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        currenHP = maxHP;

    }

    protected virtual void Update()
    {
        HandleMovement();
    }

    

    public void TakeDamage(float incomingDMG)
    {
        float calDMG = DMG - DEF;

        float totalDMG = Mathf.Max(1, calDMG);

        currenHP -= totalDMG;

        if (currenHP <= 0) Die();
    }

    

    protected virtual void HandleMovement() { }

    public virtual void EnableMovement(bool enable)
    {

        canMove = enable;
    }

    protected virtual void HandleCollision() { }


    protected virtual void Die()
    {
        if (anim != null)  anim.enabled = false;
        col.enabled = false;

        Destroy(gameObject, 1);
    }

    private void OnDrawGizmos()
    {

        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRadious);
    }
}
