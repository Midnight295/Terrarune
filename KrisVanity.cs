using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Content.Items.Accessories.Vanity
{
    public class KrisCage : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/KrisCage/KrisCage";

        public override void SetStaticDefaults()
        {
            // Registers a vertical animation with 4 frames and each one will last 5 ticks (1/12 second)
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation

        }  

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.Terrarune().KrisCage = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().KrisCage = true;
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


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 15)
            .AddRecipeGroup(RecipeGroupID.Fruit, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

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
            player.ReplaceItem(Item, ModContent.ItemType<KrisCage>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<KrisCage>());
        }

    }
}
