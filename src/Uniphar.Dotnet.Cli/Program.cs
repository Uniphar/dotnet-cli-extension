global using Microsoft.Identity.Client;
global using Spectre.Console;
global using Spectre.Console.Cli;


using Uniphar.Dotnet.Cli.Token;


CommandApp app = new();

app.Configure(config =>
{
    config.SetApplicationName("uniphar");
    config.AddCommand<GetTokenCommand>("get-token");
});

try
{
    return app.Run(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return -1;
}