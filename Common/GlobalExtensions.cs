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

    }
}
