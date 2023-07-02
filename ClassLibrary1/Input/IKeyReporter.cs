using System.Text.Json;

namespace ClassLibrary1.Input;

// TODO(ClassLibrary1.KeyReporter)
// 
// This will report key presses when it detects them.
public interface IKeyReporter
{
    void Report(InputReport inputReport);
}
public class KeyReporter : IKeyReporter
{
    public void Report(InputReport inputReport)
    {
        Console.WriteLine(JsonSerializer.Serialize(inputReport));
    }
}
