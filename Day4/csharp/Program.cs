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
var word = new[] { 'M', 'A', 'S' };

int FindInDirection(int r, int c, int dirR, int dirC)
{
    for(int i = 0; i < word.Length; i++)
    {
        var indexR = r + (i * dirR) + dirR;
        if (indexR >= numR || indexR < 0)
        {
            return 0;
        }

        var indexC = c + (i * dirC) + dirC;
        if (indexC >= numC || indexC < 0)
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
    var dirs = new int [] { -1, 0, 1 };

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

var result = 0;
for (int r = 0; r < numR; r++)
{
    for (int c = 0; c < numC; c++)
    {
        if (matrix[r][c] == 'X')
        {
            result += Find(r, c);
        }
    }
}

Console.WriteLine($"Result: {result}");