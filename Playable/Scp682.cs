namespace SCP_682_Reborn.Playable
{
    using System.Collections.Generic;
    using System.Linq;
    using Assets._Scripts.Dissonance;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.CustomRoles.Commands;
    using CustomRoles.Abilities;
    using Exiled.Events.EventArgs;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;
    using YamlDotNet.Serialization;
    using RoleType = RoleType;
    using System.ComponentModel;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Interactables.Interobjects;

    /// <summary>
    /// The <see cref="CustomRole"/> handler for SCP-682.
    /// </summary>
    [CustomRole(RoleType.Scp93989)]
    public class Scp682 : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 682;


        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 5000;

        /// <inheritdoc />
        public override string Name { get; set; } = "SCP-682";

        /// <inheritdoc />
        public override string Description { get; set; } = "You are slower, but much stronger.";

        /// <inheritdoc />
        public override string CustomInfo { get; set; } = "SCP-682";

        /// <summary>
        /// Gets or sets a multiplier used to modify the player's movement speed (running and walking).
        /// </summary>
        public float MovementMultiplier { get; set; } = 0.5f;

        public int PryGateCooldown { get; set; } = 60;

        public int DestroyDoorCooldown { get; set; } = 60;

        public string PryGateCooldownMessage { get; set; } = "You must wait %time% before prying gate.";

        public string DestroyDoorCooldownMessage { get; set; } = "You must wait %time% before destroying door.";

        public int scp682_destroy_door_chance { get; set; } = 100;

        public bool can_PryGates { get; set; } = true;

        public bool scp682_can_destroy_door { get; set; } = true;

        public Exiled.API.Features.Broadcast BroadcastMessage { get; private set; } = new Exiled.API.Features.Broadcast("You are SCP-682. You running and walking slow, but you can use charge every 45 seconds. To use the ability, write in console cmdbind <key> .special", 40);

        /// <summary>
        /// Gets or sets the custom scale factor for players when they are this role.
        /// </summary>
        public override Vector3 Scale { get; set; } = new Vector3(1.10f, 1.10f, 1.10f);

        // The following properties are only defined so that we can add the YamlIgnore attribute to them so they cannot be changed via configs.
        /// <inheritdoc />
        [YamlIgnore]
        public override RoleType Role { get; set; } = RoleType.Scp93989;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ChargeAbility(),
            new HealOnKill
            {
                HealAmount = 150f
            }
        };

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new RoleSpawnPoint
                {
                    Role = RoleType.Scp93989,
                    Chance = 100,
                }
            }
        };

        [Description("How much percent scp 682 has to Spawn instead Scp939-89")]
        public int SpawnChance { get; set; } = 100;
        protected override void RoleAdded(Player player)
        {
            player.UnitName = "Scp682";
            player.CustomInfo = $"<color=red>SCP-682</color>";

            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
                player.IsGodModeEnabled = false;
                player.Broadcast(BroadcastMessage);
            });

            player.Scale = Scale;
            DissonanceUserSetup dissonance = player.GameObject.GetComponent<DissonanceUserSetup>();
            dissonance.EnableListening(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            dissonance.EnableSpeaking(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            dissonance.SCPChat = true;

            Timing.RunCoroutine(Appearance(player), $"{player.UserId}-appearance");

            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines($"{player.UserId}-appearance");
            player.Scale = Vector3.one;
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            });

            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && Check(ev.Attacker) && ev.Target.Role.Side != Side.Scp)
            {
                ev.Target.Kill(DamageType.Scp939);
            }
        }

        private void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs ev)
        {
            if (ev.Role.roleId == RoleType.Scp93989)
            {
                string message = $"scp 6 8 2 has been {ev.TerminationCause}";

                ev.IsAllowed = false;
                Cassie.Message(message);
            }
        }

        private IEnumerator<float> Appearance(Player player)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(20f);
                player.CustomInfo = $"<color=red>SCP-682</color>";
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Nickname;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.PowerStatus;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.UnitName;
            }
        }

        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
        public Dictionary<Player, DateTime> BreakDoorCooldowns = new Dictionary<Player, DateTime>();
        public Dictionary<Player, DateTime> PryGateCooldowns = new Dictionary<Player, DateTime>();
        public System.Random rnd = new System.Random();


        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Check(ev.Player) && ev.Door.Base is PryableDoor pdoor)
            {
                if (PryGateCooldowns.ContainsKey(ev.Player))
                {
                    DateTime cooldownTime = PryGateCooldowns[ev.Player] + TimeSpan.FromSeconds(PryGateCooldown);
                    if (DateTime.Now < cooldownTime)
                    {
                        ev.Player.ShowHint(PryGateCooldownMessage.Replace("%time%", Math.Round((cooldownTime - DateTime.Now).TotalSeconds, 2).ToString()));
                    }
                    else
                    {
                        PryGateCooldowns.Remove(ev.Player);

                        if (can_PryGates)
                        {
                            pdoor.TryPryGate();
                            PryGateCooldowns[ev.Player] = DateTime.Now;
                        }
                    }
                }
                else if (can_PryGates)
                {
                    pdoor.TryPryGate();
                    PryGateCooldowns[ev.Player] = DateTime.Now;
                }
            }
            else if (scp682_can_destroy_door && Check(ev.Player))
            {
                if (BreakDoorCooldowns.ContainsKey(ev.Player))
                {
                    DateTime cooldownTime = BreakDoorCooldowns[ev.Player] + TimeSpan.FromSeconds(DestroyDoorCooldown);
                    if (DateTime.Now < cooldownTime)
                    {
                        ev.Player.ShowHint(DestroyDoorCooldownMessage.Replace("%time%", Math.Round((cooldownTime - DateTime.Now).TotalSeconds, 2).ToString()));
                    }
                    else
                    {
                        BreakDoorCooldowns.Remove(ev.Player);

                        int d = rnd.Next(0, 101);
                        if (d <= scp682_destroy_door_chance)
                        {
                            ev.Door.BreakDoor();
                            BreakDoorCooldowns[ev.Player] = DateTime.Now;
                        }
                    }
                }
                else
                {
                    int d = rnd.Next(0, 101);
                    if (d <= scp682_destroy_door_chance)
                    {
                        ev.Door.BreakDoor();
                        BreakDoorCooldowns[ev.Player] = DateTime.Now;
                    }
                }
            }
        }
    }
}