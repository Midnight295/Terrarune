using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Terrarune.Common
{
    public class LerpHelper
    {
        public enum LerpEasing
        {
            Linear,
            InSine,
            OutSine,
            InOutSine,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuint,
            OutQuint,
            InOutQuint,
            InBack,
            OutBack,
            InOutBack,
            DownParabola,
            Bell
        }
        public static float GetLerpValue(float timer, float length, LerpEasing easing, float start, bool clamp)
        {
            float x = (timer - start) / length;
            //Main.NewText(x);
            float lerp = 0;
            //i do not know the significance of these numbers. they are used in back easing calculations.
            float overshoot = 1.70158f;
            float overshoot2 = overshoot + 1;
            float overshoot3 = overshoot * 1.525f;
            lerp = easing switch
            {
                LerpEasing.InSine => 1 - (float)Math.Cos((x * Math.PI) / 2),
                LerpEasing.OutSine => (float)Math.Sin((x * Math.PI) / 2),
                LerpEasing.InOutSine => -(float)(Math.Cos(Math.PI * x) - 1) / 2,

                LerpEasing.InCubic => x * x * x,
                LerpEasing.OutCubic => 1 - (float)Math.Pow(1 - x, 3),
                LerpEasing.InOutCubic => x < 0.5f ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2,

                LerpEasing.InQuint => x * x * x * x * x,
                LerpEasing.OutQuint => 1 - (float)Math.Pow(1 - x, 5),
                LerpEasing.InOutQuint => x < 0.5f ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2,

                LerpEasing.InBack => overshoot2 * x * x * x - overshoot * x * x,
                LerpEasing.OutBack => 1 + overshoot2 * (float)Math.Pow(x - 1, 3) + overshoot * (float)Math.Pow(x - 1, 2),
                LerpEasing.InOutBack => x < 0.5f
                    ? ((float)Math.Pow(2 * x, 2) * ((overshoot3 + 1) * 2 * x - overshoot3)) / 2
                    : ((float)Math.Pow(2 * x - 2, 2) * ((overshoot3 + 1) * (x * 2 - 2) + overshoot3) + 2) / 2,
                LerpEasing.DownParabola => -(float)Math.Pow(2 * x - 1, 2) + 1,
                LerpEasing.Bell => (float)Math.Sin(2 * Math.PI * x - 0.5f * Math.PI) / 2 + 0.5f,
                _ => x
            };
            if (clamp)
            {
                lerp = Math.Clamp(lerp, 0, 1);
            }
            return lerp;
        }
        public static float LerpFloat(float startValue, float endValue, float timer, float length, LerpEasing easingStyle, float startOffset = 0, bool clamp = true)
        {
            float lerp = GetLerpValue(timer, length, easingStyle, startOffset, clamp);
            return MathHelper.Lerp(startValue, endValue, lerp);
        }
        public static float LerpAngle(float startValue, float endValue, float timer, float length, LerpEasing easingStyle, float startOffset = 0, bool clamp = true)
        {
            float lerp = GetLerpValue(timer, length, easingStyle, startOffset, clamp);
            return Utils.AngleLerp(startValue, endValue, lerp);
        }
        public static Vector2 LerpVector2(Vector2 startValue, Vector2 endValue, float timer, float length, LerpEasing easingStyle, float startOffset = 0, bool clamp = true)
        {
            float lerp = GetLerpValue(timer, length, easingStyle, startOffset, clamp);
            return Vector2.Lerp(startValue, endValue, lerp);
        }
        public static Color LerpColor(Color startValue, Color endValue, float timer, float length, LerpEasing easingStyle, float startOffset = 0, bool clamp = true)
        {
            float lerp = GetLerpValue(timer, length, easingStyle, startOffset, clamp);
            return Color.Lerp(startValue, endValue, lerp);
        }
    }
}
