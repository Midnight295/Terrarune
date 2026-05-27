using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terrarune.Common;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class DustyCRT : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/DustyCRT/DustyCRT";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().DustyCRT = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().DustyCRT = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 10)
            .AddIngredient(ItemID.Glass, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
