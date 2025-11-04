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
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
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

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddTile(TileID.Loom)
            .Register();
        }
    }
}
