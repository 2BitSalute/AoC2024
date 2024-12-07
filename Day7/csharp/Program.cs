// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

long Add(long a, long b) => a + b;
long Mul(long a, long b) => a * b;
long Con(long a, long b) => long.Parse(a.ToString() + b.ToString());

long FindSolution(long target, long result, Func<long, long, long> op, long[] operands)
{
    var operand = operands[0];

    result = op(result, operand);

    if (operands.Length == 1) // last operand
    {
        return result == target ? result : 0;
    }

    operands = operands[1..];

    return 
        Math.Max(
            FindSolution(target, result, Con, operands),
            Math.Max(
                FindSolution(target, result, Add, operands),
                FindSolution(target, result, Mul, operands)
            ));
}

var sum = 0L;
foreach(var line in lines)
{
    var colon = line.IndexOf(':');
    var result = long.Parse(line[..colon]);
    var operands =
        line[(colon + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray() ?? throw new InvalidOperationException();

    var operand = operands[0];
    operands = operands[1..];

    var s = Math.Max(
        FindSolution(result, operand, Con, operands),
        Math.Max(
            FindSolution(result, operand, Add, operands),
            FindSolution(result, operand, Mul, operands)
        ));

    sum += s;
}

Console.WriteLine($"Solution: {sum}");