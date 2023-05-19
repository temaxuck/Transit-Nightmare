using System.Collections.Generic;
using UnityEngine;

public enum ClickEvent {
    None,
    Play,
    Exit
}

public static class ClickEventHandlers {
    public static Dictionary<ClickEvent, IClickEventHandler> clickEventHandlers = new Dictionary<ClickEvent, IClickEventHandler> {
        { ClickEvent.None, null },
        { ClickEvent.Play, new PlayClickEventHandler() },
        { ClickEvent.Exit, new ExitClickEventHandler() }
    };

    public static IClickEventHandler Get(ClickEvent ce)
    {
        return clickEventHandlers[ce];
    }

    public interface IClickEventHandler {
        void Activate();
    }

    public class PlayClickEventHandler : IClickEventHandler {
        public void Activate() {
            Debug.Log("Play!");
        }
    }

    public class ExitClickEventHandler : IClickEventHandler {
        public void Activate() {
            Debug.Log("Exit!");
        }
    }
}