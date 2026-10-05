using System;
using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;
    protected Transform playerTransform;
    [SerializeField] protected LayerMask Player;

    public bool isRoomActive = false;
    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        FindPlayerViaLayer();
    }

    private void FindPlayerViaLayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, 50f, Player);

        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform;
        }
        else
        {
            Debug.LogWarning($"Nie znaleziono Gracza na warstwie {Player.value} w pobliżu {gameObject.name}!");
        }
    }

    void Update()
    {
        base.Update();
    }

    protected override void HandleMovement()
    {
        
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadious, whatIsTarget);
    }

    protected override void Die()
    {
        base.Die();
    }

    //protected virtual void CollisionDamage(Collider2D collision)
    //{
    //    if ((Player.value & (1 << collision.gameObject.layer)) != 0)
    //    {
    //        Entity playerEntity = collision.gameObject.GetComponent<Entity>();

    //        if (playerEntity != null)
    //        {
    //            playerEntity.TakeDamage(this.DMG);
    //        }
    //    }
    //}

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // TEST 1: Czy Unity w ogóle widzi jakiekolwiek zderzenie szlama?
        Debug.Log($"[TEST 1] Szlam fizycznie dotknął obiektu: {collision.gameObject.name} na warstwie numer: {collision.gameObject.layer}");

        // Sprawdzenie bitowe
        bool czyToWarstwaGracza = (Player.value & (1 << collision.gameObject.layer)) != 0;

        // TEST 2: Czy warstwa uderzonego obiektu zgadza się z tym, co wybrałeś w Inspektorze?
        Debug.Log($"[TEST 2] Czy ten obiekt należy do maski PlayerLayer? Odpowiedź: {czyToWarstwaGracza}");

        if (czyToWarstwaGracza)
        {
            Entity playerEntity = collision.gameObject.GetComponent<Entity>();

            // TEST 3: Czy udało się pobrać skrypt Entity z gracza?
            Debug.Log($"[TEST 3] Czy obiekt posiada skrypt Entity? Odpowiedź: {playerEntity != null}");

            if (playerEntity != null)
            {
                playerEntity.TakeDamage(this.DMG);
            }
        }
    }
}
