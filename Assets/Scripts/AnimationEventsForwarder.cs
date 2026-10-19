using UnityEngine;

public class AnimationEventsForwarder : MonoBehaviour
{
    public Player skryptGracza;

    public void ForwardEndAttack()
    {
        skryptGracza.EndAttack();
    }
}