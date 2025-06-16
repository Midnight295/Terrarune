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
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Projectiles;

namespace Terrarune.Content.Items.Weapons
{
    public class Haliberd : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.damage = 30;
            Item.crit = 6;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 65;
            Item.useStyle = -1;
            Item.UseSound = new Terraria.Audio.SoundStyle("Terrarune/Assets/Sounds/SpearTelegraph");
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.channel = true;
            Item.reuseDelay = 10;
            HaliberdUseStyle style = new();
            Item.SetStyle(style);
        }
        public override bool? UseItem(Player player)
        {
            
            return base.UseItem(player);
        }
    }
    public class HaliberdUseStyle : UseStyle
    {
        public float origRotation;
        public int SineLength = 30;
        public override void Usage(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center;
            //player.itemRotation = MathHelper.ToRadians(-45);
            int animation = player.itemAnimationMax - player.itemAnimation;
            float spinTime = 0.6f;
            float stabOutTime = 0.30f;
            if (player.itemTime == 2 && player.controlUseItem)
            {
                player.itemAnimation = (int)(player.itemAnimationMax * 0.4f);
                player.reuseDelay = 10;
                player.itemTime = 0;
                origRotation = player.AngleTo(Main.MouseWorld);
                player.itemRotation = origRotation + MathHelper.ToRadians(45);
                SineLength -= 2;
                if (SineLength < 2) SineLength = 2;
            }
            if (player.itemTime == 1) SineLength = 30;
            if (animation == 0)
            {
                SetPlayerDirection(player);
                origRotation = player.AngleTo(Main.MouseWorld);
            }
            
            if (animation == (int)(player.itemAnimationMax * (spinTime - 0.1f)))
            {
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Ding") with { Pitch = -0.4f, Volume = 0.5f}, player.Center);
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Ding") with { Pitch = -0.2f, Volume = 0.5f }, player.Center);
            }
            if (animation == (int)(player.itemAnimationMax * spinTime))
            {
                SetPlayerDirection(player);
                origRotation = player.AngleTo(Main.MouseWorld);
                player.itemRotation = origRotation + MathHelper.ToRadians(45);
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/CriticalSwing"), player.Center);
            }
            if (animation < player.itemAnimationMax * spinTime)
            {
                SetPlayerDirection(player);
                player.itemRotation = LerpHelper.LerpAngle(origRotation - MathHelper.ToRadians(player.direction == 1 ? 75 : -165), player.AngleTo(Main.MouseWorld) + MathHelper.ToRadians(player.direction == 1 ? 765 : -675), animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.OutSine);
                player.itemLocation = player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2()  * LerpHelper.LerpFloat(30, -10, animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.InOutQuint);
            }else if (animation < player.itemAnimationMax * (1- stabOutTime))
            {
                
                player.itemLocation = LerpHelper.LerpVector2(player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2() * -10, player.Center + origRotation.ToRotationVector2() * 40, animation, player.itemAnimationMax * (1 - (spinTime + stabOutTime)), LerpHelper.LerpEasing.InOutSine, player.itemAnimationMax * spinTime);
            }
            else
            {
                player.itemLocation = player.Center + origRotation.ToRotationVector2() * 40;
            }
            if (player.itemTime > 0)
            {
                player.itemLocation = LerpHelper.LerpVector2( player.Center + origRotation.ToRotationVector2() * 40, player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2() * -10, player.HeldItem.reuseDelay - player.itemTime, player.HeldItem.reuseDelay, LerpHelper.LerpEasing.InOutSine);
            }
            
            if (animation == (int)(player.itemAnimationMax * (1 - stabOutTime)))
            {
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center + origRotation.ToRotationVector2() * 80, player.AngleTo(Main.MouseWorld).ToRotationVector2() * 20, ModContent.ProjectileType<BerdSine>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem), ai2: SineLength);
            }
            Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.None;
            if (animation > player.itemAnimationMax * (spinTime + 0.04f))
            {
                stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
            }if (animation > player.itemAnimationMax * (spinTime + 0.07f))
            {
                stretch = Player.CompositeArmStretchAmount.Full;
            }
            player.SetCompositeArmFront(true, stretch, player.itemRotation - MathHelper.ToRadians(135));
        }
        public override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int animation = player.itemAnimationMax - player.itemAnimation;
            float spinTime = 0.6f;
            float stabOutTime = 0.30f;
            SpriteEffects effect = SpriteEffects.None;
            if (player.direction == -1) effect = SpriteEffects.FlipHorizontally;
            //has 13 frames
            Asset<Texture2D> t = ModContent.Request<Texture2D>("Terrarune/Content/Items/Weapons/Haliberd-Sheet");
            Rectangle frame = new Rectangle(0, 0, t.Width(), t.Height() / 13);
            if (animation > player.itemAnimationMax * spinTime)
            {
                int frameY = 0;
                frameY = (int)LerpHelper.LerpFloat(0, 12, animation, player.itemAnimationMax * (1 - (spinTime + stabOutTime)), LerpHelper.LerpEasing.Linear, player.itemAnimationMax * spinTime);
                frame.Y = t.Height() / 13 * frameY;
            }
            DoTheDraw(ref drawInfo, t, player.itemLocation - Main.screenPosition, frame, ColorAtHand(drawInfo), player.itemRotation + (player.direction == 1 ? 0 : MathF.PI/2), new Vector2(t.Width(),t.Height()/13)/2, player.GetAdjustedItemScale(player.HeldItem), effect);
        }
        public override void Hitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            int scale = (int)(player.GetAdjustedItemScale(player.HeldItem) * 10);
            hitbox = new Rectangle((int)player.itemLocation.X + scale/2 , (int)player.itemLocation.Y + scale/2, scale, scale);
        }
    }
}
