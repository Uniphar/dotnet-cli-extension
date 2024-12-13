return await new CommandApp().ConfigureAndRunAsync(args, config =>
{
    config.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything));
    config.SetApplicationName("uniphar");
    config.AddCommand<GetTokenCommand>("get-token");
});