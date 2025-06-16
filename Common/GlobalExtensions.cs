using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terrarune.Core.ModPlayers;

namespace Terrarune.Common
{
    public static class GlobalExtensions
    {
        public static TerraruneModPlayer Terrarune(this Player player) => player.GetModPlayer<TerraruneModPlayer>();
        public static HeadAnimationPlayer HeadAnimationPlayer(this Player player) => player.GetModPlayer<HeadAnimationPlayer>();
        public static void SetStyle(this Item item, UseStyle style, bool alt = false)
        {
            if (alt)
            {
                item.GetGlobalItem<UseStyleItemHandler>().altStyle = style;
                return;
            }
            item.GetGlobalItem<UseStyleItemHandler>().style = style;
        }

    }
}
