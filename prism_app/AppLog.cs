using System;
using System.Collections.Generic;
using prism_app.ViewModels;

namespace prism_app
{
    // TODO Переделать

    public class AppLog
    {
        private List<string> _entries = new List<string>();
        private MainWindowViewModel _trackObj;

        public AppLog()
        {
        }

        public void Log(string message)
        {
            _entries.Add(message);
            _trackObj.AppLog = GetLog();
        }

        public string GetLog()
        {
            return String.Join("\n", _entries.ToArray());
        }

        public void SetTrackProp(MainWindowViewModel obj, string propName)
        {
            _trackObj = obj;
        }
    }
}