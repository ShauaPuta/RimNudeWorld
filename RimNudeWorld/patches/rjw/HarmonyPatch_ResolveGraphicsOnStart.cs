using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rjw;

namespace RimNudeWorld {

    [HarmonyPatch(typeof(JobDriver_SexBaseInitiator), "Start")]
    class HarmonyPatch_ResolveGraphicsOnStart {

        public static void Prefix(JobDriver_SexBaseInitiator __instance) {

            //call resolveallgraphics to make sure genitalia are drawn during sex

            __instance?.pawn?.Drawer?.renderer?.graphics?.ResolveAllGraphics();
            __instance?.Partner?.Drawer?.renderer?.graphics?.ResolveAllGraphics();

        }

    }

}
