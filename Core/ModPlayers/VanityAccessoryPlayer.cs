using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terrarune.Common;
using Terrarune.Content.Items;

namespace Terrarune.Core.ModPlayers
{
    public class VanityAccessoryPlayer : ModPlayer
    {
        public override void Load()
        {
            On_Player.ResetVisibleAccessories += ResetVanity;
            On_Player.UpdateVisibleAccessory += SetVanityItem;
        }

        internal VanityAccessory currentVanity = null;
        internal VanityAccessory previousVanity = null;
        internal int previousHighest = -1;
        internal int currentHighest = -1;

        private void ResetVanity(On_Player.orig_ResetVisibleAccessories orig, Player self)
        {
            orig(self);

            if (!self.dead)
                Reset(self);
            else
            {
                VanityAccessory trans = self.Vanity().currentVanity = self.Vanity().previousVanity;
                if (trans is not null)
                    EquipVanity(trans, self, Mod);
            }
        }

        private static void Reset(Player p)
        {
            p.Vanity().previousHighest = p.Vanity().currentHighest;
            p.Vanity().currentHighest = -1;

            if (p.Vanity().currentVanity != null)
            {
                p.Vanity().previousVanity = p.Vanity().currentVanity;
                p.Vanity().currentVanity = null;
            }
        }

        private void SetVanityItem(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
        {
            orig(self, itemSlot, item, modded);

            if (self.Vanity().currentVanity == null)
            {
                if (item.ModItem is VanityAccessory Vanity && currentVanity == null)
                    self.Vanity().currentVanity = Vanity;
            }

            if (self.Vanity().currentHighest < itemSlot)
                self.Vanity().currentHighest = itemSlot;

            if (self.Vanity().currentVanity != null && itemSlot == self.Vanity().previousHighest) // last item slot, can set equip slots to our Vanity if one is equipped
            {
                VanityAccessory trans = self.Vanity().currentVanity;
                EquipVanity(trans, self, Mod);
            }
        }

        public override void UpdateAutopause()
        {
            if (currentVanity != null)
            {
                if (!currentVanity.CustomSetEquipType(Player, EquipType.Head, Mod, currentVanity.Name))
                {
                    var (Type, AssetName, EquipName) = currentVanity.EquipSlots.First(v => v.Type == EquipType.Head);
                    string name = (EquipName == null && AssetName == null) ? null : EquipName ?? currentVanity.Name;
                    SetEquipSlot(Player, EquipType.Head, Mod, name);
                }
            }

            Reset(Player);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (currentVanity != null)
            {
                currentVanity.ModifyDrawInfo(ref drawInfo);

                if (currentVanity.EquipSlots.Any(s => s.Type == EquipType.Head))
                {
                    drawInfo.headGlowMask = -1;
                    drawInfo.helmetOffset = Vector2.Zero;
                }

                if (currentVanity.EquipSlots.Any(s => s.Type == EquipType.Body))
                {
                    drawInfo.bodyGlowMask = -1;
                }

                if (currentVanity.EquipSlots.Any(s => s.Type == EquipType.Legs))
                {
                    drawInfo.legsGlowMask = -1;
                    drawInfo.legsOffset = Vector2.Zero;
                }
            }
        }

        public override void FrameEffects()
        {
            if (currentVanity != null)
            {
                currentVanity.TransformFrameEffects(Player);

                if (currentVanity.ShouldHideAccessories)
                    HideAccessories(Player);
            }
        }

        public override void PostUpdate()
        {
            currentVanity?.TransformPostUpdate(Player);
        }

        public static void HideAccessories(Player player, bool hideHeadAccs = true, bool hideBodyAccs = true, bool hideLegAccs = true, bool hideShield = true)
        {
            if (hideHeadAccs)
                player.face = -1;

            if (hideBodyAccs)
            {
                player.handon = -1;
                player.handoff = -1;

                player.back = -1;
                player.front = -1;
                player.neck = -1;
            }

            if (hideLegAccs)
            {
                player.shoe = -1;
                player.waist = -1;
            }

            if (hideShield)
                player.shield = -1;
        }

        public static void SetEquipSlot(Player player, EquipType type, Mod mod, string name)
        {
            switch (type)
            {
                case EquipType.Head:
                    player.head = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Body:
                    player.body = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Legs:
                    player.legs = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.HandsOn:
                    player.handon = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.HandsOff:
                    player.handoff = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Back:
                    player.back = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Front:
                    player.front = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Shoes:
                    player.shoe = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Waist:
                    player.waist = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Wings:
                    player.wings = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Shield:
                    player.shield = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Neck:
                    player.neck = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Face:
                    player.face = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Balloon:
                    player.balloon = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
                case EquipType.Beard:
                    player.beard = name == null ? -1 : EquipLoader.GetEquipSlot(mod, name, type);
                    break;
            }
        }

        public static int GetEquipSlot(Player player, EquipType type) => type switch
        {
            EquipType.Head => player.head,
            EquipType.Body => player.body,
            EquipType.Legs => player.legs,
            EquipType.HandsOn => player.handon,
            EquipType.HandsOff => player.handoff,
            EquipType.Back => player.back,
            EquipType.Front => player.front,
            EquipType.Shoes => player.shoe,
            EquipType.Waist => player.waist,
            EquipType.Wings => player.wings,
            EquipType.Shield => player.shield,
            EquipType.Neck => player.neck,
            EquipType.Face => player.face,
            EquipType.Balloon => player.balloon,
            EquipType.Beard => player.beard,
            _ => -1,
        };

        public static void EquipVanity(VanityAccessory trans, Player self, Mod mod)
        {
            bool[] list = new bool[15];
            foreach (var (Type, AssetName, EquipName) in trans.EquipSlots)
            {
                string name = AssetName == null ? null : EquipName ?? trans.Name;
                if (!trans.CustomSetEquipType(self, Type, mod, name) && !list[(int)Type])
                {
                    SetEquipSlot(self, Type, mod, name);
                    list[(int)Type] = true;
                }
            }
            if (trans.ShouldHideAccessories)
                HideAccessories(self);
        }

        public static EquipTexture GetEquipTexture(Player p, EquipType type) => EquipLoader.GetEquipTexture(type, GetEquipSlot(p, type));
    }
}
