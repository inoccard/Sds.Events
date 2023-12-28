namespace Sds.Events.Domain.Core;

public interface INotifierMessage
{
    void Add(string message);

    void AddRange(string[] messages);

    string[] GetMessages();

    bool IsValid();
}