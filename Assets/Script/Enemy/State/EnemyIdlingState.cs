using UnityEngine;

public class EnemyIdlingState : EnemyBaseState
{
    private Vector2 startPos;
    private bool isInitialPosSet = false;
    private bool isNotStartPos = false;

    private float speed = 5f;
    private float patrolRange = 2f;
    private int direction = 1;

    private float waitTimer;
    private float waitTime = 2;
    private bool isWaiting = false;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        if (!isInitialPosSet)
        {
            startPos = enemy.transform.position;
            isInitialPosSet = true;
        }

        float diffX = enemy.transform.position.x - startPos.x;
        
        if (diffX * diffX > 0.01f)
        {
            isNotStartPos = true;
        }
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        float detectionRange = 5f;
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        Vector2 rayDirection = new Vector2(direction, 0);
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, rayDirection, detectionRange, playerLayerMask);
        
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                enemy.SwitchState(enemy.DetectingState);
                return;
            }
        }

        if (isNotStartPos) return;
        
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0;
                
                direction *= -1;
                Vector3 scale = enemy.transform.localScale;
                scale.x = ((scale.x < 0) ? -scale.x : scale.x) * direction;
                enemy.transform.localScale = scale;
            }

            return;
        }
        
        float distanceFromStart = enemy.transform.position.x - startPos.x;

        if (Mathf.Abs(distanceFromStart) >= patrolRange)
        {
            if (distanceFromStart > 0 && direction > 0 || distanceFromStart < 0 && direction < 0)
            {
                isWaiting = true;
                enemy.Rb.linearVelocity = new Vector2(0, enemy.Rb.linearVelocity.y);
            }
        }
    }

    public override void FixedUpdateState(EnemyStateManager enemy)
    {
        if (isNotStartPos)
        {
            float diffX = startPos.x - enemy.transform.position.x;
            float returnDir = (diffX > 0) ? 1f : -1f;
            enemy.Rb.linearVelocity = new Vector2(returnDir * speed, enemy.Rb.linearVelocity.y);
            
            if (diffX * diffX < 0.01f)
            {
                enemy.transform.position = new Vector3(startPos.x, enemy.transform.position.y, enemy.transform.position.z);
                isNotStartPos = false;
                enemy.Rb.linearVelocity = new Vector2(0, enemy.Rb.linearVelocity.y);

                direction = 1;
                Vector3 scale = enemy.transform.localScale;
                scale.x = ((scale.x < 0) ? -scale.x : scale.x) * direction;
                enemy.transform.localScale = scale;
            }
        }
        else if (!isWaiting)
        {
            enemy.Rb.linearVelocity = new Vector2(direction * speed, enemy.Rb.linearVelocity.y);
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.Rb.linearVelocity = Vector2.zero;
    }
}
