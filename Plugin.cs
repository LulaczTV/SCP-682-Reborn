namespace SCP_682_Reborn
{
	using System;
	using System.Collections.Generic;
	using Exiled.API.Features;
	using Exiled.CustomRoles.API;
	using Exiled.CustomRoles.API.Features;
	using MEC;
	using Server = Exiled.Events.Handlers.Server;

	public class Plugin : Plugin<Config>
	{
		public static Plugin Singleton;

		public override string Author { get; } = "pan_andrzej";
		public override string Name { get; } = "SCP-682-Reborn";
		public override string Prefix { get; } = "682";
		public override Version Version { get; } = new Version(2, 3, 1);
		public override Version RequiredExiledVersion { get; } = new Version(5, 3, 0);

		public EventHandlers EventHandlers { get; private set; }
		public List<Player> StopRagdollList { get; } = new List<Player>();

		public override void OnEnabled()
		{
			CustomRole.RegisterRoles();

			Singleton = this;
			EventHandlers = new EventHandlers(this);

			Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnSpawning;
			Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
			Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;


			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			CustomRole.UnregisterRoles();



			Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnSpawning;
			Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
			Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;

			EventHandlers = null;
			Singleton = null;

			base.OnDisabled();
		}
	}
}