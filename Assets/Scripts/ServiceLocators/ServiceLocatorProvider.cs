using AMARI.Assets.Scripts;

public class ServiceLocatorProvider : SingletonMonoBehaviour<ServiceLocatorProvider>
{
    public ServiceLocator generalCurrent{ get; private set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        generalCurrent = new ServiceLocator();
    }
}