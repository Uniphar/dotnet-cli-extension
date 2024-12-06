namespace Uniphar.Dotnet.Cli.Token;

internal class GetTokenCommand : AsyncCommand<GetTokenCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<tenantId>")]
        public required string TenantId { get; set; }

        [CommandArgument(1, "<clientId>")]
        public required string ClientId { get; set; }

        public override ValidationResult Validate()
        {
            return string.IsNullOrEmpty(TenantId) || string.IsNullOrEmpty(ClientId)
                ? ValidationResult.Error("Tenant id and application client id are required.")
                : ValidationResult.Success();
        }
    }

    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var tenantId = settings.TenantId;
        var clientId = settings.ClientId;

        try
        {
            string token = await (new TokenService()).GetAccessTokenAsync(tenantId, clientId);
            AnsiConsole.MarkupLine($"{Environment.NewLine}[purple]{token}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }

        return 0;
    }
}

