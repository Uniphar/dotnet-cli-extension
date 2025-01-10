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

        [CommandOption("--useCIAM")]
        [DefaultValue("false")]
        public bool? UseCIAM { get; set; }
    }


    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var authorityUri = (settings.UseCIAM ?? false) ? "https://{0}.ciamlogin.com" : "https://login.microsoftonline.com/{0}";
        var tenantId = settings.TenantId.ToString();
        var clientId = settings.ClientId.ToString();

        var app = PublicClientApplicationBuilder.Create(clientId)
                                                .WithTenantId(tenantId)
                                                .WithAuthority(string.Format(authorityUri, tenantId))
                                                .WithDefaultRedirectUri()
                                                .Build();
        
        var scope = string.IsNullOrWhiteSpace(settings.Scope) ? ".default" : settings.Scope;

        AnsiConsole.MarkupLine($"[bold][steelblue1]- TenantId    : [/][/][italic]{tenantId}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- ClientId    : [/][/][italic]{clientId}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- AuthorityId : [/][/][italic]{authorityUri}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- Scope       : [/][/][italic]{scope}[/]");


        var result = await app.AcquireTokenInteractive([$"{clientId}/{scope}"]).ExecuteAsync();

        AnsiConsole.MarkupLine($"{Environment.NewLine}[purple]{result.AccessToken}[/]");

        return 0;
    }
}

