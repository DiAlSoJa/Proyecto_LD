using System.Text.RegularExpressions;

namespace LD.Application.Features.ReportQueries;

public static class ReportQuerySqlParser
{
    private static readonly Regex ParameterRegex = new(
        @"(?<!@)@([A-Za-z_][A-Za-z0-9_]*)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex ForbiddenStatementRegex = new(
        @"\b(INSERT|UPDATE|DELETE|MERGE|DROP|ALTER|TRUNCATE|CREATE|EXEC|EXECUTE|GRANT|REVOKE|USE|BACKUP|RESTORE|INTO)\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex DeclareBlockRegex = new(
        @"\bDECLARE\b(?<body>.*?)(?:;|$)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private static readonly Regex AssignedVariableRegex = new(
        @"\b(?:SET|SELECT)\s+@([A-Za-z_][A-Za-z0-9_]*)\s*=",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex DeclaredVariableRegex = new(
        @"^\s*@([A-Za-z_][A-Za-z0-9_]*)\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static IReadOnlyList<string> GetParameterNames(string? sqlQuery)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
            return [];

        var sanitized = RemoveSqlLiteralsAndComments(sqlQuery);
        var excludedParameters = GetExcludedParameters(sanitized);
        var ordered = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (Match match in ParameterRegex.Matches(sanitized))
        {
            var parameterName = match.Groups[1].Value.Trim();
            if (string.IsNullOrWhiteSpace(parameterName))
                continue;

            if (excludedParameters.Contains(parameterName))
                continue;

            if (seen.Add(parameterName))
                ordered.Add(parameterName);
        }

        return ordered;
    }

    public static bool IsReadOnlyQuery(string? sqlQuery)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
            return false;

        var sanitized = RemoveSqlLiteralsAndComments(sqlQuery);
        if (ForbiddenStatementRegex.IsMatch(sanitized))
            return false;

        var normalized = sanitized.TrimStart().TrimStart(';').TrimStart();
        return normalized.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith("WITH", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith("DECLARE", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith("SET", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith(";", StringComparison.OrdinalIgnoreCase);
    }

    private static string RemoveSqlLiteralsAndComments(string sqlQuery)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
            return string.Empty;

        var result = sqlQuery;

        result = Regex.Replace(
            result,
            @"--.*?$",
            " ",
            RegexOptions.Multiline);

        result = Regex.Replace(
            result,
            @"/\*.*?\*/",
            " ",
            RegexOptions.Singleline);

        result = Regex.Replace(
            result,
            @"'(?:''|[^'])*'",
            " ",
            RegexOptions.Singleline);

        return result;
    }

    private static HashSet<string> GetExcludedParameters(string sqlQuery)
    {
        var excluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (Match declareBlock in DeclareBlockRegex.Matches(sqlQuery))
        {
            var body = declareBlock.Groups["body"].Value;
            foreach (var segment in SplitSqlSegments(body))
            {
                var declaredMatch = DeclaredVariableRegex.Match(segment);
                if (declaredMatch.Success)
                    excluded.Add(declaredMatch.Groups[1].Value);
            }
        }

        foreach (Match match in AssignedVariableRegex.Matches(sqlQuery))
            excluded.Add(match.Groups[1].Value);

        return excluded;
    }

    private static IEnumerable<string> SplitSqlSegments(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            yield break;

        var segment = new System.Text.StringBuilder();
        var parenthesesDepth = 0;
        var inString = false;

        for (var index = 0; index < text.Length; index++)
        {
            var current = text[index];
            var next = index + 1 < text.Length ? text[index + 1] : '\0';

            if (inString)
            {
                segment.Append(current);

                if (current == '\'' && next == '\'')
                {
                    segment.Append(next);
                    index++;
                    continue;
                }

                if (current == '\'')
                    inString = false;

                continue;
            }

            switch (current)
            {
                case '\'':
                    inString = true;
                    segment.Append(current);
                    break;
                case '(':
                    parenthesesDepth++;
                    segment.Append(current);
                    break;
                case ')':
                    if (parenthesesDepth > 0)
                        parenthesesDepth--;
                    segment.Append(current);
                    break;
                case ',' when parenthesesDepth == 0:
                    yield return segment.ToString();
                    segment.Clear();
                    break;
                default:
                    segment.Append(current);
                    break;
            }
        }

        if (segment.Length > 0)
            yield return segment.ToString();
    }
}
