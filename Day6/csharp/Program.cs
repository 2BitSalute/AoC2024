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

(int r, int c, (int R, int C) direction) start = (0, 0, (0, 0));

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

(int r, int c) Increment(int r, int c, (int R, int C) direction) => (r + direction.R, c + direction.C);

bool SolvePart2(int r, int c, (int R, int C) direction)
{
    // var (initialR, initialC, initialDirection) = (r, c, direction);
    // Console.WriteLine($"({r},{c}) ({direction.R}, {direction.C})");

    // Pretend there is an obstacle
    // direction = Turn90Degrees(direction);

    var seen = new HashSet<(int r, int c, (int R, int C))>
    {
        // (initialR, initialC, initialDirection)
    };

    while(true)
    {
        // Console.WriteLine($"({r},{c}) ({direction.R}, {direction.C})");

        var point = (r, c, direction);
        if (seen.Contains(point))
        {
            return true;
        }

        seen.Add(point);

        var (rNext, cNext) = Increment(r, c, direction);

        if (OutOfBounds(rNext, cNext))
        {
            // Not a cycle
            return false;
        }

        if (map[rNext][cNext] == '#')
        {
            // turn 90%
            direction = Turn90Degrees(direction);
            (rNext, cNext) = Increment(r, c, direction);
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

    var (rNext, cNext) = Increment(r, c, direction);

    if (OutOfBounds(rNext, cNext))
    {
        return (part1count, part2Count);
    }

    if (map[rNext][cNext] == '#')
    {
        // turn 90%
        direction = Turn90Degrees(direction);
        (rNext, cNext) = Increment(r, c, direction);
    }
    else
    {
        (int R, int C) pretendObstacle = (r + direction.R, c + direction.C);
        if (!seenSolutions.Contains(pretendObstacle))
        {
            // Pretend there is an obstacle
            var orig = map[pretendObstacle.R][pretendObstacle.C];
            map[pretendObstacle.R][pretendObstacle.C] = '#';
            if (SolvePart2(start.r, start.c, start.direction))
            {
                seenSolutions.Add(pretendObstacle);
            }

            map[pretendObstacle.R][pretendObstacle.C] = orig;
        }
    }

    return Solve(rNext, cNext, direction, part1count, part2Count);
}

int SolvePart1(int r, int c, (int R, int C) direction, bool hypothetical)
{
    var count = 0;
    var seen = new HashSet<(int r, int c, (int R, int C))>();

    while (true)
    {
        var point = (r, c, direction);
        if (seen.Contains(point))
        {
            Console.WriteLine($"Cycle {hypothetical}");
            return -1;
        }
        seen.Add(point);

        if (hypothetical)
        {
            if (map[r][c] != 'X')
            {
                count++;
                map[r][c] = 'X';
            }
        }

        var (rNext, cNext) = Increment(r, c, direction);
        if (OutOfBounds(rNext, cNext))
        {
            return count;
        }

        if (hypothetical && map[rNext][cNext] != '#')
        {
            (int R, int C) pretendObstacle = Increment(r, c, direction);
            if (!seenSolutions.Contains(pretendObstacle))
            {
                // Pretend there is an obstacle
                var orig = map[pretendObstacle.R][pretendObstacle.C];
                map[pretendObstacle.R][pretendObstacle.C] = '#';
                if (SolvePart1(start.r, start.c, start.direction, false) == -1)
                {
                    seenSolutions.Add(pretendObstacle);
                }

                map[pretendObstacle.R][pretendObstacle.C] = orig;
            }
        }

        while (map[rNext][cNext] == '#')
        {
            // turn 90%
            direction = Turn90Degrees(direction);
            (rNext, cNext) = Increment(r, c, direction);
        }

        (r, c) = (rNext, cNext);
    }
}

int Parts1and2()
{
    for (int r = 0; r < numRows; r++)
    {
        for (int c = 0; c < numCols; c++)
        {
            var direction = GetDirection(map[r][c]);
            if (direction != (0,0))
            {
                start = (r, c, direction);
                map[r][c] = '.';

                // return Solve(r, c, direction, 0, 0).Part1;
                return SolvePart1(r, c, direction, true);
            }
        }
    }

    throw new InvalidOperationException("Not supposed to happen");
}

var part1 = Parts1and2();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {seenSolutions.Count}");