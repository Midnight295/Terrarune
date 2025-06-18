using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Content.Projectiles;

namespace Terrarune.Content.Items.Weapons
{
    public class ManeAx : ModItem
    {
        public override string Texture => "Terrarune/Assets/Weapons/ManeAx";

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 5;
            Item.reuseDelay = 5;
            Item.shootSpeed = 5;
            Item.shoot = ModContent.ProjectileType<RudeBuster>();
            Item.UseSound = new SoundStyle("Terrarune/Assets/Sounds/RudebusterSwing");
        }

        public override bool AltFunctionUse(Player player) => true;

    }
}
