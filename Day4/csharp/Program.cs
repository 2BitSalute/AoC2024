// See https://aka.ms/new-console-template for more information

// var filename = "../short_input";
var filename = "../input";

var matrix = File.ReadAllLines(filename);
var numC = matrix[0].Length;
var numR = matrix.Length;

// When encountering X, start searching all the patterns
// - Horizontal in 2 directions
// - Diagonal in 4 directions
// - Vertical in 4 directions

// Skip X
var word = new [] { 'M', 'A', 'S' };

bool IsInvalidRow(int indexR) => indexR >= numR || indexR < 0;
bool IsInvalidCol(int indexC) => indexC >= numC || indexC < 0;

int FindInDirection(int r, int c, int dirR, int dirC)
{
    for(int i = 0; i < word.Length; i++)
    {
        var indexR = r + (i * dirR) + dirR;
        if (IsInvalidRow(indexR))
        {
            return 0;
        }

        var indexC = c + (i * dirC) + dirC;
        if (IsInvalidCol(indexC))
        {
            return 0;
        }

        if (matrix[indexR][indexC] != word[i])
        {
            return 0;
        }
    }

    return 1;
}

int Find(int r, int c)
{
    var dirs = new [] { -1, 0, 1 };

    var result = 0;
    for (int dirR = 0; dirR < dirs.Length; dirR++)
    {
        for (int dirC = 0; dirC < dirs.Length; dirC++)
        {
            result += FindInDirection(r, c, dirs[dirR], dirs[dirC]);
        }
    }

    return result;
}

bool FindXChar(int r, int c, int dirR, int dirC, char toFind)
{
    var indexR = r + dirR;
    if (IsInvalidRow(indexR))
    {
        return false;
    }

    var indexC = c + dirC;
    if (IsInvalidCol(indexC))
    {
        return false;
    }

    return matrix[indexR][indexC] == toFind;
}

bool FindXHalf(int r, int c, int dirR, int dirC)
{
    return
        FindXChar(r, c, dirR, dirC, 'M') &&
        FindXChar(r, c, dirR * -1, dirC * -1, 'S');
}

int FindX(int r, int c)
{
    var dirs = new [] { 1, -1 };

    var result = 0;
    for (int dirR = 0; dirR < dirs.Length; dirR++)
    {
        for (int dirC = 0; dirC < dirs.Length; dirC++)
        {
            result += FindXHalf(r, c, dirs[dirR], dirs[dirC]) ? 1 : 0;
        }
    }

    return result == 2 ? 1 : 0;
}

var part1Result = 0;
var part2Result = 0;
for (int r = 0; r < numR; r++)
{
    for (int c = 0; c < numC; c++)
    {
        if (matrix[r][c] == 'X')
        {
            part1Result += Find(r, c);
        }
        else if (matrix[r][c] == 'A')
        {
            part2Result += FindX(r, c);
        }
    }
}

Console.WriteLine($"Part 1 result: {part1Result}");
Console.WriteLine($"Part 2 result: {part2Result}");
