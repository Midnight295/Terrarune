using Terrarune.Core.ModPlayers;
using Terraria;
using Terraria.ID;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class KrisKnife : VanityAccessory
    {
        public override string Texture => "deltarune/Assets/Items/Vanity/KrisKnife/KrisKnife";
        public override void Load()
        {   
            base.Load();         
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;

        }

        public override void UpdateVanity(Player player)
        {
            TerraruneModPlayer deltarune = player.GetModPlayer<TerraruneModPlayer>();
            deltarune.KrisKnife = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerraruneModPlayer deltarune = player.GetModPlayer<TerraruneModPlayer>();
            if (!hideVisual)
                deltarune.KrisKnife = true;
        }
    }
}
