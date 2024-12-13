using System.ComponentModel;

internal sealed class GetTokenCommand : AsyncCommand<GetTokenCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<tenantId>")]
        public required Guid TenantId { get; set; }

        [CommandArgument(1, "<clientId>")]
        public required Guid ClientId { get; set; }

        [CommandOption("-s|--scope")]
        [DefaultValue(".default")]
        public string? Scope { get; set; }
    }


    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var app = PublicClientApplicationBuilder.Create(settings.ClientId.ToString())
                                               .WithTenantId(settings.TenantId.ToString())
                                               .WithDefaultRedirectUri()
                                               .Build();
        
        var scope = string.IsNullOrWhiteSpace(settings.Scope) ? ".default" : settings.Scope;

        var result = await app.AcquireTokenInteractive([$"{settings.TenantId}/{scope}"]).ExecuteAsync();
        
        AnsiConsole.MarkupLine($"{Environment.NewLine}[purple]{result.AccessToken}[/]");

        return 0;
    }
}

