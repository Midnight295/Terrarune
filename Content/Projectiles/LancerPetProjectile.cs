using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terrarune.Common;
using Terrarune.Content.Projectiles;
using Terrarune.Content.Buffs;
using Terrarune.Content.Items.Accessories.Vanity;
using Terrarune.Core.ModPlayers.DrawLayers;

namespace Terrarune.Content.Projectiles
{
	public class LancerPetProjectile : ModProjectile
	{
        public override string Texture => "Terrarune/Assets/Projectiles/LancerPetProjectile";

        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 17;
			Main.projPet[Type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Type] = ProjectileID.Sets.SimpleLoop(2, 8, 6)
				.WhenNotSelected(0, 0)
				.WithOffset(-9, 1f)
				.WithSpriteDirection(-1);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.CavelingGardener); 
			Projectile.width = 42;
			Projectile.height = 44;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
            Projectile.penetrate = -1;

            AIType = ProjectileID.CavelingGardener; 
        }



        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];

			player.zephyrfish = false; // Relic from AIType

            return true;
		}

		
        public int AnimationDelay => 15;

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
           
			if (player.Terrarune().SusieHairbrush || player.Terrarune().SusieChalk)
            {


                if (player.Terrarune().SusieLaughCounter == 85)
                {
				 //failed animation loop code (i tried like 5 different things i cant figure it out :sob:)
                    if (++Projectile.frameCounter >= 15)
                    {
                        Projectile.frameCounter = 0;
						
						Projectile.frame = 16;

                        if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        {
                            Projectile.frame = 17;
                        }
                    }

                    int index = CombatText.NewText(new Rectangle((int)Projectile.Center.X + (Projectile.direction * 32), (int)Projectile.Center.Y + 32, 2, 2), new Color(47, 136, 224, 225), "ho!", true);
					//Combat Text not in loop yet
                    SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/LancerLaugh"));
                }

            }
          


            // Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
            if (!player.dead && player.HasBuff(ModContent.BuffType<LancerPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}