using UnityEditor;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyBaseState _currentState;
    public EnemyDetectingState DetectingState = new EnemyDetectingState();
    public EnemyAttackingState AttackingState = new EnemyAttackingState();
    public EnemyIdlingState IdlingState = new EnemyIdlingState();
    
    public Transform Player;

    public GameObject EffectPrefab;
    
    public Rigidbody2D Rb;

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Player = player.transform;
        }
        if (EffectPrefab != null)
        {
            EffectPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefab/NoticeEffect.prefab", typeof(GameObject));
        }
    }
    
    void Start()
    {
        _currentState = IdlingState;
        _currentState.EnterState(this);
    }
    
    void Update()
    {
        _currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        _currentState?.ExitState(this);
        _currentState = state;
        state.EnterState(this);
    }
}
