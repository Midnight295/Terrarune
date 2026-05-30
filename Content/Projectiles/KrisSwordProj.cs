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
using Terrarune.Common;


namespace Terrarune.Content.Projectiles
{
	public class KrisSwordProj : ModProjectile
	{
		//MOST OF THIS IS EXAMPLEMOD CODE, I CANT BE CREDITED FOR ANY OF IT - Zara
		
		//constants to determine the swing of the sword
		private const float SWINGRANGE = 1.34f * (float)Math.PI; // The angle a swing attack covers (300 deg)
		private const float FIRSTHALFSWING = 0.45f; // How much of the swing happens before it reaches the target angle (in relation to swingRange)
		private const float WINDUP = 0.15f; // How far back the player's hand goes when winding their attack (in relation to swingRange)
		private const float UNWIND = 0.10f; // When should the sword start disappearing

		
		private enum AttackStage // What stage of the attack is being executed, see functions found in AI for description
		{
			Prepare,
			Execute,
			Unwind
		}
		
		private AttackStage CurrentStage
		{
			get => (AttackStage)Projectile.localAI[0];
			set
			{
				Projectile.localAI[0] = (float)value;
				Timer = 0; // reset the timer when the projectile switches states
			}
		}

		// Variables to keep track of during runtime
		private ref float InitialAngle => ref Projectile.ai[1]; // Angle aimed in (with constraints)
		private ref float Timer => ref Projectile.ai[2]; // Timer to keep track of progression of each stage
		private ref float Progress => ref Projectile.localAI[1]; // Position of sword relative to initial angle
		private ref float Size => ref Projectile.localAI[2]; // Size of sword

		// We define timing functions for each stage, taking into account melee attack speed
		private float prepTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
		private float execTime => 11f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
		private float hideTime => 15f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

		public override string Texture => "Terrarune/Assets/Projectiles/KrisSwordProj";
		private Player Owner => Main.player[Projectile.owner];


        public override void SetStaticDefaults()
		{
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 55; 
			Projectile.height = 50; 
			Projectile.friendly = true; 
			Projectile.timeLeft = 10000; 
			Projectile.penetrate = -1; 
			Projectile.tileCollide = false; 
			Projectile.usesLocalNPCImmunity = true; 
			Projectile.localNPCHitCooldown = -1; 
			Projectile.ownerHitCheck = true; 
			Projectile.DamageType = DamageClass.Melee; 
		}
		
		public override void OnSpawn(IEntitySource source)
		{
			Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
			float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();
				if (Projectile.spriteDirection == 1)
				{
					
					targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1 / 3, (float)Math.PI * 1 / 6);
				}
				else
				{
					if (targetAngle < 0)
					{
						targetAngle += 2 * (float)Math.PI; 
					}

					targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
				}

				InitialAngle = targetAngle - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection; 
			
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((sbyte)Projectile.spriteDirection);
		}

       

        public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.spriteDirection = reader.ReadSByte();
		}

		

        public override void AI()
		{
           
                Owner.itemAnimation = 2;
			Owner.itemTime = 2;

			
			if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
			{
				Projectile.Kill();
				return;
			}
			
			
			switch (CurrentStage)
			{
				case AttackStage.Prepare:
					PrepareStrike();
					break;
				case AttackStage.Execute:
					ExecuteStrike();
					break;
				default:
					UnwindStrike();
					break;
			}

			SetSwordPosition();
			Timer++;

		}


		public override bool PreDraw(ref Color lightColor)
		{
			// Calculate origin of sword (hilt) based on orientation and offset sword rotation (as sword is angled in its sprite)
			Vector2 origin;
			float rotationOffset;
			SpriteEffects effects;

			if (Projectile.spriteDirection > 0)
			{
				origin = new Vector2(4, Projectile.height - 10f);
				rotationOffset = MathHelper.ToRadians(45f);
				effects = SpriteEffects.None;
			}
			else
			{
				origin = new Vector2(Projectile.width - 16f, Projectile.height - 7f);
				rotationOffset = MathHelper.ToRadians(135f);
				effects = SpriteEffects.FlipHorizontally;
			}

			//white mask for slash

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

			if (CurrentStage == AttackStage.Execute || CurrentStage == AttackStage.Unwind)
			{
				Texture2D MaskTexture = ModContent.Request<Texture2D>("Terrarune/Assets/Projectiles/KrisSwordMask").Value;

				float maskOpacity = (1.0f - Timer/hideTime);

				Main.spriteBatch.Draw(MaskTexture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity * maskOpacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

				
			}
            return false;
        }

     

		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
		}

		
		public override void CutTiles()
		{
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
			Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
		}

		public override bool? CanDamage()
		{
			if (CurrentStage == AttackStage.Prepare)
				return false;
			return base.CanDamage();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			
			modifiers.HitDirectionOverride = target.position.X > Owner.MountedCenter.X ? 1 : -1;

			
		}

	
		public void SetSwordPosition()
		{
			Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress; 

		
			Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(85f)); 
			Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); 

			
			if (Owner.gravDir == -1f)
			{
				Projectile.rotation = 0f - Projectile.rotation;
				armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
			}

			armPosition.Y += Owner.gfxOffY;
			Projectile.Center = armPosition;
			Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem);

			Owner.heldProj = Projectile.whoAmI;
		}

		
		private void PrepareStrike()
		{
			Progress = WINDUP * SWINGRANGE * (1f - Timer / prepTime); 
			Size = MathHelper.SmoothStep(0.8f, 1, Timer / prepTime);

			if (Timer >= prepTime)
			{
				SoundEngine.PlaySound(SoundID.Item1); // Play sword sound here since playing it on spawn is too early
				CurrentStage = AttackStage.Execute; 
			}
		}

		
		private void ExecuteStrike()
		{
			

			Progress = MathHelper.SmoothStep(0, SWINGRANGE, Timer / execTime);

				if (Timer >= execTime)
				{
					CurrentStage = AttackStage.Unwind;
					
				}
			
			
		}

		
		private void UnwindStrike()
		{
			

			Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);

			Size = 1f;

				if (Timer >= hideTime)
				{
					Projectile.Kill();
				}
			
			
		}

		



	}
}
