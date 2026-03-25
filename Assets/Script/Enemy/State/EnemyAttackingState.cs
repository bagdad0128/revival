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
        pastPlayerPos = enemy.Player.transform;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Timer += Time.deltaTime;
        if (Timer >= savePastPlayerPosTime)
        {
            pastPlayerPos = enemy.Player.transform;
        }
        
        reloadTimer += Time.deltaTime;
        if (reloadTimer >= reloadTime)
        {
            if (enemy.EffectPrefab != null)
            {
                Object.Instantiate(enemy.EffectPrefab, pastPlayerPos.position, enemy.transform.rotation);
            }
            reloadTimer = 0;
        }
        
        float distance = enemy.Player.position.x - enemy.transform.position.x;
        
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
