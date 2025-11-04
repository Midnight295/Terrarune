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
    public class SilverWatch : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/SilverWatch/SilverWatch";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().SilverWatch = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().SilverWatch = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SilverWatch)
            .AddIngredient(ItemID.SnowBlock, 25)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
