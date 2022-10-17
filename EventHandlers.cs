namespace SCP_682_Reborn
{
	using System.Collections.Generic;
	using Exiled.CustomRoles.API.Features;
	using CustomRoles;
	using Exiled.Events.EventArgs;
	using Exiled.Loader;
	using MEC;

	public class EventHandlers
	{
		private readonly Plugin _plugin;

		public bool Scp682spawned = false;

		internal EventHandlers(Plugin plugin) => this._plugin = plugin;

		public void OnSpawning(SpawningEventArgs ev)
		{
			if (ev.Player.Role.Type == RoleType.Scp93989 && !Scp682spawned)
			{
				CustomRole.Get(682).AddRole(ev.Player);
				Scp682spawned = true;
			}
		}

		public void OnChangingRole(ChangingRoleEventArgs ev)
		{
			if (ev.Player.Role.Type == RoleType.Scp93989 && !Scp682spawned)
			{
				CustomRole.Get(682).AddRole(ev.Player);
				Scp682spawned = true;
			}
		}

		public void OnDied(DiedEventArgs ev)
        {
			if (ev.Target.Role.Type == RoleType.Scp93989)
            {
				Scp682spawned = false;
            }
        }

		public void OnRoundEnded(RoundEndedEventArgs ev)
        {
			Scp682spawned = false;
        }
	}
}