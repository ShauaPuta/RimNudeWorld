using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using AlienRace;
using rjw;
using System.Reflection;

namespace RimNudeWorld
{

    [StaticConstructorOnStartup]
    static class HarmonyPatchAll {

        static HarmonyPatchAll() {

            Harmony har = new Harmony("RimNudeWorld");
            har.PatchAll(Assembly.GetExecutingAssembly());

        }

    }


    [HarmonyPatch(typeof(AlienPartGenerator.BodyAddon), "GetPath")]
    public class GenitalPatch {

        public static void Postfix(Pawn pawn, ref Graphic __result) {

            string originalPath = __result.path;


            if(pawn.RaceHasSexNeed()) {

                if(originalPath.Contains("penis") && xxx.need_sex(pawn) > xxx.SexNeed.Horny && !(pawn.jobs.curDriver is JobDriver_Sex)) {

                    Log.Message("Attempting to replace " + originalPath + " with " + originalPath + "_flaccid");

                    Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(originalPath + "_flaccid", __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                    if(newGraphic != null) {
                        __result = newGraphic;
                        Log.Message("Success");
                        return;
                    }
                    else {
                        Log.Message("graphic not found");
                    }

                }

            }

        }

    }

}
