using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Content.Projectiles;
using Terrarune.Common;
using Terrarune.Content.Buffs;

//lifted from examplemod
namespace Terrarune.Content.Items.Pets
{
	public class LancerPet : ModItem
	{
		public override string Texture => "Terrarune/Assets/Items/PetItems/LancerPet";

        // Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.PigPetItem); // Copy the Defaults of the PigPet Item.

			Item.shoot = ModContent.ProjectileType<LancerPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<LancerPetBuff>(); // Apply buff upon usage of the Item.
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(Item.buffType, 3600);
			}
			return true;
		}

	}
}
