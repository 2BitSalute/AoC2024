// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

var numR = lines.Length;
var numC = lines[0].Length;

var solutionPart1 = new HashSet<(int r, int c)>();

var frequencies = new Dictionary<char, List<(int, int)>>();

for(int r = 0; r < numR; r++)
{
    for (int c = 0; c < numC; c++)
    {
        char f = lines[r][c];
        if (f == '.')
        {
            continue;
        }

        if (!frequencies.ContainsKey(f))
        {
            frequencies.Add(f, []);
        }

        frequencies[f].Add((r, c));
    }
}

void PlaceAntinode((int r, int c) antinode)
{
    if (antinode.r < numR && antinode.r >= 0 && antinode.c < numC && antinode.c >= 0)
    {
        solutionPart1.Add(antinode);
    }
}

void PlaceAntinodes((int r, int c) a, (int r, int c) b)
{
    // reorder points so the leftmost or topmost (if columns are the same) is first
    if (a.c > b.c || (a.c == b.c && a.r > b.r))
    {
        (a, b) = (b, a);
    }

    // measure distance between a and b
    (int r, int c) distance = (a.r - b.r, a.c -  b.c);

    // extend beyond a
    var antinode1 = (a.r + distance.r, a.c + distance.c);

    // extend beyond b
    var antinode2 = (b.r - distance.r, b.c - distance.c);

    PlaceAntinode(antinode1);
    PlaceAntinode(antinode2);
}

// For each pair of locations
foreach((var frequency, var locations) in frequencies)
{
    Console.WriteLine($"{frequency} has {locations.Count} locations");
    for (int i = 0; i < locations.Count; i++)
    {
        for (int j = 0; j < locations.Count; j++)
        {
            if (locations[i] == locations[j])
            {
                continue;
            }

            PlaceAntinodes(locations[i], locations[j]);
        }
    }
}

Console.WriteLine($"Num antinodes: {solutionPart1.Count}");