using System.Threading.Tasks;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Runs the task.
        /// </summary>
        public static void Await(this Task task) => task.GetAwaiter().GetResult();

        /// <summary>
        /// Runs the task of type <typeparamref name="T"/> and returns the result.
        /// </summary>
        public static T Await<T>(this Task<T> task) => task.GetAwaiter().GetResult();

        /// <summary>
        /// Runs the task. See <see cref="Task.ConfigureAwait(bool)"/>.
        /// </summary>
        public static void Await(this Task task, bool continueOnCapturedContext)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs the task of type <typeparamref name="T"/> and returns the result.
        /// See <see cref="Task.ConfigureAwait(bool)"/>.
        /// </summary>
        public static T Await<T>(this Task<T> task, bool continueOnCapturedContext)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
    }
}
