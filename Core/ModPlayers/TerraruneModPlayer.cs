using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Terrarune.Core.ModPlayers
{
    public class TerraruneModPlayer : ModPlayer
    {
        public bool KrisKnife;
        public bool SusieChalk;

        public override void FrameEffects()
        {
            if (KrisKnife)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, "KrisKnife", EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, "KrisKnife", EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, "KrisKnife", EquipType.Head);
            }

            if (SusieChalk)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, "SusieChalk", EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, "SusieChalk", EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, "SusieChalk", EquipType.Head);
            }
        }

        public override void ResetEffects()
        {
            KrisKnife = false;
            SusieChalk = false;
        }
    }
}
