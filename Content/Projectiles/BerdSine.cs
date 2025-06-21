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
using Terrarune.Common;

namespace Terrarune.Content.Projectiles
{
    public class BerdSine : ModProjectile
    {
        public override string Texture => "Terrarune/Assets/Projectiles/BerdSine";

        private static int TimeLeft = 1000;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.width = Projectile.height = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = TimeLeft;
            Projectile.scale = 2.3f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
            Projectile.penetrate = -1;
            base.SetDefaults();
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(point1);
            writer.WriteVector2(point2);
            writer.WriteVector2(point3);
            writer.WriteVector2(point4);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            point1 = reader.ReadVector2();
            point2 = reader.ReadVector2();
            point3 = reader.ReadVector2();
            point4 = reader.ReadVector2();
        }
        public Vector2 point1;
        public Vector2 point2;
        public Vector2 point3;
        public Vector2 point4;
        public override void AI()
        {
            //change to .owner when done testing
            Player owner = Main.player[Projectile.owner];
            if (owner.whoAmI == Main.myPlayer && Projectile.ai[0] == 0)
            {
                int dir = Main.rand.NextBool() ? 1 : -1;
                point2 = Main.MouseWorld;
                point1 = Projectile.Center + (owner.AngleTo(Main.MouseWorld).ToRotationVector2() * owner.Distance(Main.MouseWorld)  * 0.2f) + (owner.AngleTo(Main.MouseWorld) + MathHelper.PiOver2).ToRotationVector2() * (Main.rand.NextFloat(30, 200) * dir);
                point3 = point2 - (point1 - point2);
                point4 = Projectile.Center + (owner.AngleTo(Main.MouseWorld).ToRotationVector2() * owner.Distance(Main.MouseWorld) *2f) + (owner.AngleTo(Main.MouseWorld) + MathHelper.PiOver2).ToRotationVector2() * (Main.rand.NextFloat(30, 200) * -dir);
                Projectile.ai[0]++;
                Projectile.netUpdate = true;
            }
            if (Projectile.timeLeft % 8 == 0 && Projectile.timeLeft > TimeLeft - (Projectile.ai[2] * 0.66f) && Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/HaliberdFire") with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Volume = 0.3f}, Projectile.Center);
            }
            Vector2 targetPos = point1;
            if (Projectile.ai[0] == 1)
            {
                targetPos = point1;
                if (Projectile.Distance(point1) < 20)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 2)
            {
                targetPos = point2;
                if (Projectile.Distance(point2) < 20)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 3)
            {
                targetPos = point3;
                if (Projectile.Distance(point3) < 20)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 4)
            {
                targetPos = point4;
                if (Projectile.Distance(point4) < 20)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 5)
            {
                targetPos = Projectile.Center + Projectile.velocity * 10;
            }
            if (Projectile.ai[1] == 0 && Projectile.timeLeft > Projectile.oldPos.Length)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.AngleTo(targetPos).ToRotationVector2() * 23, 0.3f);
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Opacity = 0;
                if (Projectile.timeLeft > Projectile.oldPos.Length)
                {
                    Projectile.timeLeft = Projectile.oldPos.Length;
                }
                if (Projectile.oldPos[(int)Projectile.ai[2]-1] == Projectile.position)
                {
                    Projectile.Kill();
                }
            }
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.05f);
            base.AI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[1] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> t = TextureAssets.Projectile[Type];
            for (int i = 0; i < Projectile.ai[2]; i++)
            {
                if (i % 2 == 0 && (Projectile.ai[2] - i) < Projectile.timeLeft)
                {
                    float opacity = i > (Projectile.ai[2] * 0.66f) ? (1 - ((i - (Projectile.ai[2] * 0.66f)) / (Projectile.ai[2] * 0.33f))) : 1;
                    Main.EntitySpriteDraw(t.Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, lightColor * opacity, Projectile.oldRot[i], t.Size() / 2, Projectile.scale, SpriteEffects.None);
                }
            }
            Main.EntitySpriteDraw(t.Value, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, t.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
