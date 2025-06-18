using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrarune.Content.Items.Accessories.Functional
{
    public class SilverCard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 1, 0); 
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
            Item.defense = 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<AmberCard>(2);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }
    }
}
