using System.Reflection;
using NLog;
using Torch.Managers.PatchManager;
using VRage.Network;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.EntityComponents.Blocks;

namespace SpriteUnlimiter
{
    [PatchShim]
    public static class SpriteUnlimiterPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static float DistanceSetting { get; set; }

        private static readonly ConstructorInfo callSiteConstructor = typeof(CallSite).GetConstructor
        (
            new[] 
            {
                typeof(MySynchronizedTypeInfo),
                typeof(uint),
                typeof(MethodInfo),
                typeof(CallSiteFlags),
                typeof(ValidationType),
                typeof(float)
            }
        );
        private static readonly MethodInfo constructorPatch = typeof(SpriteUnlimiterPatch)
            .GetMethod(nameof(ConstructPatch), BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly MethodInfo textPanelUpdateSprites = typeof(MyMultiTextPanelComponent)
            .GetMethod("OnUpdateSpriteCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo lcdSurfaceUpdateSprites = typeof(MyLcdSurfaceComponent)
            .GetMethod("OnUpdateSpriteCollection", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(callSiteConstructor).Prefixes.Add(constructorPatch);
        }

        private static void ConstructPatch(MethodInfo info, CallSiteFlags flags, ref float distanceRadius)
        {
            if ((flags & CallSiteFlags.DistanceRadius) != 0)
            {
                if (distanceRadius == 32 && (info == textPanelUpdateSprites || info == lcdSurfaceUpdateSprites))
                {
                    distanceRadius = DistanceSetting;
                    Log.Info($"Changed sync distance to {distanceRadius} on {info.DeclaringType.Name}.{info.Name}.");
                }
            }
        }
    }
}