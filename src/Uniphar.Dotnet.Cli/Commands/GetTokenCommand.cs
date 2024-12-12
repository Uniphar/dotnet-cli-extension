namespace Uniphar.Dotnet.Cli.Token;

internal sealed class GetTokenCommand : AsyncCommand<GetTokenCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<tenantId>")]
        public required Guid TenantId { get; set; }

        [CommandArgument(1, "<clientId>")]
        public required Guid ClientId { get; set; }

    }

    private const string Scope = ".default";

    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var tenantId = settings.TenantId.ToString();
        var clientId = settings.ClientId.ToString();

        var app = PublicClientApplicationBuilder.Create(settings.ClientId.ToString())
                                               .WithTenantId(settings.TenantId.ToString())
                                               .WithDefaultRedirectUri()
                                               .Build();

        var result = await app.AcquireTokenInteractive([$"{clientId}/{Scope}"]).ExecuteAsync();
        AnsiConsole.MarkupLine($"{Environment.NewLine}[purple]{result.AccessToken}[/]");

        return 0;
    }
}

