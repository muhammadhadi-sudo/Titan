namespace Titan.Controllers;

public class LogMessages
{
    private static readonly string fileName = "MessagesLog.txt";

    public void AppendLine(string line)
    {
        using var sw = File.AppendText(fileName);
        sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {line}");
    }
}