public class AllServices
{
    public static AllServices Container => _instens ?? (_instens = new AllServices());

    private static AllServices _instens;

    internal void RegisterSingle<TService>(TService implementation) where TService : IService =>
        Impelintation<TService>.SetviceInstance = implementation;

    internal TService Single<TService>() where TService : IService =>
        Impelintation<TService>.SetviceInstance;

    private static class Impelintation<TService> where TService : IService
    {
        public static TService SetviceInstance;
    }
}
