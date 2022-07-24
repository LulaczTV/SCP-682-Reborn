namespace SCP_682_Reborn.NestingObjects
{
    using SCP_682_Reborn.Playable;

    public class Playable
    {
        public Methods Methods { get; set; }
        public EventHandlers EventHandlers { get; set; }

        public Playable(Plugin plugin)
        {
            Methods = new Methods(plugin);
            EventHandlers = new EventHandlers(plugin);
        }
    }
}