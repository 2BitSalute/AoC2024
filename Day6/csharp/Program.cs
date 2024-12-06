// var filename = "../short_input";
var filename = "../input";

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

var seenSolutions = new HashSet<(int r, int c)>();

int SolvePart2(int r, int c, (int R, int C) direction)
{
    var (initialR, initialC, initialDirection) = (r, c, direction);
    // Console.WriteLine($"({r},{c}) ({direction.R}, {direction.C})");

    var pretendObstacle = (r + direction.R, c + direction.C);
    if (seenSolutions.Contains(pretendObstacle))
    {
        return 0;
    }

    // Pretend there is an obstacle
    direction = Turn90Degrees(direction);

    var seen = new HashSet<(int r, int c, (int R, int C))>
    {
        (initialR, initialC, initialDirection)
    };

    while(true)
    {
        // Console.WriteLine($"({r},{c}) ({direction.R}, {direction.C})");

        var point = (r, c, direction);

        var rNext = r + direction.R;
        var cNext = c + direction.C;

        if (OutOfBounds(rNext, cNext))
        {
            // Not a cycle
            return 0;
        }

        if (map[rNext][cNext] == '#')
        {
            // turn 90%
            direction = Turn90Degrees(direction);
            rNext = r + direction.R;
            cNext = c + direction.C;
        }
        else
        {
            if (seen.Contains(point))
            {
                // We're back to a starting point
                seenSolutions.Add(pretendObstacle);
                return 1;
            }
            
            seen.Add(point);
        }

        (r, c) = (rNext, cNext);
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
    }
    else
    {
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