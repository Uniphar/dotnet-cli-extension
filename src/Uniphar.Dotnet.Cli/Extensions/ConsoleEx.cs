public static class ConsoleEx
{
    public static ConsoleColorReset SetColors(ConsoleColor? foreground = default, ConsoleColor? background = default)
    {
        return new ConsoleColorReset(foreground, background);
    }

    public readonly struct ConsoleColorReset : IDisposable
    {
        private ConsoleColor Foreground { get; }
        private ConsoleColor Background { get; }

        public ConsoleColorReset(ConsoleColor? newForeground = default, ConsoleColor? newBackground = default)
        {
            Foreground = Console.ForegroundColor;
            Background = Console.BackgroundColor;

            Console.ForegroundColor = newForeground ?? Foreground;
            Console.BackgroundColor = newBackground ?? Background;
        }

        public void Dispose()
        {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
        }
    }
}