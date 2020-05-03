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
using UnityEngine;

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

            if(originalPath.Contains("penis") && originalPath.Length >= 9) {

                string modifiedPath = originalPath.Insert(9, "Flaccid/") + "_flaccid";

                if (pawn.RaceHasSexNeed()) {

                    if (xxx.need_sex(pawn) > xxx.SexNeed.Horny && !(pawn.jobs.curDriver is JobDriver_Sex) && ContentFinder<Texture2D>.Get(modifiedPath, false) != null) {

                        Graphic newGraphic = GraphicDatabase.Get<Graphic_Multi>(modifiedPath, __result.Shader, __result.drawSize, __result.color, __result.colorTwo);
                        __result = newGraphic;

                        return;
                    }

                }


            }
            

        }

    }

}
