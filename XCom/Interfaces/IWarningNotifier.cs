using System;

namespace XCom.Interfaces
{
    public interface IWarningNotifier
    {
        event Action<string> HandleWarning;
    }
}