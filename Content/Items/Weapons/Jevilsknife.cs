using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Projectiles;

namespace Terrarune.Content.Items.Weapons
{
    public class Jevilsknife : ModItem
    {
        public override string Texture => "Terrarune/Assets/Weapons/Jevilsknife";
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.damage = 30;
            Item.crit = 6;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.shoot = ModContent.ProjectileType<JevilsknifeProjectile>();
            Item.shootSpeed = 20;
            Item.knockBack = 2;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}
