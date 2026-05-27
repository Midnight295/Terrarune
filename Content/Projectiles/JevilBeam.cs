using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
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
    public class JevilBeam : ModProjectile
    {
        public override string Texture => "Terraria/Images/Extra_" + ExtrasID.LightDisc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
            base.SetStaticDefaults();
        }
        const int time = 60;
        const float growTime = 0.3f;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 10;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            base.SetDefaults();
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 30)
            {
                float pitch = Projectile.ai[0] == 0 ? -0.5f : -1.5f; ;

                SoundEngine.PlaySound(SoundID.Item12 with { Pitch = pitch }, Projectile.Center);
            }
            float maxscale = Projectile.ai[0] == 0 ? 0.5f : 1.5f;
            float scale = 0;
            if (Projectile.timeLeft > (30 * (1 - growTime)))
            {
                scale = LerpHelper.LerpFloat(0.1f, maxscale, 30 - Projectile.timeLeft, 30 * growTime, LerpHelper.LerpEasing.OutQuint);
            }
            else
            {
                scale = LerpHelper.LerpFloat(maxscale, 0, 30 - Projectile.timeLeft, 30 * (1 - growTime), LerpHelper.LerpEasing.InQuint, 30 * growTime);
            }
            Projectile.Resize((int)(scale * 50), 1000);
            base.AI();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> t = TextureAssets.Projectile[Type];
            float maxscale = Projectile.ai[0] == 0 ? 0.5f : 1.5f;
            float scale = 0;
            if (Projectile.timeLeft > (30 * (1 - growTime))) {
                scale = LerpHelper.LerpFloat(0.1f, maxscale, 30 - Projectile.timeLeft, 30 * growTime, LerpHelper.LerpEasing.OutQuint);
            }
            else
            {
                scale = LerpHelper.LerpFloat(maxscale, 0, 30 - Projectile.timeLeft, 30 *  (1 - growTime), LerpHelper.LerpEasing.InQuint, 30 * growTime);
            }
            if (!Main.gamePaused)

            Main.EntitySpriteDraw(t.Value, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 }, MathF.PI / 2, t.Size() / 2, new Vector2(30, scale), SpriteEffects.None);
                return false;
        }
    }
}
