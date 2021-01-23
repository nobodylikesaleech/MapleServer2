using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using System;


namespace MapleServer2.Packets
{
    class SendStatPacket
    {
        public static Packet WriteCharacterStats(GameSession session, Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);

            PlayerStat Str = CalcPlayerStats(character, character.Stats.Str, StatType.Str);
            PlayerStat Dex = CalcPlayerStats(character, character.Stats.Dex, StatType.Dex);
            PlayerStat Int = CalcPlayerStats(character, character.Stats.Int, StatType.Int);
            PlayerStat Luk = CalcPlayerStats(character, character.Stats.Luk, StatType.Luk);
            PlayerStat Hp = CalcPlayerStats(character, character.Stats.Hp, StatType.Hp);
            PlayerStat CritRate = CalcPlayerStats(character, character.Stats.CritRate, StatType.CritRate);

            // onject id
            pWriter.WriteInt(session.FieldPlayer.ObjectId);

            pWriter.WriteByte(1); // mode (?) // mode 0 = load, mode 1 = update (maybe?)
            pWriter.WriteByte(35); // total number of stat types

            // Str
            pWriter.WriteInt(Str.Total);
            pWriter.WriteInt(Str.Min);
            pWriter.WriteInt(Str.Max);

            // Dex
            pWriter.WriteInt(Dex.Total);
            pWriter.WriteInt(Dex.Min);
            pWriter.WriteInt(Dex.Max);

            // Int
            pWriter.WriteInt(Int.Total);
            pWriter.WriteInt(Int.Min);
            pWriter.WriteInt(Int.Max);

            // Luk
            pWriter.WriteInt(Luk.Total);
            pWriter.WriteInt(Luk.Min);
            pWriter.WriteInt(Luk.Max);

            // Hp
            pWriter.WriteLong(Hp.Total);
            pWriter.WriteLong(Hp.Min);
            pWriter.WriteLong(Hp.Max); // Hp Current

            // Hp Regen
            pWriter.WriteInt(10);
            pWriter.WriteInt(10);
            pWriter.WriteInt(10);

            // Hp Regen Interval
            pWriter.WriteInt(3000);
            pWriter.WriteInt(3000);
            pWriter.WriteInt(3000);

            // Spirit
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);
            pWriter.WriteInt(62); // Spirit Current

            // Spirit Regen
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);

            // Spirit Regen Interval
            pWriter.WriteInt(200);
            pWriter.WriteInt(200);
            pWriter.WriteInt(200);

            // Stamina
            pWriter.WriteInt(120);
            pWriter.WriteInt(120);
            pWriter.WriteInt(90); // Stamina Current

            // Stamina Regen
            pWriter.WriteInt(10);
            pWriter.WriteInt(10);
            pWriter.WriteInt(10);

            // Stamina Regen Interval
            pWriter.WriteInt(500);
            pWriter.WriteInt(500);
            pWriter.WriteInt(500);

            // Atk Spd
            pWriter.WriteInt(120);
            pWriter.WriteInt(100);
            pWriter.WriteInt(120); // current atk spd

            // Move Spd
            pWriter.WriteInt(110);
            pWriter.WriteInt(100);
            pWriter.WriteInt(109); // currrent move spd

            // Acc
            pWriter.WriteInt(94);
            pWriter.WriteInt(82);
            pWriter.WriteInt(94);

            // Eva
            pWriter.WriteInt(80);
            pWriter.WriteInt(80);
            pWriter.WriteInt(80);

            // Crit Rate
            pWriter.WriteInt(CritRate.Total);
            pWriter.WriteInt(CritRate.Min);
            pWriter.WriteInt(CritRate.Max);

            // Crit Dmg
            pWriter.WriteInt(1038);
            pWriter.WriteInt(250);
            pWriter.WriteInt(1038);

            // Crit Eva
            pWriter.WriteInt(50);
            pWriter.WriteInt(50);
            pWriter.WriteInt(50);

            // Def
            pWriter.WriteInt(73891);
            pWriter.WriteInt(80);
            pWriter.WriteInt(73891);

            // Guard
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);

            // Jump Height
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);

            // Phys Atk
            pWriter.WriteInt(1026);
            pWriter.WriteInt(256);
            pWriter.WriteInt(1026);

            // Mag Atk
            pWriter.WriteInt(9);
            pWriter.WriteInt(8);
            pWriter.WriteInt(9);

            // Phys Res
            pWriter.WriteInt(196);
            pWriter.WriteInt(15);
            pWriter.WriteInt(196);

            // Mag Res
            pWriter.WriteInt(264);
            pWriter.WriteInt(13);
            pWriter.WriteInt(264);

            // Min Atk
            pWriter.WriteInt(249147);
            pWriter.WriteInt(0);
            pWriter.WriteInt(249147);

            // Max Atk
            pWriter.WriteInt(354805);
            pWriter.WriteInt(0);
            pWriter.WriteInt(354805);

            // Unk 30 - Min Dmg?
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);

            // Unk 31 - Max Dmg?
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);

            // Pierce
            pWriter.WriteInt(21);
            pWriter.WriteInt(0);
            pWriter.WriteInt(21);

            // Mount Speed
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);

            // Bonus Atk
            pWriter.WriteInt(2750);
            pWriter.WriteInt(0);
            pWriter.WriteInt(2750);

            // Pet Atk
            pWriter.WriteInt(3150);
            pWriter.WriteInt(0);
            pWriter.WriteInt(3150);

            return pWriter;
        }

        public static PlayerStat CalcPlayerStats(Player character, PlayerStat StatName, StatType StatType)
        {
            int StatCount;
            if (!character.StatPointDistribution.AllocatedStats.TryGetValue((byte) StatType, out StatCount))
            {
                StatCount = 0;
            }

            PlayerStat Stat = new PlayerStat
                (
                StatName.Total + StatCount,
                StatName.Min + StatCount,
                StatName.Max + StatCount
                );

            Console.WriteLine("StatType: " + StatType + ", StatName: " + StatName + ", Stat (Total=" + Stat.Total + ", Min=" + Stat.Min + ", Max=" + Stat.Max +")");

            return Stat;
        }
    }
}
