using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using AlienRace;
using rjw;

namespace RimNudeWorld
{

    [StaticConstructorOnStartup]
    public class HarmonyPatchAll {

        public HarmonyPatchAll() {

            Harmony har = new Harmony("RimNudeWorld");
            har.PatchAll();

        }

    }


    [HarmonyPatch(typeof(AlienPartGenerator.BodyAddon), "GetPath")]
    public class GenitalPatch {

        public static void Postfix(Pawn pawn, ref Graphic __result) {

            string originalPath = __result.path;


            if(pawn.RaceHasSexNeed()) {

                if(originalPath.Contains("penis") && (xxx.need_sex(pawn) <= xxx.SexNeed.Horny || (pawn.jobs.curDriver is JobDriver_Sex))) {

                    Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_flaccid", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                    if(newGraphic != null) {
                        __result = newGraphic;
                        return;
                    }

                }

            }

        }

    }

}
