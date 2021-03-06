﻿using MapleServer2.Data.Static;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Levels
    {
        private readonly Player Player;
        public short Level { get; private set; }
        public long Exp { get; private set; }
        public long RestExp { get; private set; }
        public int PrestigeLevel { get; private set; }
        public long PrestigeExp { get; private set; }

        public Levels(Player player, short playerLevel, long exp, long restExp, int prestigeLevel, long prestigeExp)
        {
            Player = player;
            Level = playerLevel;
            Exp = exp;
            RestExp = restExp;
            PrestigeLevel = prestigeLevel;
            PrestigeExp = prestigeExp;
        }


        public void SetLevel(short level)
        {
            Level = level;
            Exp = 0;
            Player.Session.Send(ExperiencePacket.ExpUp(0, Exp, 0));
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, Level));
        }

        public bool LevelUp()
        {
            if (!ExpMetadataStorage.LevelExist((short) (Level + 1)))
            {
                return false;
            }

            Level++;
            // TODO: Gain max HP and heal to max hp
            Player.StatPointDistribution.AddTotalStatPoints(5);
            Player.Session.Send(StatPointPacket.WriteTotalStatPoints(Player));
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, Level));
            return true;
        }

        public void SetPrestigeLevel(int level)
        {
            PrestigeLevel = level;
            PrestigeExp = 0;
            Player.Session.Send(PrestigePacket.ExpUp(Player, 0));
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
        }

        public void PrestigeLevelUp()
        {
            PrestigeLevel++;
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
        }

        public void GainExp(int amount)
        {
            if (amount <= 0 && !ExpMetadataStorage.LevelExist((short) (Level + 1)))
            {
                return;
            }

            long newExp = Exp + amount + RestExp;

            if (RestExp > 0)
            {
                RestExp -= amount;

            }
            else
            {
                RestExp = 0;
            }

            while (newExp >= ExpMetadataStorage.GetExpToLevel(Level))
            {
                newExp -= ExpMetadataStorage.GetExpToLevel(Level);
                if (!LevelUp()) // If can't level up because next level doesn't exist, exit the loop
                {
                    newExp = 0;
                    break;
                }
            }

            Exp = newExp;
            Player.Session.Send(ExperiencePacket.ExpUp(amount, newExp, RestExp));
        }

        public void GainPrestigeExp(long amount)
        {
            if (Level < 50) // Can only gain prestige exp after level 50.
            {
                return;
            }
            // Prestige exp can only be earned 1M exp per day. 
            // TODO: After 1M exp, reduce the gain and reset the exp gained every midnight.

            long newPrestigeExp = PrestigeExp + amount;

            if (newPrestigeExp >= 1000000)
            {
                newPrestigeExp -= 1000000;
                PrestigeLevelUp();
            }

            PrestigeExp = newPrestigeExp;
            Player.Session.Send(PrestigePacket.ExpUp(Player, amount));
        }

    }
}
