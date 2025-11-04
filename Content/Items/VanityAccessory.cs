using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terrarune.Common;
using System.Security.Policy;

namespace Terrarune.Content.Items
{
    public abstract class VanityAccessory : ModItem
    {
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "Terrarune/Assets/Items/Vanity/" + this.Name + "/" + this.Name + "Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, "Terrarune/Assets/Items/Vanity/" + this.Name + "/" + this.Name + "Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, "Terrarune/Assets/Items/Vanity/" + this.Name + "/" + this.Name + "Legs", EquipType.Legs, this);
            }

        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {   
            if (incomingItem.ModItem != null && incomingItem.ModItem is VanityAccessory && incomingItem.type != Type)
            {
                return false;
            }
            return true;
        }

        public override void SetStaticDefaults()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;

            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }

        public virtual void ModifyDrawInfo(ref PlayerDrawSet drawInfo) { }
    }
}
