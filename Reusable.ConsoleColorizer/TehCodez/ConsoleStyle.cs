using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using Reusable.MarkupBuilder.Html;

namespace Reusable.ConsoleColorizer
{
    public interface IConsoleStyle
    {
        ConsoleColor ForegroundColor { get; }

        ConsoleColor BackgroundColor { get; }

        IDisposable Apply();
    }

    public class ConsoleStyle : IConsoleStyle
    {
        // language=regexp
        //private const string StylePattern = "background-color: (?<BackgroundColor>([a-z]+))|color: (?<ForegroundColor>([a-z]+))";

        //private static readonly AsyncLocal<ConsoleStyle> _current = new AsyncLocal<ConsoleStyle>();

        private ConsoleStyle(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public static ConsoleStyle Current => new ConsoleStyle(Console.ForegroundColor, Console.BackgroundColor);

        public ConsoleColor ForegroundColor { get; }

        public ConsoleColor BackgroundColor { get; }

        public static ConsoleStyle Parse(XElement xElement)
        {
            var style = xElement.Attribute("style")?.Value;
            if (style is null)
            {
                return Current;
            }

            // https://regex101.com/r/V9XIgD/2

            var declarations = style.ToDeclarations().ToDictionary(x => x.property, x => x.value);

            //            var styleMatch = Regex.Match(style, StylePattern, RegexOptions.ExplicitCapture);
            //            if (!styleMatch.Success)
            //            {
            //                return Current;
            //            }                       

            var foregroundColor = string.Empty;
            var backgroundColor = string.Empty;

            declarations.TryGetValue("color", out foregroundColor);
            declarations.TryGetValue("background-color", out backgroundColor);

            return new ConsoleStyle(
                Enum.TryParse(foregroundColor, true, out ConsoleColor consoleForegroundColor) ? consoleForegroundColor : Console.ForegroundColor,
                Enum.TryParse(backgroundColor, true, out ConsoleColor consoleBackgroundColor) ? consoleBackgroundColor : Console.BackgroundColor);
        }

        public IDisposable Apply()
        {
            return Disposable<ConsoleStyle>.CreateInitialized(() =>
            {
                var restoreStyle = Current;
                Console.ForegroundColor = ForegroundColor;
                Console.BackgroundColor = BackgroundColor;
                return restoreStyle;
            }, restoreStyle =>
            {
                Console.ForegroundColor = restoreStyle.ForegroundColor;
                Console.BackgroundColor = restoreStyle.BackgroundColor;
            });
        }
    }
}