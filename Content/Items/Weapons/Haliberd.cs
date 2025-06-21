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
using Terrarune.Core.ModPlayers;

namespace Terrarune.Content.Items.Weapons
{
    public class Haliberd : ModItem
    {
        public override string Texture => "Terrarune/Assets/Weapons/Haliberd";
        public override void SetDefaults()
        {
            Item.width = Item.height = 20;
            Item.damage = 30;
            Item.crit = 6;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 75;
            Item.useStyle = -1;
            Item.UseSound = new Terraria.Audio.SoundStyle("Terrarune/Assets/Sounds/SpearTelegraph");
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 5);
            HaliberdUseStyle style = new();
            Item.SetStyle(style);
        }
        
    }
    public class HaliberdUseStyle : UseStyle
    {
        public int SineLength = 30;
        public override void Usage(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center;
            TerraruneModPlayer mp = player.Terrarune();
            //player.itemRotation = MathHelper.ToRadians(-45);
            int animation = player.itemAnimationMax - player.itemAnimation;
            //how long each phase of the animation lasts in %. wack numbers because use time was originally 65 but then i added a phase
            float spinTime = 0.52f;
            float stabTime = 0.087f;
            float stabOutTime = 0.26f;
            float retractTime = 0.133f;
            //player face use direction on initial use, set target angle to where mouse points
            if (animation == 0 && Main.myPlayer == player.whoAmI)
            {
                SetPlayerDirection(player);
                if (Main.myPlayer == player.whoAmI)
                    mp.TargetRotation = player.AngleTo(Main.MouseWorld);
            }
            
            //on reuses, advance to stab part and reduce the length of the projectile, and start the draw animation partway through so you dont see it as a halberd for a frame
            if (animation == 0 && player.Terrarune().ItemUsedPreviousFrame == player.HeldItem.type)
            {
                player.itemAnimation = (int)(player.itemAnimationMax * (1 - spinTime));
                animation = player.itemAnimationMax - player.itemAnimation;
                if (Main.myPlayer == player.whoAmI)
                {
                    mp.TargetRotation = player.AngleTo(Main.MouseWorld);
                    player.itemRotation = mp.TargetRotation + MathHelper.ToRadians(45);
                    SineLength -= 2;
                    if (SineLength < 2) SineLength = 2;
                    mp.UseStyleVar = 6;
                }
            }
            //reset start frame and projectile length on normal uses
            else if (animation == 0 && Main.myPlayer == player.whoAmI)
            {
                SineLength = 30;
                mp.UseStyleVar = 0;
            }
            
            //play ding right before stab
            if (animation == (int)(player.itemAnimationMax * (spinTime - 0.1f)))
            {
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Ding") with { Pitch = -0.4f, Volume = 0.5f}, player.Center);
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/Ding") with { Pitch = -0.2f, Volume = 0.5f }, player.Center);
            }
            //set target angle at start of stab
            if (animation == (int)(player.itemAnimationMax * spinTime) && Main.myPlayer == player.whoAmI)
            {
                for (int i = 0; i < player.meleeNPCHitCooldown.Length; i++)
                {
                    player.meleeNPCHitCooldown[i] = 0;
                }
                SetPlayerDirection(player);
                mp.TargetRotation = player.AngleTo(Main.MouseWorld);
                
                SoundEngine.PlaySound(new SoundStyle("Terrarune/Assets/Sounds/CriticalSwing") with { Volume = 0.8f}, player.Center);
               
            }
            //spin the halberd around and move it in closer to prepare for the stab
            if (animation < player.itemAnimationMax * spinTime)
            {
                //SetPlayerDirection(player);
                player.itemRotation = LerpHelper.LerpAngle(mp.TargetRotation - MathHelper.ToRadians(player.direction == 1 ? 75 : -165), mp.TargetRotation + MathHelper.ToRadians(player.direction == 1 ? 765 : -675), animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.OutSine);
                player.itemLocation = player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2()  * LerpHelper.LerpFloat(30, -10, animation, player.itemAnimationMax * spinTime, LerpHelper.LerpEasing.InOutQuint);
            }
            //stab out
            else if (animation < player.itemAnimationMax * (spinTime + stabTime))
            {
                player.itemRotation = mp.TargetRotation + MathHelper.ToRadians(45);
                player.itemLocation = LerpHelper.LerpVector2(player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2() * -10, player.Center + mp.TargetRotation.ToRotationVector2() * 40, animation, player.itemAnimationMax * stabTime, LerpHelper.LerpEasing.InOutSine, player.itemAnimationMax * spinTime);
            }
            //stay out
            else if (animation < player.itemAnimationMax * (spinTime + stabTime + stabOutTime))
            {
                player.itemLocation = player.Center + mp.TargetRotation.ToRotationVector2() * 40;
            }
            //retract in
            else
            {
                player.itemLocation = LerpHelper.LerpVector2(player.Center + mp.TargetRotation.ToRotationVector2() * 40, player.Center + (player.itemRotation - MathHelper.ToRadians(45)).ToRotationVector2() * -10, animation, player.itemAnimationMax * retractTime, LerpHelper.LerpEasing.InOutSine, player.itemAnimationMax * (spinTime + stabOutTime + stabTime));
            }
            //spawn projectile at end of stab
            if (animation == (int)(player.itemAnimationMax * (spinTime + stabTime)) && Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center + mp.TargetRotation.ToRotationVector2() * 80, player.AngleTo(Main.MouseWorld).ToRotationVector2() * 20, ModContent.ProjectileType<BerdSine>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem), ai2: SineLength);
            }
            //arm stretch (none, then stretches out towards full during stab)
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
            TerraruneModPlayer mp = player.Terrarune();
            int animation = player.itemAnimationMax - player.itemAnimation;
            float spinTime = 0.52f;
            float stabTime = 0.087f;
            float stabOutTime = 0.26f;
            float retractTime = 0.133f;
            
            SpriteEffects effect = SpriteEffects.None;
            if (player.direction == -1) effect = SpriteEffects.FlipHorizontally;
            //has 13 frames
            Asset<Texture2D> t = ModContent.Request<Texture2D>("Terrarune/Assets/Weapons/Haliberd-Sheet");
            Rectangle frame = new Rectangle(0, 0, t.Width(), t.Height() / 13);
            //set a start frame because itll be different depending on reuse/first use
            if (animation == player.itemAnimationMax * spinTime && player.Terrarune().ItemUsedPreviousFrame == player.HeldItem.type)
            {
                frame.Y = t.Height() / 13 * (int)mp.UseStyleVar;
            }
            //setting frame to animate towards end of sheet during stab
            if (animation > player.itemAnimationMax * spinTime)
            {
                int frameY = 0;
                frameY = (int)LerpHelper.LerpFloat(mp.UseStyleVar, 12, animation, player.itemAnimationMax * stabTime, LerpHelper.LerpEasing.Linear, player.itemAnimationMax * spinTime);
                frame.Y = t.Height() / 13 * frameY;
            }
            DoTheDraw(ref drawInfo, t, player.itemLocation - Main.screenPosition, frame, ColorAtHand(drawInfo), player.itemRotation + (player.direction == 1 ? 0 : MathF.PI/2), new Vector2(t.Width(),t.Height()/13)/2, player.GetAdjustedItemScale(player.HeldItem), effect);
        }
        public override void Hitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            int scale = (int)(player.GetAdjustedItemScale(player.HeldItem) * player.HeldItem.width)*2;
            Vector2 location = player.Center + player.Center.AngleTo(player.itemLocation).ToRotationVector2() * player.Center.Distance(player.itemLocation)*2.5f;
            hitbox = new Rectangle((int)location.X - scale/2, (int)location.Y - scale/2, scale, scale);
        }
    }
}
