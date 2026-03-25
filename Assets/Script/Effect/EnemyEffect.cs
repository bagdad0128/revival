using UnityEditor;
using UnityEngine;

public class EnemyEffect : EffectBase
{
    private Vector3 originalSize;
    private float waitTime = 0.01f;
    private float timer;

    private GameObject effectPrefab;

    private void Awake()
    {
        originalSize = transform.localScale;
        effectPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefab/EffectEnemy.prefab", typeof(GameObject));
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(4f, 4f, 1f);
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            transform.localScale -= new Vector3(0.08f, 0.08f, 0);
            if (transform.localScale.x <= originalSize.x)
            {
                transform.localScale = originalSize;
                Instantiate(effectPrefab, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
            timer = 0;
        }
    }
}
