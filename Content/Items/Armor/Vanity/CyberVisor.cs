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
    [AutoloadEquip(EquipType.Head)]
    public class CyberVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 27;
            Item.height = 18;

            Item.value = Item.sellPrice(0,0,20,40);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Goggles, 1)
                .AddIngredient(ItemID.OrangeDye, 1)
                .AddIngredient(ItemID.TealDye, 1)
                .AddTile(TileID.Loom).
            Register();
        }
    }
}
