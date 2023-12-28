using System.Collections.Generic;
using System.Linq;

namespace Sds.Events.Domain.Core;

public class NotifierMessage : INotifierMessage
{
    private readonly List<string> _messages;

    public NotifierMessage()
    {
        _messages = new List<string>();
    }

    public bool IsValid() => !_messages.Any();

    public void Add(string message) => _messages.Add(message);

    public void AddRange(string[] messages) => _messages.AddRange(messages);

    public string[] GetMessages() => _messages.ToArray();

    protected void Clear() => _messages.Clear();

}
