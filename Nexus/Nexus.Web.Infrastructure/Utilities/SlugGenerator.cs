
using Nexus.Web.Infrastructure.Utilities.Interfaces;

namespace Nexus.Web.Infrastructure.Utilities
{
    public class SlugGenerator : ISlugGenerator
    {
        public string GenerateSlug(string input)
        {

            string[] arrayInputData = input
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.ToLowerInvariant())
            .ToArray();

            return string.Join("-", arrayInputData);

        }
    }
}
