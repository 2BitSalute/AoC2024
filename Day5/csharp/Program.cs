// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

var edges = new Dictionary<string, HashSet<string>>();

bool CompliesWithRules(string[] u)
{
    for (int i = 0; i < u.Length; i++)
    {
        for (int j = i + 1; j < u.Length; j++)
        {
            if (edges.TryGetValue(u[j], out HashSet<string>? mustBeAfter) && mustBeAfter.Contains(u[i]))
            {
                return false;
            }
        }
    }

    return true;
}

void ComplyWithRules(string[] u)
{
    for (int i = 0; i < u.Length; i++)
    {
        for (int j = i + 1; j < u.Length; j++)
        {
            if (edges.TryGetValue(u[j], out HashSet<string>? mustBeAfter) && mustBeAfter.Contains(u[i]))
            {
                var temp = u[j];
                u[j] = u[i];
                u[i] = temp;

                i = -1;
                j = 0;
                break;
            }
        }
    }
}

// Find the path in the graph (ignoring intermediate vertices)
bool FindPath(string vertex, string[] u, int i, HashSet<(string, int)> visited)
{
    if (vertex == u[i])
    {
        i++;
    }

    if (i == u.Length)
    {
        return true;
    }

    if (edges.TryGetValue(vertex, out HashSet<string>? candidates))
    {
        foreach (var candidate in candidates)
        {
            if (FindPath(candidate, u, i, visited))
            {
                return true;
            }
        }
    }

    return false;
}

bool FindPathIter(string vertex, string[] u)
{
    int i = 0;
    var next = new Stack<(string, int)>();
    next.Push((vertex, i));

    var visited = new HashSet<(string, int)>();

    while (next.Count > 0)
    {
        (vertex, i) = next.Pop();
        visited.Add((vertex, i));

        if (vertex == u[i])
        {
            i++;
        }

        if (i == u.Length)
        {
            return true;
        }

        if (edges.TryGetValue(vertex, out HashSet<string>? candidates))
        {
            foreach (var candidate in candidates.Where(c => !visited.Contains((c, i))))
            {
                next.Push((candidate, i));
            }
        }
    }

    return false;
}

int GetMiddleValue(string[] u) => int.Parse(u[u.Length / 2]);

bool rules = true;
int sum = 0;
int incorrectSum = 0;
foreach (var line in lines)
{
    if (line == "")
    {
        rules = false;
        continue;
    }

    if (rules)
    {
        var tokens = line.Split('|');
        var from = tokens[0];
        var to = tokens[1];

        if (!edges.ContainsKey(from))
        {
            edges.Add(from, []);
        }

        edges[from].Add(to);
    }
    else
    {
        // tests/updates
        var update = line.Split(',');

        try
        {
            // if (CompliesWithRules(u))
            // {
            //     sum += GetMiddleValue(u);
            // }
            // else
            // {
            //     ComplyWithRules(u);
            //     incorrectSum += GetMiddleValue(u);
            // }

            var original = new List<string>(update);
            Array.Sort(update, new Comparison<string>((a, b) =>
                edges.TryGetValue(a, out HashSet<string>? mustBeAfter) && mustBeAfter.Contains(b) ? -1 : 1
            ));

            if (original.SequenceEqual(update))
            {
                sum += GetMiddleValue(update);
            }
            else
            {
                incorrectSum += GetMiddleValue(update);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

Console.WriteLine($"Part 1: {sum}");
Console.WriteLine($"Part 2: {incorrectSum}");