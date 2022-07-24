namespace SCP_682_Reborn.Playable
{
    using System.Linq;
    using Exiled.Events.Handlers;
    using Exiled.Loader;
    using Player = Exiled.API.Features.Player;

    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => _plugin = plugin;

        public void Init()
        {
            Server.RoundStarted += _plugin.Playable.EventHandlers.OnRoundStarted;
        }

        public void Disable()
        {
            Server.RoundStarted -= _plugin.Playable.EventHandlers.OnRoundStarted;
        }

        public void TrySpawn682()
        {
            if (Loader.Random.Next(100) <= _plugin.Config.PlayableConfig.Scp682.SpawnChance)
            {
                Player player = Player.Get(RoleType.Scp93989).FirstOrDefault();
                if (player == null)
                    return;

                _plugin.Config.PlayableConfig.Scp682.AddRole(player);
                Disable();
            }
        }
    }
}