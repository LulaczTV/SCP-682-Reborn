namespace SCP_682_Reborn
{
	using System.Collections.Generic;
	using Exiled.Events.EventArgs;
	using Exiled.Loader;
	using MEC;
	using SCP_682_Reborn.ConfigObjects;

	public class EventHandlers
	{
		private readonly Plugin _plugin;
		public EventHandlers(Plugin plugin) => _plugin = plugin;

		public bool TeslasDisabled = false;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

		public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
		{
			if (!_plugin.StopRagdollList.Contains(ev.Owner)) 
				return;
			
			ev.IsAllowed = false;
			_plugin.StopRagdollList.Remove(ev.Owner);
		}

		public void OnWaitingForPlayers()
		{
			if (_plugin.Config.SpawnType == InstanceType.Random && Loader.Random.Next(100) > 55)
				_plugin.Playable.Methods.Init();
			else
				_plugin.Playable.Methods.Init();
		}
	}
}
