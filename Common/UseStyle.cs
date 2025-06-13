using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.GameContent;

namespace Terrarune.Common
{
    public abstract class UseStyle
    {
        public float PercentTrickIntoSwinging;
        public float altTimeMultiplyer = 1f;
        /// <summary>
        /// override this to change what actually physically happens during the use style
        /// </summary>
        /// <param name="player"></param>
        /// <param name="heldItemFrame"></param>
        public abstract void Usage(Player player, Rectangle heldItemFrame);
        /// <summary>
        /// override this to draw the use style. will not draw anything if not done.
        /// </summary>
        /// <param name="drawInfo"></param>
        public abstract void Draw(ref PlayerDrawSet drawInfo);
        /// <summary>
        /// override this to change the melee hitbox of the item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="hitbox"></param>
        /// <param name="noHitbox"></param>
        public abstract void Hitbox(Player player, ref Rectangle hitbox, ref bool noHitbox);

        public void DoTheDraw(ref PlayerDrawSet drawInfo, Asset<Texture2D> texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            drawInfo.DrawDataCache.Add(new DrawData(texture.Value, position, sourceRect, color, rotation, origin, scale, effects));
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void SetPlayerDirection(Player player)
        {
            player.direction = -1;
            if (Main.MouseWorld.X >= player.Center.X)
            {
                player.direction = 1;

            }
        }
       
        public Color ColorAtHand(PlayerDrawSet drawinfo)  => Lighting.GetColor(drawinfo.drawPlayer.itemLocation.ToTileCoordinates()); 
    }
    public class UseStyleItemHandler : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public UseStyle style;
        public UseStyle altStyle;
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (altStyle != null)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            int animation = player.itemAnimationMax - player.itemAnimation;

            if (altStyle != null && player.altFunctionUse == 2 && altStyle.altTimeMultiplyer != 1)
            {
                float mult = altStyle.altTimeMultiplyer;
                if (animation == 0)
                {
                    player.itemAnimationMax = (int)(player.itemAnimationMax * mult);
                    player.itemAnimation = player.itemAnimationMax;
                }
                if (player.itemAnimation == 1)
                {
                    player.itemAnimationMax = (int)(player.itemAnimationMax / mult);
                }
            }
            if (style != null && player.altFunctionUse != 2)
            {
                style.Usage(player, heldItemFrame);
            }
            if (altStyle != null && player.altFunctionUse == 2)
            {
                altStyle.Usage(player, heldItemFrame);
            }
            base.UseStyle(item, player, heldItemFrame);
        }
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if(style != null && player.altFunctionUse != 2)
            {
                style.Hitbox(player, ref hitbox, ref noHitbox);
            }
            if (altStyle != null && player.altFunctionUse == 2)
            {
                altStyle.Hitbox(player, ref hitbox, ref noHitbox);
            }
        }
    }
    public class UseStyleDrawer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            if (drawInfo.heldItem.type == 0)
            {
                return false;
            }
            UseStyle style = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().style;
            UseStyle altStyle = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().altStyle;
            if (((style != null && drawInfo.drawPlayer.altFunctionUse != 2) || (altStyle != null && drawInfo.drawPlayer.altFunctionUse == 2)) && drawInfo.drawPlayer.ItemAnimationActive)
            {
                return true;
            }
            
            return false;
        }
        public override Position GetDefaultPosition() => new Between(Terraria.DataStructures.PlayerDrawLayers.SolarShield, Terraria.DataStructures.PlayerDrawLayers.ArmOverItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            UseStyle style = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().style;
            UseStyle altStyle = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().altStyle;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (style != null && drawInfo.drawPlayer.altFunctionUse != 2)
            {
                style.Draw(ref drawInfo);
            }
            if (altStyle != null && drawInfo.drawPlayer.altFunctionUse == 2)
            {
                altStyle.Draw(ref drawInfo);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
    public class UseStyleHider : ModPlayer
    {
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (drawInfo.heldItem.type == 0) return;
            UseStyle style = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().style;
            UseStyle altStyle = drawInfo.heldItem.GetGlobalItem<UseStyleItemHandler>().altStyle;
            if (((style != null && drawInfo.drawPlayer.altFunctionUse != 2) || (altStyle != null && drawInfo.drawPlayer.altFunctionUse == 2)) && drawInfo.drawPlayer.ItemAnimationActive)
            {
                Terraria.DataStructures.PlayerDrawLayers.HeldItem.Hide();
            }

        }
    }
    //detour for tricking vanilla into doing the player's use style 1 framing during a swing
    public class UseStyleOneAnimator : ModSystem
    {
        public override void Load()
        {
            On_Player.PlayerFrame += On_Player_PlayerFrame;
        }

        private void On_Player_PlayerFrame(On_Player.orig_PlayerFrame orig, Player self)
        {
            if (self.HeldItem.type != 0 &&
                ((self.HeldItem.GetGlobalItem<UseStyleItemHandler>().style != null && self.HeldItem.GetGlobalItem<UseStyleItemHandler>().style.PercentTrickIntoSwinging > 0 && self.altFunctionUse != 2) ||
                (self.HeldItem.GetGlobalItem<UseStyleItemHandler>().altStyle != null && self.HeldItem.GetGlobalItem<UseStyleItemHandler>().altStyle.PercentTrickIntoSwinging > 0 && self.altFunctionUse == 2)))
            {
                
                int animation = self.itemAnimationMax - self.itemAnimation;
                int originalMax = self.itemAnimationMax;
                int originalAnim = self.itemAnimation;

                if (self.altFunctionUse != 2)
                {
                    float percent = self.HeldItem.GetGlobalItem<UseStyleItemHandler>().style.PercentTrickIntoSwinging;
                    self.itemAnimation -= (int)(self.itemAnimationMax * (1 - percent));
                    self.itemAnimationMax = (int)(self.itemAnimationMax * percent);
                }
                if (self.altFunctionUse == 2)
                {
                    float percent = self.HeldItem.GetGlobalItem<UseStyleItemHandler>().altStyle.PercentTrickIntoSwinging;
                    self.itemAnimation -= (int)(self.itemAnimationMax * (1 - percent));
                    self.itemAnimationMax = (int)(self.itemAnimationMax * percent);
                }
                self.HeldItem.useStyle = ItemUseStyleID.Swing;
                
                orig(self);
                self.HeldItem.useStyle = -1;
                self.itemAnimationMax = originalMax;
                self.itemAnimation = originalAnim;
            }
            else
                orig(self);
        }
    }
}
