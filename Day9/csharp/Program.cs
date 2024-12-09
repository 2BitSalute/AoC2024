using System.Runtime.ExceptionServices;

// var filename = @"../short_input";
var filename = @"../input";

var lines = File.ReadAllLines(filename);

int ParseChar(char c)
{
    return c switch
    {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        '8' => 8,
        '9' => 9,
        _ => throw new ArgumentException("Expecting a digit")
    };
}

var file = true;
var fs = new List<int>();

void Fill(int count, int value)
{
    for (int i = 0; i < count; i++)
    {
        fs.Add(value);
    }
}

var id = 0;

foreach(var d in lines[0].Select(ParseChar)) // int.Parse(c + "")
{
    // alternating files and free space
    var value = file ? id : -1;
    Fill(d, value);

    id = file ? id + 1 : id;

    file = !file;
}

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

var checksum = 0L;
for(int i = 0; i < fs.Count; i++)
{
    var value = fs[i];

    if (value != -1)
    {
        checksum += value * i;
    }

    // Console.Write(value);
}

Console.WriteLine($"Checksum: {checksum}");
