using System;
using StardewModdingAPI;

namespace JsonExporter
{
    public class ModEntry : Mod 
    {
        public override void Entry(IModHelper helper)
        {
            helper.ConsoleCommands.Add("export", "export all data", (cmd, args) =>
            {
                this.Monitor.Log("hello world!", LogLevel.Info);
            });
        }
    }
}