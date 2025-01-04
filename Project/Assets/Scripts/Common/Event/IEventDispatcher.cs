using System;

public interface IEventDispatcher
{
    void AddEventListener(string type, Action<BaseEvent> listener);

    void RemoveEventListener(string type, Action<BaseEvent> listener);

    bool DispatchEvent(BaseEvent evt);
}