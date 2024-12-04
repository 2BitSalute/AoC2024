// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

var regex = new Regex(@"mul\((\d+),(\d+)\)");

ulong sum = 0;
foreach (var line in lines)
{
    var matches = regex.Matches(line);
    if (matches is MatchCollection matchCollection)
    {
        foreach (Match match in matchCollection)
        {
            ulong product = 1UL;
            foreach (Group group in match.Groups.Values.Skip(1))
            {
                product *= ulong.Parse(group.Value);
            }

            sum += product;
        }
    }
}

Console.WriteLine($"Sum of products: {sum}");

regex = new Regex(@"mul\((\-?\d+),(\-?\d+)\)|don't\(\)|do\(\)");

bool should = true;
sum = 0;
foreach (var line in lines)
{
    var matches = regex.Matches(line);
    if (matches is MatchCollection matchCollection)
    {
        foreach (Match match in matchCollection)
        {
            var first = match.Groups.Values.First().Value;
            if (first == "do()")
            {
                should = true;
            }
            else if (first == "don't()")
            {
                should = false;
            }
            else if (should)
            {
                ulong product = 1UL;
                foreach (Group group in match.Groups.Values.Skip(1))
                {
                    product *= ulong.Parse(group.Value);
                }

                sum += product;
            }
        }
    }
}

Console.WriteLine($"Sum of products with conditionals: {sum}");