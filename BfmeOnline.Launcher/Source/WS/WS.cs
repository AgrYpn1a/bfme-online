using BfmeOnline.Launcher.Source.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;

namespace BfmeOnline.Launcher.Source.WS
{
    [System.Serializable]
    public enum WSEvent
    {
        ERROR = -1,
        MESSAGE = 0,
        GETTER = 1,
        CONNECTION_OPENED = 2,
        CONNECTION_CLOSED = 3,
        CMD = 4
    }

    [System.Serializable]
    public enum WSResult
    {
        NONE = -1,
        ONLINE_PLAYERS = 0,
        QUEUED = 1,
        MATCH_FOUND = 2
    }

    [System.Serializable]
    public struct WSMessage
    {
        public WSEvent EventType { get; set; }
        public WSResult ResultType { get; set; }
        public string Message { get; set; }
    }


    public sealed class WS
    {
        private static readonly object _lock = new object();
        private static WS _instance = null;

        public static WS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new WS();
                    }
                }

                return _instance;
            }
        }

        private WS() { }

        private WebSocket _ws;

        public void EstablishWebSocketConnection()
        {
            var headers = new Dictionary<string, string>()
            {
                { "auth-token", Player.AuthToken }
            };

            _ws = new WebSocket("ws://localhost:8080", "", null, headers.ToList());

            Logger.LogMessage("Trying to connect to WS Server...");

            _ws.MessageReceived += Ws_MessageReceived;
            _ws.Opened += Ws_Opened;
            _ws.Error += Ws_Error;
            _ws.Closed += Ws_Closed;
            _ws.Open();
        }

        private void Ws_Closed(object sender, EventArgs e)
        {
            Logger.LogMessage("Connection closed.", "WS", Logger.LogType.Error);
        }

        private void Ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Logger.LogMessage(e.Exception.Message, "WS", Logger.LogType.Error);
        }

        private void Ws_Opened(object sender, EventArgs e)
        {
            Logger.LogMessage("WS Connected.", "WS", Logger.LogType.Default);

            if (_handlers.ContainsKey(WSEvent.CONNECTION_OPENED))
                _handlers[WSEvent.CONNECTION_OPENED]?.Invoke(default);
        }

        private void Ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            WSMessage message = JsonConvert.DeserializeObject<WSMessage>(e.Message);

            if (_handlers.ContainsKey(message.EventType))
                _handlers[message.EventType]?.Invoke(message);

            Logger.LogMessage(message.Message, "WS", Logger.LogType.Default);
        }

        private Dictionary<WSEvent, Action<WSMessage>> _handlers = new Dictionary<WSEvent, Action<WSMessage>>();

        public void RegisterHandler(WSEvent eventType, Action<WSMessage> handler) => _handlers.Add(eventType, handler);

        public void SendMessage(WSMessage message) => _ws.Send(JsonConvert.SerializeObject(message));

    }
}
