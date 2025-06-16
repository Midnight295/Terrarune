using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Projectiles;

namespace Terrarune.Content.Items.Weapons
{
    public class Halbird : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.damage = 30;
            Item.crit = 6;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 65;
            Item.useStyle = -1;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 5);

            HalbirdUseStyle style = new();
            Item.SetStyle(style);
        }
        
    }
    public class HalbirdUseStyle : UseStyle
    {
        public float origRotation;
        public override void Usage(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center;
            //player.itemRotation = MathHelper.ToRadians(-45);
            int animation = player.itemAnimationMax - player.itemAnimation;
            float spinTime = 0.6f;
            float stabOutTime = 0.30f;
            if (animation == 0)
            {
                SetPlayerDirection(player);
                origRotation = player.AngleTo(Main.MouseWorld);
            }
            if (animation == (int)(player.itemAnimationMax * spinTime))
            {
                origRotation = player.AngleTo(Main.MouseWorld);

            }
            if (animation < player.itemAnimationMax * spinTime)
            {
                player.itemRotation = LerpHelper.LerpAngle(origRotation - MathHelper.ToRadians(75), player.AngleTo(Main.MouseWorld) + MathHelper.ToRadians(765), animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.OutSine);
                player.itemLocation = player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2()  * LerpHelper.LerpFloat(30, -10, animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.InOutQuint);
            }else if (animation < player.itemAnimationMax * (1- stabOutTime))
            {
                
                player.itemLocation = LerpHelper.LerpVector2(player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2() * -10, player.Center + origRotation.ToRotationVector2() * 40, animation, player.itemAnimationMax * (1 - (spinTime + stabOutTime)), LerpHelper.LerpEasing.InOutSine, player.itemAnimationMax * spinTime);
            }
            else
            {
                player.itemLocation = player.Center + origRotation.ToRotationVector2() * 40;
            }
            if (animation == (int)(player.itemAnimationMax * (1 - stabOutTime)))
            {
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center + origRotation.ToRotationVector2() * 80, player.AngleTo(Main.MouseWorld).ToRotationVector2() * 20, ModContent.ProjectileType<BerdSine>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem));
            }
        }
        public override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int animation = player.itemAnimationMax - player.itemAnimation;
            float spinTime = 0.6f;
            float stabOutTime = 0.30f;
            SpriteEffects effect = SpriteEffects.None;
            //if (animation > player.itemAnimationMax * (spinTime + stabOutTime)) effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            //has 13 frames
            Asset<Texture2D> t = ModContent.Request<Texture2D>("Terrarune/Content/Items/Weapons/Halbird-Sheet");
            Rectangle frame = new Rectangle(0, 0, t.Width(), t.Height() / 13);
            if (animation > player.itemAnimationMax * spinTime)
            {
                int frameY = 0;
                frameY = (int)LerpHelper.LerpFloat(0, 12, animation, player.itemAnimationMax * (1 - (spinTime + stabOutTime)), LerpHelper.LerpEasing.Linear, player.itemAnimationMax * spinTime);
                frame.Y = t.Height() / 13 * frameY;
            }
            DoTheDraw(ref drawInfo, t, player.itemLocation - Main.screenPosition, frame, ColorAtHand(drawInfo), player.itemRotation, new Vector2(t.Width(),t.Height()/13)/2, player.GetAdjustedItemScale(player.HeldItem), effect);
        }
        public override void Hitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            int scale = (int)(player.GetAdjustedItemScale(player.HeldItem) * 10);
            hitbox = new Rectangle((int)player.itemLocation.X + scale/2 , (int)player.itemLocation.Y + scale/2, scale, scale);
        }
    }
}
