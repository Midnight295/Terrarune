using Terrarune.Core.ModPlayers;
using Terraria;
using Terraria.ID;
using Terrarune.Common;

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
