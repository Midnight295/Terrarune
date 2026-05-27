using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Content.Projectiles
{
    public class JevilsknifeProjectile : ModProjectile
    {
        public override string Texture => "Terrarune/Assets/Weapons/Jevilsknife";
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.scale = 0.8f;
            base.SetDefaults();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == -2)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), (Projectile.Center + target.Center)/2, Vector2.Zero, ModContent.ProjectileType<JevilBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.scale <= 1 ? 0 : 1);
                Projectile.ai[2] = -4;
            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void AI()
        {
            if (Projectile.owner < 0 || Main.player[Projectile.owner].dead || !Main.player[Projectile.owner].active || Main.player[Projectile.owner].Distance(Projectile.Center) > 3000)
            {
                Projectile.Kill();
            }
            Player owner = Main.player[Projectile.owner];
            int flightTime = 60;
            int readjustTime = 15;
            if (Projectile.ai[2] == 0 && Projectile.ai[0] == 0) {
                int[] randomChaoses = {
                40, //nothing
                20, // 4 directions
                20, //red
                19, //light
                1, //big light
            };
                int chaos = Main.rand.Next(1, 101);
                int j = 0;
                for (int i = 0; i < randomChaoses.Length; i++)
                {
                    j += randomChaoses[i];
                    if (chaos <= j)
                    {
                        chaos = i;
                        break;
                    }
                }
                switch (chaos)
                {
                    case 1:
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathF.PI / 2), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, MathF.PI / 2);
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathF.PI), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, MathF.PI);
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(-MathF.PI / 2), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, MathF.PI * 1.5f);
                        break;
                    case 2:
                        Projectile.scale *= 2;
                        Projectile.damage *= 2;
                        Projectile.ai[2] = -1;
                        break;
                    case 3:
                        Projectile.ai[2] = -2;
                        break;
                    case 4:
                        Projectile.ai[2] = -2;
                        Projectile.scale *= 3;
                        Projectile.damage *= 3;
                        break;
                }
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[1] = Projectile.velocity.ToRotation();
                Projectile.rotation = Projectile.ai[1];
                Projectile.velocity *= 0;
            }
            if (Projectile.ai[0] < readjustTime)
            {
                Projectile.localAI[0] = owner.AngleTo(Main.MouseWorld) + (Projectile.ai[2] >= 0 ?  Projectile.ai[2] : 0);
                
            }
            Projectile.ai[1] = Utils.AngleLerp(Projectile.ai[1], Projectile.localAI[0], 0.05f);
            if (Projectile.ai[0] >= flightTime)
            {
                Projectile.Kill();
            }
            Projectile.rotation += MathHelper.ToRadians(15);
            Projectile.ai[0]++;
            float dist = LerpHelper.LerpFloat(0, owner.HeldItem.shootSpeed * 20 * (Projectile.ai[2] <= -2 ? 1.5f : 1), Projectile.ai[0], flightTime, LerpHelper.LerpEasing.DownParabola);
            Projectile.Center = owner.Center + Projectile.ai[1].ToRotationVector2() * dist;
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> t = TextureAssets.Projectile[Type];
            Color color = lightColor;
            if (Projectile.ai[2] == -1)
            {
                color.R *= 4;
                color.G /= 4;
                color.B /= 4;
            }
            Main.EntitySpriteDraw(t.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, t.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
