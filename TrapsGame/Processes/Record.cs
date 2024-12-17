using System.IO;

namespace TrapsGame.Processes;

internal static class Record
{
    static string _path = Directory.GetCurrentDirectory() + "/record.txt";

    static public int Get()
    {
        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, "0");
        }

        if (int.TryParse(File.ReadAllText(_path), out int result))
        {
            return result;
        }
        return 0;
    }

    static public void Set(int value)
    {
        int last = Get();

        if (value > last)
        {
            File.WriteAllText(_path, value.ToString());
        }
    }
}
