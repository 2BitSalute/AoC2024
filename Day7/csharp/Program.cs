using Op = System.Func<long, long, long>;

// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

long Add(long a, long b) => a + b;
long Mul(long a, long b) => a * b;
long Con(long a, long b) => long.Parse(a.ToString() + b.ToString());

long Find(long target, long result, long[] operands)
{
    operands = operands[1..];

    var max = 0L;
    foreach(var op in new Op[] { Add, Mul, Con })
    {
        max = Math.Max(max, Apply(target, result, op, operands));
    }

    return max;
}

long Apply(long target, long result, Op op, long[] operands)
{
    var operand = operands[0];
    result = op(result, operand);
    if (operands.Length == 1) // last operand
    {
        return result == target ? result : 0;
    }

    return Find(target, result, operands);
}

var sum = 0L;
foreach(var line in lines)
{
    var colon = line.IndexOf(':');
    var target = long.Parse(line[..colon]);
    var operands =
        line[(colon + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray() ?? throw new InvalidOperationException();

    sum += Find(target, result: operands[0], operands);
}

Console.WriteLine($"Solution: {sum}");