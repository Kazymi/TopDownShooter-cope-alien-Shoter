public class PuduSpawner : DamageableSpawner
{
    private void OnEnable()
    {
        ServiceLocator.Subscribe<PuduSpawner>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.UnSubscribe<PuduSpawner>();
    }
}