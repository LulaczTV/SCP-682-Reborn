namespace SCP_682_Reborn
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using SCP_682_Reborn.ConfigObjects;

    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("The type of SCP-682 that will be used. Valid options:Playable")]
        public InstanceType SpawnType { get; set; } = InstanceType.Playable;

        [Description("The configs for playable instances of SCP-682.")]
        public PlayableConfig PlayableConfig { get; set; } = new PlayableConfig();

        [Description("Whether of not debug messages are displayed in the console.")]
        public bool Debug { get; set; } = false;
    }
}