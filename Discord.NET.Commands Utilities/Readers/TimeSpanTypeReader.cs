using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses <see cref="TimeSpan"/>.</summary>
    public class TimeSpanTypeReader : TypeReader
    {
        /// <summary>Milliseconds per week</summary>
        public const long FromWeek = 604_800_000;

        /// <summary>Milliseconds per day</summary>
        public const long FromDay = 86_400_000;

        /// <summary>Milliseconds per hour</summary>
        public const long FromHour = 3_600_000;

        /// <summary>Milliseconds per minute</summary>
        public const long FromMinute = 60_000;

        /// <summary>Milliseconds per second</summary>
        public const long FromSecond = 1_000;

        /// <summary>Milliseconds per millisecond</summary>
        public const long FromMilliSecond = 1;


        /// <summary>Regex parser for weeks</summary>
        public static readonly Regex Week = new Regex(@"(\d+)[w]", RegexOptions.IgnoreCase);

        /// <summary>Regex parser for days</summary>
        public static readonly Regex Day = new Regex(@"(\d+)[d]", RegexOptions.IgnoreCase);

        /// <summary>Regex parser for hours</summary>
        public static readonly Regex Hour = new Regex(@"(\d+)[h]", RegexOptions.IgnoreCase);

        /// <summary>Regex parser for minutes</summary>
        public static readonly Regex Minute = new Regex(@"(\d+)[m](?!s)", RegexOptions.IgnoreCase);

        /// <summary>Regex parser for seconds</summary>
        public static readonly Regex Second = new Regex(@"(\d+)[s]", RegexOptions.IgnoreCase);

        /// <summary>Regex parser for milliseconds</summary>
        public static readonly Regex MilliSecond = new Regex(@"(\d+)(ms)", RegexOptions.IgnoreCase);

        /// <summary>Dictionary between milliseconds and the regex parser</summary>
        public static readonly ImmutableDictionary<long, Regex> Parsers = new Dictionary<long, Regex>
        {
            { FromMilliSecond, MilliSecond },
            { FromSecond, Second },
            { FromMinute, Minute },
            { FromHour, Hour },
            { FromDay, Day },
            { FromWeek, Week},
        }.ToImmutableDictionary();


        /// <summary>Provides a default static error message</summary>
        public static TypeReaderResult Error { get; } = TypeReaderResult.FromError(CommandError.ParseFailed, "Failed to parse timespan from input, or input was 0 milliseconds");


        /// <summary>Parses the <see cref="TimeSpan"/></summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var millis = 0L;
            TypeReaderResult result;

            foreach (var pair in Parsers)
            {
                foreach (Match match in pair.Value.Matches(input))
                {
                    input = input.Replace(match.Value, "");
                    millis += pair.Key * long.Parse(match.Groups[1].Value);
                }
            }

            if (millis == 0)
            {
                if (TimeSpan.TryParse(input, out TimeSpan time))
                    result = TypeReaderResult.FromSuccess(time);
                else
                    result = Error;
            }
            else
            {
                result = TypeReaderResult.FromSuccess(TimeSpan.FromMilliseconds(millis));
            }

            return Task.FromResult(result);
        }
    }
}
