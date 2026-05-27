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
using Terraria.ModLoader;
using Terrarune.Common;

namespace Terrarune.Core.ModPlayers.DrawLayers
{
    public class NoelleAntlersDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0)
                return false;

           if (!player.Terrarune().SilverWatch)
                return false;

            return true;
        }

        public override bool IsHeadLayer => true;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            // Attempt at recreating head drawcode, its a bit silly.
            Vector2 Position = drawInfo.helmetOffset +
                new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 +
                drawInfo.drawPlayer.width / 2),
                (int)(drawInfo.Position.Y - Main.screenPosition.Y +
                drawInfo.drawPlayer.height -
                drawInfo.drawPlayer.bodyFrame.Height + 4f)) +
                drawInfo.drawPlayer.headPosition +
                drawInfo.headVect +
                new Vector2(0, -14) * drawPlayer.gravDir;
           
            Texture2D HornsTexture = ModContent.Request<Texture2D>("Terrarune/Assets/Items/Vanity/SilverWatch/SilverWatchHeadExtension", AssetRequestMode.ImmediateLoad).Value;

            DrawData item = new(HornsTexture, Position, drawPlayer.bodyFrame, drawInfo.colorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect);
            item.shader = drawInfo.cBody;
            drawInfo.DrawDataCache.Add(item);
        }
    }
}
