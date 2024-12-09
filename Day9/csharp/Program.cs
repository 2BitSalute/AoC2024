// var filename = @"../short_input";
var filename = @"../input";

var lines = File.ReadAllLines(filename);

var file = true;
var fs = new List<int>();

void Fill(int count, int value)
{
    for (int i = 0; i < count; i++)
    {
        fs.Add(value);
    }
}

var free = new List<(int L, int Pos)>();
var files = new List<(int Id, int L, int Pos)>();
var id = 0;

foreach(var length in lines[0].Select(c => int.Parse(c + "")))
{
    var value = file ? id : -1;
    var position = fs.Count;

    if (file)
    {
        files.Add((id, length, position));
        id += 1;
    }
    else
    {
        free.Add((length, position));
    }

    Fill(length, value);

    file = !file;
}

void Part2()
{
    for(int i = files.Count - 1; i >= 0; i--)
    {
        var file = files[i];
        for (int j = 0; j < free.Count; j++)
        {
            var slot = free[j];
            if (file.Pos < slot.Pos)
            {
                break;
            }

            if (slot.L >= file.L)
            {
                // swap file and free space
                for(int k = 0; k < file.L; k++)
                {
                    (fs[slot.Pos + k], fs[file.Pos + k]) = (fs[file.Pos + k], fs[slot.Pos + k]);
                }

                // update free space at j to new length
                free[j] = (slot.L - file.L, slot.Pos + file.L);

                break;
            }
        }
    }
}

void Part1()
{
    (int left, int right) = (0, fs.Count - 1);
    while(left < right)
    {
        if (fs[left] != -1)
        {
            left++;
        }
        else // found empty space
        {
            if (fs[right] == -1)
            {
                right--;
            }
            else // found something to move
            {
                (fs[left], fs[right]) = (fs[right], fs[left]);
            }
        }
    }
}

// Part1();
Part2();

var checksum = 0L;
for(int i = 0; i < fs.Count; i++)
{
    var value = fs[i];

    if (value != -1)
    {
        checksum += value * i;
    }

    // Console.Write(value != -1 ? value.ToString() : ".");
}
Console.WriteLine();

Console.WriteLine($"Checksum: {checksum}");
