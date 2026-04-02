using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetectingState : EnemyBaseState
{
    private float speed = 5f;
    private float distance;
    private bool isDetecting = false;

    public override void EnterState(EnemyStateManager enemy)
    {
        isDetecting = true;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.player is not null)
        {
            distance = enemy.player.position.x - enemy.transform.position.x;

            if (distance * distance >= 36f)
            {
                isDetecting = false;
                enemy.SwitchState(enemy.IdlingState);
            }
        }
        
        if (distance * distance < 2.25f)
        {
            isDetecting = false;
            enemy.SwitchState(enemy.AttackingState);
        }
    }

    public override void FixedUpdateState(EnemyStateManager enemy)
    {
        if (isDetecting)
        {
            float moveDirection = distance > 0 ? 1f : -1f;
            enemy.Rb.linearVelocity = new Vector2(moveDirection * speed, enemy.Rb.linearVelocity.y);
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.Rb.linearVelocity = Vector2.zero;
    }
}
