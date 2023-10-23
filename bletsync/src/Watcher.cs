using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bletsync.src;
public class Watcher
{
    public string path;
    public string changelogPath;
    Logger fslogger;

    public Watcher(string DirectoryPath)
    {
        path = DirectoryPath;
        changelogPath = Path.Combine(path, "changelog.bsync");
        fslogger = new(changelogPath);
    }
    public void Start()
    {
        using var watcher = new FileSystemWatcher(path);
        watcher.NotifyFilter = NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Size;
        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;
        watcher.Error += OnError;
        watcher.Filter = "";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        Console.ReadLine();
    }
    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.Name == "changelog.bsync")
            return;
        if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
            return;
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;
        string rpath = Path.GetRelativePath(path, e.FullPath);
        Console.WriteLine($"modified:\t{rpath}");
        fslogger.Log("modify", rpath);
    }
    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        if (e.Name == "changelog.bsync")
            return;
        string rpath = Path.GetRelativePath(path, e.FullPath);
        Console.WriteLine($"created:\t{rpath}");
        fslogger.Log("create", rpath);
    } 
    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        if (e.Name == "changelog.bsync")
            return;
        string rpath = Path.GetRelativePath(path, e.FullPath);
        Console.WriteLine($"deleted:\t{rpath}");
        fslogger.Log("delete", rpath);
    }
    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (e.Name == "changelog.bsync")
            return;
        string oldrpath = Path.GetRelativePath(path, e.OldFullPath);
        string rpath = Path.GetRelativePath(path, e.FullPath);
        Console.WriteLine($"renamed:\t{oldrpath} -> {rpath}");
        fslogger.Log("rename", oldrpath, rpath);
    }
    private static void OnError(object sender, ErrorEventArgs e)
    {
        Console.Write($"ERROR: {e.ToString}");
    }
}