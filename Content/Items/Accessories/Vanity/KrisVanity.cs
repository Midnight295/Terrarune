using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class KrisKnife : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/KrisKnife/KrisKnife";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.Terrarune().KrisKnife = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().KrisKnife = true;
        }

        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<KrisLW>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<KrisLW>());
        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 15)
            .AddRecipeGroup(RecipeGroupID.Fruit, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

    public class KrisLW : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/KrisLW/KrisLW";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.Terrarune().KrisLW = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().KrisLW = true;
        }

        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<KrisKnife>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<KrisKnife>());
        }

    }
}
