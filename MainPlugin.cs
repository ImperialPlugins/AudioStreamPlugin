using System;
using System.Linq;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using SDG.Unturned.Community.Components.Audio;
using UnityEngine;

namespace AudioStreamPlugin
{
    public class MainPlugin : RocketPlugin
    {
        [RocketCommand(nameof(Play), "Play audio", "<url> <isStream:true/false>")]
        public void Play(IRocketPlayer player, string[] args)
        {
            if (args.Length != 2 || !bool.TryParse(args[1], out var isStream))
            {
                UnturnedChat.Say(player, "Usage: /play <url> <isStream:true/false>", Color.red);
                return;
            }

            var handle = AudioModule.Instance.Play(ESteamCall.CLIENTS, args[0], isStream);
            UnturnedChat.Say($"Playing[{(int)handle}]: {args[0]}", Color.green);
        }

        [RocketCommand(nameof(SetAudioVolume), "Set volume of audio", "<audio id> <volume>")]
        public void SetAudioVolume(IRocketPlayer player, string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var audioId) || !int.TryParse(args[1], out var volume))
            {
                UnturnedChat.Say(player, "Usage: /setvolume <audio id> <volume>", Color.red);
                return;
            }

            AudioModule.Instance.SetVolume(ESteamCall.CLIENTS, audioId, (float)volume/100);
            UnturnedChat.Say($"Audio[{audioId}] volume set to: {volume}", Color.green);
        }

        [RocketCommand(nameof(AttachPlayer), "Attach audio to player", "<audio id> <target player>")]
        public void AttachPlayer(IRocketPlayer player, string[] args)
        {
            SteamPlayer targetPlayer;
            if (args.Length != 2 || !int.TryParse(args[0], out var audioId) || 
                ((targetPlayer = Provider.clients.FirstOrDefault(c => c.playerID.playerName.StartsWith(args[1], StringComparison.OrdinalIgnoreCase))) == null))
            {
                UnturnedChat.Say(player, "Usage: /attachplayer <audio id> <target player>", Color.red);
                return;
            }

            AudioModule.Instance.AttachToPlayer(ESteamCall.CLIENTS, audioId, targetPlayer);
            UnturnedChat.Say($"Audio[{audioId}] attached to: {targetPlayer.playerID.playerName}", Color.green);
        }

        [RocketCommand(nameof(AttachVehicle), "Attach audio to vehicle", "<audio id>", AllowedCaller.Player)]
        public void AttachVehicle(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /attachvehicle <audio id>", Color.red);
                return;
            }

            if (!((UnturnedPlayer) player).IsInVehicle)
            {
                UnturnedChat.Say(player, "You need to be in a vehicle to use this command!", Color.red);
                return;
            }

            var veh = ((UnturnedPlayer) player).CurrentVehicle;

            AudioModule.Instance.AttachToVehicle(ESteamCall.CLIENTS, audioId, veh);
            UnturnedChat.Say($"Audio[{audioId}] attached to: {veh.instanceID}", Color.green);
        }

        [RocketCommand(nameof(Deattach), "Deattach audio", "<audio id>")]
        public void Deattach(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /deattach <audio id>", Color.red);
                return;
            }

            AudioModule.Instance.Deattach(ESteamCall.CLIENTS, audioId);
            UnturnedChat.Say($"Audio[{audioId}] deattached!", Color.green);
        }

        [RocketCommand(nameof(SetPos), "Set position of audio to your current location", "<audio id>", AllowedCaller.Player)]
        public void SetPos(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /setpos <audio id>", Color.red);
                return;
            }

            var pos = ((UnturnedPlayer) player).Position;

            AudioModule.Instance.SetPosition(ESteamCall.CLIENTS, audioId, pos);
            UnturnedChat.Say($"Audio[{audioId}] set to: X: {pos.x}, Y: {pos.y}, Z: {pos.z}", Color.green);
        }

        [RocketCommand(nameof(SetMaxDist), "Set max distance of audio", "<audio id> <distance>")]
        public void SetMaxDist(IRocketPlayer player, string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var audioId) || !float.TryParse(args[1], out var dist))
            {
                UnturnedChat.Say(player, "Usage: /setmaxdist <audio id> <distance>", Color.red);
                return;
            }

            AudioModule.Instance.SetMaxDistance(ESteamCall.CLIENTS, audioId, dist);
            UnturnedChat.Say($"Audio[{audioId}] MaxDistance set to: {dist}!", Color.green);
        }

        [RocketCommand(nameof(SetMinDist), "Set min distance of audio", "<audio id> <distance>")]
        public void SetMinDist(IRocketPlayer player, string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var audioId) || !float.TryParse(args[1], out var dist))
            {
                UnturnedChat.Say(player, "Usage: /setmindist <audio id> <distance>", Color.red);
                return;
            }

            AudioModule.Instance.SetMinDistance(ESteamCall.CLIENTS, audioId, dist);
            UnturnedChat.Say($"Audio[{audioId}] MinDistance set to: {dist}!", Color.green);
        }

        [RocketCommand(nameof(Resume), "Resume audio", "<audio id>")]
        public void Resume(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /resume <audio id>", Color.red);
                return;
            }

            AudioModule.Instance.Resume(ESteamCall.CLIENTS, audioId);
            UnturnedChat.Say($"Audio[{audioId}] resuming...", Color.green);
        }


        [RocketCommand(nameof(Pause), "Pause audio", "<audio id>")]
        public void Pause(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /pause <audio id>", Color.red);
                return;
            }

            AudioModule.Instance.Pause(ESteamCall.CLIENTS, audioId);
            UnturnedChat.Say($"Audio[{audioId}] pause...", Color.green);
        }


        [RocketCommand(nameof(Destroy), "Destroy audio", "<audio id>")]
        public void Destroy(IRocketPlayer player, string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out var audioId))
            {
                UnturnedChat.Say(player, "Usage: /destroy <audio id>", Color.red);
                return;
            }

            AudioModule.Instance.Destroy(ESteamCall.CLIENTS, audioId);
            UnturnedChat.Say($"Audio[{audioId}] destroying...", Color.green);
        }

        [RocketCommand(nameof(SetLoop), "Loop audio?", "<audio id> <loop:true/false>")]
        public void SetLoop(IRocketPlayer player, string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var audioId) || !bool.TryParse(args[1], out var loop))
            {
                UnturnedChat.Say(player, "Usage: /setloop <audio id> <loop:true/false>", Color.red);
                return;
            }

            AudioModule.Instance.SetLoop(ESteamCall.CLIENTS, audioId, loop);
            UnturnedChat.Say($"Audio[{audioId}] looping: {loop}", Color.green);
        }
    }
}
