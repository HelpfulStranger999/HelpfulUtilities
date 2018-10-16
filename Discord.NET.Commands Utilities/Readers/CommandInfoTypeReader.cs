using Discord.Commands;
using HelpfulUtilities.Discord.Commands.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpfulUtilities.Discord.Commands.Readers
{
    /// <summary>This <see cref="TypeReader"/> parses <see cref="CommandInfo"/>.</summary>
    public class CommandInfoTypeReader : TypeReader
    {
        /// <summary>Provides a default static error message</summary>
        public static TypeReaderResult Error { get; } = TypeReaderResult.FromError(CommandError.ObjectNotFound, "Could not find any commands matching the given input.");

        /// <summary>Parses the command</summary>
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var search = services.GetService<CommandService>().Search(context, input);
            if (!search.IsSuccess) return Task.FromResult(Error);

            var matches = search.Commands.Where(cmd =>
            {
                return !cmd.Command.IsHidden();
            });

            if (matches.Count() <= 0) return Task.FromResult(Error);

            var first = matches.FirstOrDefault();
            if (first.Equals(default)) return Task.FromResult(Error);
            return Task.FromResult(TypeReaderResult.FromSuccess(first));
        }
    }
}
