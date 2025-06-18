using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Content.Projectiles
{
    public class RudeBuster : ModProjectile
    {
        public Color AfterimageColor;
        public float Timer;
        public float HomingTimer;
        public float AfterimageFade;
        public bool Homing = false;
        public override string Texture => "Terrarune/Assets/Projectiles/RudeBuster";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.height = 70;
            Projectile.width = 50;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 1;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (!Homing)
                    Projectile.velocity *= LerpHelper.LerpFloat(1, 1.05f, ++Timer * 0.2f, 1, LerpHelper.LerpEasing.InSine, 0, true);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].Distance(Projectile.Center) <= 250 && !Main.npc[i].friendly)
                    {
                        Projectile.velocity += Projectile.DirectionTo(Main.npc[i].Center);
                    }

                }
            }
            else
            {
                Projectile.velocity *= LerpHelper.LerpFloat(1, 0, ++Timer * 0.01f, 1, LerpHelper.LerpEasing.OutSine, 0, true);
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            
                
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/RudebusterHit"), target.Center);
            for (int i = 1; i < 5; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, new Vector2(9, 9).RotatedBy(i * MathHelper.TwoPi / 4), Projectile.type, 0, 0, Main.player[Projectile.owner].whoAmI, -1);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, new Vector2(11, 11).RotatedBy(i * MathHelper.TwoPi / 4), Projectile.type, 0, 0, Main.player[Projectile.owner].whoAmI, -1);
            }

            if (Main.player[Projectile.owner].altFunctionUse == 2)
                hit.Damage *= 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 origin2 = rectangle.Size() / 2f;
            float scale = 1;
            if (Projectile.ai[0] == 0)
            {
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    if (i < 3)
                        AfterimageColor = Color.MediumPurple;
                    else if (i < 5)
                        AfterimageColor = Color.HotPink;
                    else if (i < 6)
                        AfterimageColor = Color.Lerp(Color.Pink, Color.LightPink, i);
                    
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Rectangle?(rectangle), AfterimageColor, Projectile.oldRot[i], origin2, new Vector2(Projectile.scale - 0.2f, LerpHelper.LerpFloat(Projectile.scale - 0.2f, Projectile.scale - 0.2f / i, ++AfterimageFade * 0.015f, 6, LerpHelper.LerpEasing.InSine, 0, true)), Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                }
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(rectangle), Color.White, Projectile.rotation, origin2, 1.1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            }
            else
            {
                if (!Main.gamePaused)
                    scale = LerpHelper.LerpFloat(Projectile.scale, Projectile.scale * 0, ++AfterimageFade * 0.05f, 1, LerpHelper.LerpEasing.InSine, 0, true);               
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(rectangle), Color.White, Projectile.rotation, origin2, new Vector2(scale * 1.2f, Projectile.scale * 1.2f), Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                if (scale == 0)
                    Projectile.Kill();
            }
                
            return false;
        }
    }
}
