using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bletsync.src;

public class Logger
{
    public string changelogPath;

    public Logger(string logPath) 
    {
        changelogPath = logPath;
    }

    public void Log(string action, string path)
    {
        string log = $"{action}|{path}";
        WriteToChangelog(log);
    }
    public void Log(string action, string oldpath, string path)
    {
        string log = $"{action}|{oldpath}|{path}";
        WriteToChangelog(log);
    }
    private void WriteToChangelog(string log)
    {
        using StreamWriter writer = new(changelogPath, true, Encoding.UTF8);
        writer.WriteLine(log);
    }
}