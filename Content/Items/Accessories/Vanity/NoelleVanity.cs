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
    public class NoelleWatch : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/NoelleWatch/NoelleWatch";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().NoelleWatch = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().NoelleWatch = true;
        }

        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<NoellePencil>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<NoellePencil>());
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


    public class NoellePencil : VanityAccessory
    {
        public override string Texture => "Terrarune/Assets/Items/Vanity/NoellePencil/NoellePencil";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.Terrarune().NoellePencil = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.Terrarune().NoellePencil = true;
        }

        public override bool CanRightClick() => true;
        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<NoelleWatch>());
            return false;
        }

        public override void RightClick(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Equip"));
            player.ReplaceItem(Item, ModContent.ItemType<NoelleWatch>());
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
