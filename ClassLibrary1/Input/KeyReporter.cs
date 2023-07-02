namespace ClassLibrary1.Input;

public interface IKeyReportService : IKeyReporter
{
    /// <summary>
    /// IKeyReportServer.Execute();
    /// 
    /// == DESCRIPTION ==
    /// This does two things: 
    /// 
    /// It receives the list of reports at a point in time
    /// AND also it clears those reports from the registry.
    /// </summary>
    InputReport[] Execute();
}