using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private float reloadTimer;
    private float reloadTime = 4f;

    private Transform pastPlayerPos;
    private float savePastPlayerPosTime = 3f;
    private float Timer;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        pastPlayerPos = enemy.player.transform;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Timer += Time.deltaTime;
        if (Timer >= savePastPlayerPosTime)
        {
            pastPlayerPos = enemy.player.transform;
        }
        
        reloadTimer += Time.deltaTime;
        if (reloadTimer >= reloadTime)
        {
            if (enemy.effectPrefab is not null)
            {
                Object.Instantiate(enemy.effectPrefab, pastPlayerPos.position, enemy.transform.rotation);
            }
            reloadTimer = 0;
        }
        
        float distance = enemy.player.position.x - enemy.transform.position.x;
        
        if (distance * distance >= 64)
        {
            enemy.SwitchState(enemy.DetectingState);
        }
    }

    public override void FixedUpdateState(EnemyStateManager enemy)
    {
        
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        
    }
}
