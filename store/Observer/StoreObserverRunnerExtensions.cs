namespace BlogAtor.Store.Observer;

using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

public static class StoreObserverRunnerExtensions
{
    public static void AddStoreObserver(this Runner runner)
    {
        runner.AddModule<IStoreObserver, StoreObserver>();
    }
}