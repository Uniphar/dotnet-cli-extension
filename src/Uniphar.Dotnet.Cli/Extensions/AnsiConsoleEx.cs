public static class AnsiConsoleEx
{
    public static ConsoleColorReset SetColors(Color? foreground = default, Color? background = default)
    {
        return new ConsoleColorReset(foreground, background);
    }

    public static void WritePlainLine(string? value) => Console.WriteLine(value);


    public readonly struct ConsoleColorReset : IDisposable
    {
        private Color Foreground { get; }
        private Color Background { get; }

        public ConsoleColorReset(Color? newForeground = default, Color? newBackground = default)
        {
            Foreground = AnsiConsole.Foreground;
            Background = AnsiConsole.Background;

            AnsiConsole.Foreground = newForeground ?? Foreground;
            AnsiConsole.Background = newBackground ?? Background;
        }

        public void Dispose()
        {
            AnsiConsole.Foreground = Foreground;
            AnsiConsole.Background = Background;
        }
    }
}