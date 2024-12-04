using System.IO;
using System.Reflection;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// var filename = "../short_input";
var filename = "../input";

var lines = File.ReadAllLines(filename);

var left = new PriorityQueue<int, int>(initialCapacity: lines.Length);
var right = new PriorityQueue<int, int>(initialCapacity: lines.Length);

// var list = new List<int>(capacity: lines.Length);
// var dict = new Dictionary<int, int>(capacity: lines.Length);

var size = 100000;
var ll = new int[size];
var rr = new int[size];

foreach (var line in lines)
{
    var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var l = int.Parse(tokens[0]);
    var r = int.Parse(tokens[1]);

    left.Enqueue(l, l);
    right.Enqueue(r, r);

    ll[l]++;
    rr[r]++;
}

var sumOfDiffs = 0;

while (left.Count > 0 && right.Count > 0)
{
    var l = left.Dequeue();
    var r = right.Dequeue();

    sumOfDiffs += Math.Abs(r - l);
}

Console.WriteLine($"Part 1, sum of diffs:\n   {sumOfDiffs}");

var similarityScore = 0;
for(int i = 0; i < ll.Length; i++)
{
    var calc = i * ll[i] * rr[i];
    similarityScore += calc; 
}

Console.WriteLine($"Part 2, similarity score:\n   {similarityScore}");