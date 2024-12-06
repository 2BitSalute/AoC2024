var filename = "../short_input";
// var filename = "../input";

var lines = File.ReadAllLines(filename);
var map = lines.Select(line => line.ToArray()).ToArray();

(int R, int C) GetDirection(char cursor)
{
    return cursor switch
    {
        '^' => (-1, 0),
        'v' => (1, 0),
        '>' => (0, 1),
        '<' => (0, -1),
        _ => (0, 0)
    };
}

var numRows = map.Length;
var numCols = map[0].Length;

bool OutOfBounds(int r, int c) => r < 0 || r >= numRows || c < 0 || c >= numCols;

(int R, int C) Turn90Degrees((int R, int C) direction)
{
    return direction switch
    {
        (0, 1) => (1, 0),
        (1, 0) => (0, -1),
        (0, -1) => (-1, 0),
        (-1, 0) => (0, 1),
        _ => throw new InvalidOperationException()
    };
}

int GetLength(int r, int c, (int R, int C) direction, int max)
{
    if (max == 0)
    {
        return 0;
    }

    int l = 0;
    while (map[r][c] != '#')
    {
        r += direction.R;
        c += direction.C;
        l++;

        if (l == max)
        {
            Console.WriteLine($"Max reached: {max}");
            break;
        }
    }

    Console.WriteLine($"Length is {l}");

    return l;
}

(int R, int C) Increment(int r, int c, (int R, int C) direction, int factor) => (r + direction.R * (factor -  1), c + direction.C * (factor - 1));

var seenSolutions = new HashSet<((int r, int c), (int r, int c), (int r, int c), (int r, int c))>();

int SolvePart2(int r, int c, (int R, int C) direction)
{
    try
    {
        var point1 = (r, c);
        // draw X1 (stop at #)
        int x1 = GetLength(r, c, direction, max: -1);

        (r, c) = Increment(r, c, direction, x1);
        direction = Turn90Degrees(direction);

        var point2 = (r, c);

        // draw Y1 (stop at #)
        int y1 = GetLength(r, c, direction, max: -1);

        (r, c) = Increment(r, c, direction, y1);
        direction = Turn90Degrees(direction);

        var point3 = (r, c);
        // can draw X2 (stop at length of X1)
        int x2 = GetLength(r, c, direction, max: x1);
        if (x2 > x1)
        {
            return 0;
        }

        (r, c) = Increment(r, c, direction, x2);
        direction = Turn90Degrees(direction);

        var point4 = (r, c);
        // can draw Y2 (stop at length of Y1 and at starting point)
        int y2 = GetLength(r, c, direction, max: y1);

        var result = y2 < y1 ? 0 : 1;

        (r, c) = Increment(r, c, direction, y2);
        point1 = (r, c);

        var a = new (int r, int c)[]{ point1, point2, point3, point4};
        Array.Sort(a);

        if (result == 1)
        {
            seenSolutions.Add((a[0], a[1], a[2], a[3]));

            Console.WriteLine($"({a[0].r}, {a[0].c}) ({a[1].r}, {a[1].c}) ({a[2].r}, {a[2].c}) ({a[3].r}, {a[3].c})");
            Console.WriteLine($"({point1.r}, {point1.c}) ({point2.r}, {point2.c}) ({point3.r}, {point3.c}) ({point4.r}, {point4.c})");
        }

        return result;
    }
    catch
    {
        // Out of bounds
        return 0;
    }
}

(int Part1, int Part2) Solve(int r, int c, (int R, int C) direction, int part1count, int part2Count)
{
    if (map[r][c] != 'X')
    {
        part1count++;
        map[r][c] = 'X';
    }

    var rNext = r + direction.R;
    var cNext = c + direction.C;

    if (OutOfBounds(rNext, cNext))
    {
        return (part1count, part2Count);
    }

    if (map[rNext][cNext] == '#')
    {
        // turn 90%
        direction = Turn90Degrees(direction);
        rNext = r + direction.R;
        cNext = c + direction.C;

        part2Count += SolvePart2(r, c, direction);
    }

    return Solve(rNext, cNext, direction, part1count, part2Count);
}

(int Part1, int Part2) Parts1and2()
{
    for (int r = 0; r < numRows; r++)
    {
        for (int c = 0; c < numCols; c++)
        {
            var direction = GetDirection(map[r][c]);
            if (direction != (0,0))
            {
                map[r][c] = '.';

                return Solve(r, c, direction, 0, 0);
            }
        }
    }

    throw new InvalidOperationException("Not supposed to happen");
}

var (part1, part2) = Parts1and2();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {seenSolutions.Count}");