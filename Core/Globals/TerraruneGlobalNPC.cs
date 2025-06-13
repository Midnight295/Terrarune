using Terraria;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Core.Globals
{
    public class TerraruneGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if(npc.boss)
            {
                foreach(Player p in Main.ActivePlayers)
                {
                    if (p.dead || !p.Terrarune().SusieChalk)
                        continue;

                    p.Terrarune().SusieLaughCounter = 90;
                }
            }
        }
    }
}
