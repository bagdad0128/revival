public class PlayerEffect : EffectBase
{
    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
