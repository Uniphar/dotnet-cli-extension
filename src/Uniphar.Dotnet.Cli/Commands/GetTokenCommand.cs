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

        [CommandOption("--useCIAM")]
        [DefaultValue("false")]
        public bool? UseCIAM { get; set; }
    }


    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var tenantId = settings.TenantId.ToString();
        var clientId = settings.ClientId.ToString();
        var authorityUri = (settings.UseCIAM ?? false) ? $"https://{tenantId}.ciamlogin.com" : $"https://login.microsoftonline.com/{tenantId}";

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


        var result = await app.AcquireTokenInteractive([scope]).ExecuteAsync();

        //Access token is long and MarkupLine breaks the output in multiple lines with new line character
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"{Environment.NewLine}{result.AccessToken}");
        Console.ForegroundColor = color;

        return 0;
    }
}

