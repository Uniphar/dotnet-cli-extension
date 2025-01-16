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
        [DefaultValue(null)]
        public string? Scope { get; set; }

        [CommandOption("--authority")]
        [DefaultValue("ms")]
        public string? Authority { get; set; }
    }


    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var tenantId = settings.TenantId.ToString();
        var clientId = settings.ClientId.ToString();
        var authority = settings.Authority ?? "ms";

        var (authorityUri, isPredefinedAuthority) = authority.ToLower() switch
        {
            "ms" => ($"https://login.microsoftonline.com/{tenantId}", true),
            "ciam" => ($"https://{tenantId}.ciamlogin.com", true),
            _ when Uri.TryCreate(authority, UriKind.Absolute, out _) => (authority, false),
            _ => throw new Exception("Invalid authority URI")
        };

        if (!isPredefinedAuthority)
            AnsiConsole.MarkupLine("[bold yellow]Warning:[/] Using an arbitrary URI for authority is temporary. Please submit a PR to add your authority (currently supported: ms, ciam).");

        var app = PublicClientApplicationBuilder.Create(clientId)
                                                .WithTenantId(tenantId)
                                                .WithAuthority(authorityUri)
                                                .WithDefaultRedirectUri()
                                                .Build();

        var scope = string.IsNullOrWhiteSpace(settings.Scope) ? $"{clientId}/.default" : settings.Scope;

        AnsiConsole.MarkupLine($"[bold][steelblue1]- TenantId    : [/][/][italic]{tenantId}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- ClientId    : [/][/][italic]{clientId}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- AuthorityId : [/][/][italic]{authorityUri}[/]");
        AnsiConsole.MarkupLine($"[bold][steelblue1]- Scope       : [/][/][italic]{scope}[/]");
        AnsiConsole.WriteLine();

        var result = await app.AcquireTokenInteractive([scope]).ExecuteAsync();

        using (ConsoleEx.SetColors(foreground: ConsoleColor.DarkGreen))
            Console.WriteLine(result.AccessToken); // We are using Console because AnsiConsole is adding line breaks to the output

        return 0;
    }
}