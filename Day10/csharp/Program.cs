using Points = System.Collections.Generic.HashSet<(int r, int c)>;

var map = File.ReadAllLines("../short_input");
// var map = File.ReadAllLines("../input");

int numR = map.Length;
int numC = map[0].Length;

var directions = new (int r, int c)[] {(0, 1), (0, -1), (1, 0), (-1, 0)};

bool part1; // used only to indicate whether alternative routes should be counted; part 1 does not

bool IsCandidate((int r, int c) curr, (int r, int c) next, Points visited)
{
    return !
        ((part1 && visited.Contains(next)) ||
        next.r < 0 || next.c < 0 || next.r >= numR || next.c >= numC ||
        map[curr.r][curr.c] + 1 != map[next.r][next.c]);
}

int FindTops(int r, int c, Points visited, Points tops)
{
    visited.Add((r, c));

    if (map[r][c] == '9')
    {
        tops.Add((r, c));
        return 1;
    }

    var score = 0;
    foreach (var direction in directions)
    {
        (int rNext, int cNext) = (r + direction.r, c + direction.c);
        if (!IsCandidate((r, c), (rNext, cNext), visited))
        {
            continue;
        }

        score += FindTops(rNext, cNext, visited, tops);
    }

    return score;
}

var part1Score = 0;
var part2Score = 0;
for (int r = 0; r < numR; r++)
{
    for (int c = 0; c < numC; c++)
    {
        if (map[r][c] == '0')
        {
            part1 = true;
            part1Score += FindTops(r, c, [], []);

            part1 = false;
            part2Score += FindTops(r, c, [], []);
        }
    }
}

Console.WriteLine($"{part1Score}");
Console.WriteLine($"{part2Score}");