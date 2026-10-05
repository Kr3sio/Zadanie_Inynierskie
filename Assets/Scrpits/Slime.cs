using System;
using System.Collections;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpDuration = 0.8f;
    [SerializeField] private float chargeDuration = 0.3f;
    [SerializeField] private float restDuration = 1.5f;

    private bool isBehaviorRunning = false;

    private void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoomActive && playerTransform != null && !isBehaviorRunning)
        {
            StartCoroutine(SlimeBehaviorRoutine());
        }

    }

    private IEnumerator SlimeBehaviorRoutine()
    {
        isBehaviorRunning = true;

        while(isRoomActive && playerTransform != null)
        {   //odpoczynek
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(restDuration);

            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            //ładowanie
            if (anim != null) anim.SetTrigger("charge");
            yield return new WaitForSeconds(chargeDuration);

            //skok
            if (anim != null) anim.SetTrigger("jump");

            rb.linearVelocity = directionToPlayer * jumpForce;
            yield return new WaitForSeconds(jumpDuration);

            rb.linearVelocity = Vector2.zero;
        }
        isBehaviorRunning = false;
    }
}
