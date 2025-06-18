using Terrarune.Core.ModPlayers;
using Terraria;
using Terraria.ID;
using Terrarune.Common;
using Terraria.ModLoader;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class SusieChalk : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/SusieChalk/SusieChalk";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.vanity = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return incomingItem.type != ModContent.ItemType<FluffyHat>() &&
                incomingItem.type != ModContent.ItemType<KrisKnife>() &&
                incomingItem.type != ModContent.ItemType<HornedHeadband>();
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().SusieChalk = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().SusieChalk = true;
        }
    }
}
