using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Logging;

namespace BeyondLauncherV2.Utilities
{
    internal class RPC
    {
        public static DiscordRpcClient? client;

        public static void StartRPC()
        {
            try
            {
                LoggingSystem.WriteToLog("Starting RPC..");

                client = new("1212433317425709216");

                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

                client.OnReady += (sender, e) =>
                {
                    LoggingSystem.WriteToLog($"RPC Ready for User: {e.User.Username}");
                };

                client.OnPresenceUpdate += (sender, e) =>
                {
                    LoggingSystem.WriteToLog($"RPC Presence Update");
                };

                client.Initialize();

                client.SetPresence(new RichPresence()
                {
                    Details = "Idling..",
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = "slurpy_chill_jawn_for_beyond_8k"
                    },
                    Buttons = new Button[]
                    {
                        new Button() {Label = "Join the Discord!", Url = "https://discord.gg/beyondmp"}
                    }
                });
            } catch
            {
                LoggingSystem.WriteToLog("RPC Failed to Start");
            }
        }

        public static void UpdateRPC(string details, bool bTimeStamp = false)
        {
            if (bTimeStamp)
            {
                client!.SetPresence(new RichPresence()
                {
                    Details = details,
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = "slurpy_chill_jawn_for_beyond_8k"
                    },
                    Buttons = new Button[]
                    {
                        new Button() {Label = "Join the Discord!", Url = "https://discord.gg/beyondmp"}
                    }
                });
            } else
            {
                client!.SetPresence(new RichPresence()
                {
                    Details = details,
                    Assets = new Assets()
                    {
                        LargeImageKey = "slurpy_chill_jawn_for_beyond_8k"
                    },
                    Buttons = new Button[]
                    {
                        new Button() {Label = "Join the Discord!", Url = "https://discord.gg/beyondmp"}
                    }
                });
            }
        }

        public static void StopRPC()
        {
            // dispose better than deinit, stop memoryleaks
            client!.Dispose();
        }
        public static void StopRPCDeInit()
        {
            client!.Deinitialize();
        }
    }
}
