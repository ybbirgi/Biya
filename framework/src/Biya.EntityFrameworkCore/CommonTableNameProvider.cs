using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore;

public static class CommonTableNameProvider
{
    public static string GetTableName<T>(
        this EntityTypeBuilder<T> entityTypeBuilder
    ) where T : class => typeof(T).Name
        switch
        {
            _ => MakePlural(typeof(T).Name)
        };

    public static string MakePlural(
        string word
    )
    {
        if (string.IsNullOrWhiteSpace(word))
            return word;

        if (word.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
            word.Length > 1 &&
            !"aeiou".Contains(char.ToLower(word[word.Length - 2])))
        {
            return word.Substring(0, word.Length - 1) + "ies";
        }
        else if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase) ||
                 word.EndsWith("x", StringComparison.OrdinalIgnoreCase) ||
                 word.EndsWith("z", StringComparison.OrdinalIgnoreCase) ||
                 word.EndsWith("ch", StringComparison.OrdinalIgnoreCase) ||
                 word.EndsWith("sh", StringComparison.OrdinalIgnoreCase))
        {
            return word + "es";
        }
        else
        {
            return word + "s";
        }
    }
}
