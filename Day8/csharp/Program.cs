﻿// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

var numR = lines.Length;
var numC = lines[0].Length;

var solutionPart1 = new HashSet<(int r, int c)>();
var solutionPart2 = new HashSet<(int r, int c)>();

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

bool PlaceAntinode((int r, int c) antinode, HashSet<(int r, int c)> solution)
{
    if (antinode.r < numR && antinode.r >= 0 && antinode.c < numC && antinode.c >= 0)
    {
        solution.Add(antinode);
        return true;
    }

    return false;
}

void PlaceAntinodesPart1((int r, int c) a, (int r, int c) b)
{
    // measure distance between a and b
    (int r, int c) distance = (a.r - b.r, a.c -  b.c);

    // extend beyond a
    var antinode1 = (a.r + distance.r, a.c + distance.c);

    // extend beyond b
    var antinode2 = (b.r - distance.r, b.c - distance.c);

    PlaceAntinode(antinode1, solutionPart1);
    PlaceAntinode(antinode2, solutionPart1);
}

void PlaceAntinodesPart2((int r, int c) a, (int r, int c) b)
{
    // measure distance between a and b
    (int r, int c) distance = (a.r - b.r, a.c -  b.c);

    // extend beyond a
    (int r, int c) = a;
    while (PlaceAntinode((r, c), solutionPart2))
    {
        (r, c) = (r + distance.r, c + distance.c);
    }

    // extend beyond b
    (r, c) = b;
    while (PlaceAntinode((r, c), solutionPart2))
    {
        (r, c) = (r - distance.r, c - distance.c);
    }
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

            PlaceAntinodesPart1(locations[i], locations[j]);
            PlaceAntinodesPart2(locations[i], locations[j]);
        }
    }
}

Console.WriteLine($"Num antinodes: {solutionPart1.Count}");
Console.WriteLine($"Num antinodes: {solutionPart2.Count}");
