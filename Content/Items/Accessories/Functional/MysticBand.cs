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
    internal class MysticBand : ModItem
    {
        public override string Texture => "Terrarune/Assets/Items/MysticBand";
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 2, 17);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BandofStarpower, 1);
            recipe.AddIngredient(ItemID.SilverBar, 3);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;

            base.UpdateAccessory(player, hideVisual);
        }
    }
}
