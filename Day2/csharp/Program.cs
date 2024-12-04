// var filename = "../short_input";

var filename = "../input";

var lines = File.ReadAllLines(filename);

static (int Result, int Index) SafetyCheck(List<int> reports)
{
    List<int> diffs = new List<int>(reports.Count - 1);
    for (int i = 1; i < reports.Count; i++)
    {
        diffs.Add(reports[i] - reports[i - 1]);
    }

    var allIncreasing = diffs.TrueForAll(diff => diff > 0 && diff < 4);
    var allDecreasing = diffs.TrueForAll(diff => diff < 0 && diff > -4);

    if (allIncreasing || allDecreasing)
    {
        return (1, 0);
    }

    return (0, 0);
}

static (int Result, int Index) SafetyCheckOld(List<int> reports)
{
    // Rules:
    //  - Decreasing OR increasing
    //  - Diff must be > 0 and < 4

    var prevDiff = 0;
    for (int i = 1; i < reports.Count; i++)
    {
        var diff = reports[i] - reports[i - 1];
        if (diff == 0)
        {
            return (0, i);
        }
        else if (Math.Abs(diff) > 3)
        {
            return (0, i);
        }
        else if (prevDiff != 0 && (diff < 0 && prevDiff > 0 || diff > 0 && prevDiff < 0))
        {
            return (0, i);
        }

        prevDiff = diff;
    }

    return (1, 0);
}

// This implementation assumes that either the element at the current
// or the previous index can be removed to fix the problem.
//
// The original implementation was incorrect for these 3 cases from the input:
//  14 13 14 17 20 22 25 27
//  68 66 68 69 70
//  81 83 82 79 77 75 74 71
//
// The patterns are:
//  inc dec inc inc...
//  dec inc dec dec...
static int SafeCountOld(List<int> reports)
{
    var check = SafetyCheckOld(reports);
    if (check.Result > 0)
    {
        return check.Result;
    }

    var origReports = reports;

    for (int i = check.Index; (i >= (check.Index - 2)) && (i >= 0); i--)
    {
        reports = new List<int>(origReports);
        reports.RemoveAt(i);

        var secondCheck = SafetyCheckOld(reports);
        if (secondCheck.Result > 0)
        {
            return secondCheck.Result;
        }
    }

    return 0;
}

static int SafeCount(List<int> reports)
{
    var check = SafetyCheck(reports);
    if (check.Result > 0)
    {
        return check.Result;
    }

    // Look for which one is bad
    for(int i = 0; i < reports.Count; i++)
    {
        var clone = new List<int>(reports);
        clone.RemoveAt(i);

        check = SafetyCheck(clone);
        if (check.Result > 0)
        {
            return check.Result;
        }
    }

    return 0;
}

var safeCount = 0;

foreach (var line in lines)
{
    var sequence =
        line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse(s));

    var safe = SafeCountOld(sequence.ToList());

    safeCount += safe;

    Console.WriteLine($"SAFE: {safe}, {line}");
}

Console.WriteLine($"Part 1, safe count:\n   {safeCount}");
