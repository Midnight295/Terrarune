using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terrarune.Common;

namespace Terrarune.Content.UI
{
    public class DeltaruneModResourceDisplay : ModResourceDisplaySet
    {   
        private PlayerStatsSnapshot preparedSnapshot;
        private Asset<Texture2D> You;
        private Asset<Texture2D> Box;
        private Asset<Texture2D> Bars;
        private Asset<Texture2D> hpText;
        private Asset<Texture2D> mpText;
        private Asset<Texture2D> ManaBar;
        private Asset<Texture2D> ManaFilling;

        public override void Load()
        {
            if (Main.dedServ)
                return;

            string filepath = "Terrarune/Assets/UI/";
            You = ModContent.Request<Texture2D>(filepath + "You");
            Box = ModContent.Request<Texture2D>(filepath + "Box");
            Bars = ModContent.Request<Texture2D>(filepath + "Bars");
            hpText = ModContent.Request<Texture2D>(filepath + "HP");
            mpText = ModContent.Request<Texture2D>(filepath + "ManaText");
            ManaBar = ModContent.Request<Texture2D>(filepath + "ManaBar");
            ManaFilling = ModContent.Request<Texture2D>(filepath + "ManaFilling");
        }

        public override void DrawLife(SpriteBatch spriteBatch)
        {
            float lifeRatio = Main.LocalPlayer.statLife / (float)Main.LocalPlayer.statLifeMax2;
            ResourceDrawSettings resourceDrawSettings = default;
            resourceDrawSettings.StatsSnapshot = preparedSnapshot;
            Main.EntitySpriteDraw(Box.Value, new Vector2(Main.screenWidth - 340, 15), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None);
            //Main.EntitySpriteDraw(You.Value, new Vector2(1350, 70), null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None);
            Main.EntitySpriteDraw(hpText.Value, new Vector2(Main.screenWidth - 265, 45), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None);

            int BarToUse = 1;
            if (Main.LocalPlayer.ConsumedLifeFruit >= 1)
                BarToUse = 2;
            if (Main.LocalPlayer.Terrarune().KrisKnife)
                BarToUse = 3;
            if (Main.LocalPlayer.Terrarune().SusieChalk)
                BarToUse = 4;
            if (Main.LocalPlayer.Terrarune().FluffyHat || Main.LocalPlayer.Terrarune().HornedHeadband)
                BarToUse = 5;
            if (Main.LocalPlayer.Terrarune().BerdlyGrade)
                BarToUse = 7;
            Rectangle BarRect = new(0, (Bars.Height() / 8) * 0, Bars.Width(), Bars.Height() / 8);
            Rectangle HealthRect = new(0, (Bars.Height() / 8) * BarToUse, (int)(Bars.Width() * lifeRatio), Bars.Height() / 8);

            Main.EntitySpriteDraw(Bars.Value, new Vector2(Main.screenWidth - 130, 55), new Rectangle?(BarRect), Color.White, 0, BarRect.Size() * 0.5f, 1, SpriteEffects.None);
            Main.EntitySpriteDraw(Bars.Value, new Vector2(Main.screenWidth - 130, 55), new Rectangle?(HealthRect), Color.White, 0, BarRect.Size() * 0.5f, 1, SpriteEffects.None);
            Utils.DrawBorderString(spriteBatch, Main.LocalPlayer.statLife + " / " + Main.LocalPlayer.statLifeMax2, new Vector2(Main.screenWidth - 175, 20), lifeRatio <= .25 ? Color.Yellow : Color.White, 1);

            
        }

        public override void DrawMana(SpriteBatch spriteBatch)
        {
            float lifeRatio = Main.LocalPlayer.statMana / (float)Main.LocalPlayer.statManaMax2;
            ResourceDrawSettings resourceDrawSettings = default;
            resourceDrawSettings.StatsSnapshot = preparedSnapshot;

            int BarToUse = (Main.LocalPlayer.Terrarune().KrisKnife || Main.LocalPlayer.Terrarune().SusieChalk || Main.LocalPlayer.Terrarune().FluffyHat || Main.LocalPlayer.Terrarune().HornedHeadband || Main.LocalPlayer.Terrarune().BerdlyGrade) ? 1 : 0;
            Rectangle TextRect = new(0, (mpText.Height() / 2) * BarToUse, mpText.Width(), mpText.Height() / 2);
            Main.EntitySpriteDraw(mpText.Value, new Vector2(Main.screenWidth - 20, 30), TextRect, Color.White, 0, TextRect.Size() / 2, 1, SpriteEffects.None);

            Rectangle ManaRect = new(0, (ManaFilling.Height() / 2) * BarToUse, ManaFilling.Width(), (int)((ManaFilling.Height() / 2) * lifeRatio));
            Main.EntitySpriteDraw(ManaBar.Value, new Vector2(Main.screenWidth - 20, 190), null, Color.White, 0, ManaBar.Size() * 0.5f, new Vector2(1, 1), SpriteEffects.None);
            Main.EntitySpriteDraw(ManaFilling.Value, new Vector2(Main.screenWidth - 20, 190), new Rectangle?(ManaRect), Color.White, 15.71f, ManaBar.Size() * 0.5f, new Vector2(1, 1), SpriteEffects.None);
            //It has to be rotated by a value of specifically 15.71 because otherwise the mana drains from bottom to top, instead of top to bottom.

            Utils.DrawBorderString(spriteBatch, Main.LocalPlayer.statMana + "\n" + " / " + "\n" +  Main.LocalPlayer.statManaMax2, new Vector2(Main.screenWidth - 35, 140), lifeRatio <= .25 ? Color.Yellow : Color.White, 1);
        }
    }
}
