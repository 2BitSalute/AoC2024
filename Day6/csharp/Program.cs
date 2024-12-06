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

int SolvePart1(int r, int c, (int R, int C) direction, int count)
{
    if (map[r][c] != 'X')
    {
        count++;
        map[r][c] = 'X';
    }

    var rNext = r + direction.R;
    var cNext = c + direction.C;

    if (OutOfBounds(rNext, cNext))
    {
        return count;
    }

    if (map[rNext][cNext] == '#')
    {
        // turn 90%
        direction = Turn90Degrees(direction);
        rNext = r + direction.R;
        cNext = c + direction.C;
    }

    return SolvePart1(rNext, cNext, direction, count);
}

int Part1()
{
    for (int r = 0; r < numRows; r++)
    {
        for (int c = 0; c < numCols; c++)
        {
            var direction = GetDirection(map[r][c]);
            if (direction != (0,0))
            {
                map[r][c] = '.';

                return SolvePart1(r, c, direction, 0);
            }
        }
    }

    throw new InvalidOperationException("Not supposed to happen");
}

var part1 = Part1();

Console.WriteLine($"Part 1: {part1}");