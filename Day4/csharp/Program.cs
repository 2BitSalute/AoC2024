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

int FindHorizontalBackwards(int r, int c)
{
    for(int i = 0; i < word.Length; i++)
    {
        var index = c - i - 1;
        if (index < 0)
        {
            return 0;
        }

        if (matrix[r][index] != word[i])
        {
            return 0;
        }
    }

    return 1;
}

int FindHorizontal(int r, int c)
{
    for(int i = 0; i < word.Length; i++)
    {
        var index = c + i + 1;
        if (index >= numC)
        {
            return 0;
        }

        if (matrix[r][index] != word[i])
        {
            return 0;
        }
    }

    return 1;
}

int FindVerticalBackwards(int r, int c)
{
    for(int i = 0; i < word.Length; i++)
    {
        var index = r - i - 1;
        if (index < 0)
        {
            return 0;
        }

        if (matrix[index][c] != word[i])
        {
            return 0;
        }
    }

    return 1;
}

int FindVertical(int r, int c)
{
    for(int i = 0; i < word.Length; i++)
    {
        var index = r + i + 1;
        if (index >= numR)
        {
            return 0;
        }

        if (matrix[index][c] != word[i])
        {
            return 0;
        }
    }

    return 1;
}

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
            // result +=
            //     FindHorizontal(r, c) +
            //     FindHorizontalBackwards(r, c) +
            //     FindVertical(r, c) +
            //     FindVerticalBackwards(r, c);

            if (FindInDirection(r, c, 0, 1) != FindHorizontal(r, c))
            {
                Console.WriteLine($"FindInDirection wrong at {r}, {c}");
            }
            if (FindInDirection(r, c, 0, -1) != FindHorizontalBackwards(r, c))
            {
                Console.WriteLine($"FindInDirection wrong at {r}, {c}");
            }
            if (FindInDirection(r, c, 1, 0) != FindVertical(r, c))
            {
                Console.WriteLine($"FindInDirection wrong at {r}, {c}");
            }
            if (FindInDirection(r, c, -1, 0) != FindVerticalBackwards(r, c))
            {
                Console.WriteLine($"FindInDirection wrong at {r}, {c}");
            }

            result += Find(r, c);
        }
    }
}

Console.WriteLine($"Result: {result}");