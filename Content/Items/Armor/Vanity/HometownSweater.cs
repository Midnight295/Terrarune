using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrarune.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class HometownSweater : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 45;
            Item.height = 30;

            Item.value = Item.sellPrice(0,0,20,40);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FamiliarShirt, 1)
                .AddIngredient(ItemID.YellowDye, 1)
                .AddIngredient(ItemID.GreenDye, 1)
                .AddTile(TileID.Loom).
            Register();
        }
    }
}
