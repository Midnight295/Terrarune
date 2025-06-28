using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terrarune.Common;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class BerdlyGrade : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/BerdlyGrade/BerdlyGrade";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return incomingItem.type != ModContent.ItemType<FluffyHat>() &&
                incomingItem.type != ModContent.ItemType<HornedHeadband>() &&
                incomingItem.type != ModContent.ItemType<SusieChalk>() &&
                incomingItem.type != ModContent.ItemType<KrisKnife>();
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().BerdlyGrade = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().BerdlyGrade = true;
        }
    }
}
