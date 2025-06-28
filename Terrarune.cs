using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Core.ModPlayers;

namespace Terrarune
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class Terrarune : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            int message = reader.Read7BitEncodedInt();
            switch (message)
            {
                case 0:
                    Player player = Main.player[reader.Read7BitEncodedInt()];
                    TerraruneModPlayer mp = player.Terrarune();
                    mp.ItemUsedPreviousFrame = reader.Read7BitEncodedInt();
                    mp.TargetRotation = reader.ReadSingle();
                    mp.UseStyleVar = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        mp.SyncPlayer(-1, player.whoAmI, false);
                    }
                    break;
            }
            base.HandlePacket(reader, whoAmI);
        }
    }
}
