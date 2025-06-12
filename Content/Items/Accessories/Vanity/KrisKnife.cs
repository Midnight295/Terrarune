﻿using Terrarune.Core.ModPlayers;
using Terraria;
using Terraria.ID;
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
    }
}
