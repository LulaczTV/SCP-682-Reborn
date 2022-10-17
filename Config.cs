namespace SCP_682_Reborn
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using SCP_682_Reborn.Playable;

    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether of not debug messages are displayed in the console.")]
        public bool Debug { get; set; } = false;

        public Scp682 Scp682 { get; set; } = new Scp682();
    }
}