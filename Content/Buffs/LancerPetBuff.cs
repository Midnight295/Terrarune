using Terraria;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Projectiles;

//lifted from examplemod
namespace Terrarune.Content.Buffs
{
	public class LancerPetBuff : ModBuff
	{
        public override string Texture => "Terrarune/Assets/Buffs/LancerPetBuff";

        public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{ // This method gets called every frame your buff is active on your player.
			bool unused = false;
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<LancerPetProjectile>());
		}
	}
}