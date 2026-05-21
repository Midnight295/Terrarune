using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terrarune.Content.Projectiles;
using Terrarune.Common;


namespace Terrarune.Assets.Weapons
{
	public class KrisSword : ModItem
	{
       
		public int attackType = 0; // keeps track of which attack it is
        
		public override void SetDefaults()
		{
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot; 
			Item.useTime = 30; 
			Item.useAnimation = 30; 
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Melee; 
			Item.damage = 12; 
			Item.knockBack = 9; 
			Item.crit = 6;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<KrisSwordProj>();
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1; 
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // overrides the swing to set the attack
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
            return false; // return false to prevent original projectile from being shot
        }

       

        public override bool MeleePrefix()
        {
            return true; 
        }

      

		
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.IronBar, 10)
				.AddRecipeGroup(RecipeGroupID.Wood, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}