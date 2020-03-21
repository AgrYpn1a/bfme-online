using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BfmeOnline.Launcher.Source
{
    public sealed class InterProcessCommunicator
    {
        private Process _process;
        private ConcurrentDictionary<string, Action<string>> _callbacks = new ConcurrentDictionary<string, Action<string>>();

        public InterProcessCommunicator(Process process, Action<string> outCallback)
        {
            _process = process;

            // Default output callback
            while (!_callbacks.TryAdd("STDOUT", outCallback)) { }

            Logger.LogMessage("Created InterProcessCommunicator.");
        }

        public void Run()
        {
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardInput = true;

            _process.StartInfo.UseShellExecute = false;
            _process.EnableRaisingEvents = true;

            // Bind events
            _process.OutputDataReceived += Process_OutputDataReceived;

            // Start process
            _process.Start();

            // Begin reading
            _process.BeginErrorReadLine();
            _process.BeginOutputReadLine();

            _process.WaitForExit();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string message = e.Data;

            if (message != null && message != string.Empty)
            {
                // Parse message
                var parts = message.Split('#');

                // Do not handle this format for now
                if (parts.Length <= 1)
                    return;

                var id = parts[1];
                var res = parts[2];

                // Call response
                if (_callbacks.ContainsKey(id))
                {
                    _callbacks[id]?.Invoke(res);

                    Action<string> ac;
                    if (id != "STDOUT")
                        _callbacks.TryRemove(id, out ac);
                }
            }
        }

        public void ExecuteCommand(string cmd, Action<string> response)
        {
            var id = Guid.NewGuid().ToString();
            string message = $"cmd#{id}#{cmd}";

            // Send to process
            while (!_callbacks.TryAdd(id, response)) { }
            _process.StandardInput.WriteLine(message);
        }

        public void SetData(Dictionary<string, string> data, Action<string> response)
        {
            var id = Guid.NewGuid().ToString();

            string msgData = string.Empty;
            if (data != null)
                msgData = string.Join(";", data.Select(xy => $"{xy.Key}={xy.Value}"));

            string message = $"set#{id}#{msgData}";

            // Send to process
            while (!_callbacks.TryAdd(id, response)) { }
            _process.StandardInput.WriteLine(message);
        }

        public void GetData(string name, Action<string> response)
        {

        }

    }
}
