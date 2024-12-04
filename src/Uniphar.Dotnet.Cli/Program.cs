global using Microsoft.Identity.Client;

using System.CommandLine;
using Uniphar.Dotnet.Cli;
using Uniphar.Dotnet.Cli.Token;


RootCommand rootCommand = new()
{
    Description = "A .NET CLI tool to support and simplify your development and testing processes",
    Name = "uniphar"
};

var tenantIdOption = new Option<string>(["-t", "--tenantId"], "Tenant id") { IsRequired = true };
var clientIdOption = new Option<string>(["-c", "--clientId"], "App client id") { IsRequired = true };
var getTokenCommand = new Command("get-token", "Obtains a token for a specific user from an Entra application.") {
    tenantIdOption,
    clientIdOption
};

getTokenCommand.SetHandler(async (tenantId, clientId) =>
{
    try
    {
        if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Tenant id and application client id are required.");
        }

        Console.WriteLine($"{ConsoleColors.Yellow}Tenant id: {tenantId}");
        Console.WriteLine($"{ConsoleColors.Yellow}Application client id: {clientId}");

        string token = await (new TokenService()).GetAccessTokenAsync(tenantId, clientId);

        Console.WriteLine($"{ConsoleColors.Green}Authentication successful!");
        Console.WriteLine($"{ConsoleColors.Yellow}Token: {ConsoleColors.Magenta}{token}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"{ConsoleColors.Red}{ex.Message}");
    }
}, tenantIdOption, clientIdOption);

rootCommand.AddCommand(getTokenCommand);

return await rootCommand.InvokeAsync(args);