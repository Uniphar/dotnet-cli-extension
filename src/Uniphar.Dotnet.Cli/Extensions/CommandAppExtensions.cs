namespace Uniphar.Dotnet.Cli.Extensions;

public static class CommandAppExtensions
{
    public static async Task<int> ConfigureAndRunAsync(this CommandApp app, IEnumerable<string> args, Action<IConfigurator> configurator)
    {
        app.Configure(configurator);
        return await app.RunAsync(args);
    }
}