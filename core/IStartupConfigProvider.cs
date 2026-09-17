namespace BlogAtor.Core;

using BlogAtor.Core.Config;

public interface IStartupConfigProvider
{
    public StartupConfig StartupConfig { get; }
}