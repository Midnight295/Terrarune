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
    public class FluffyHat : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/FluffyHat/FluffyHat";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().FluffyHat = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().FluffyHat = true;
        }

        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<HornedHeadband>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<HornedHeadband>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .Register();
        }
    }

    public class HornedHeadband : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/HornedHeadband/HornedHeadband";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().HornedHeadband = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().HornedHeadband = true;
        }
        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<FluffyHat>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<FluffyHat>());
        }

    }
}
