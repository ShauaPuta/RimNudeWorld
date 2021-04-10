using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using rjw;
using UnityEngine;
using RimWorld;

namespace RimNudeWorld
{
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    class HarmonyPatch_SwapBodyGraphicForRJW
    {

        public static void Postfix(PawnGraphicSet __instance)
        {
            Pawn p = __instance.pawn;
            if(!Genital_Helper.has_penis_fertile(p) && !Genital_Helper.has_penis_infertile(p) && !Genital_Helper.has_multipenis(p))
            {
                return;
            }

            string originalPath = __instance.nakedGraphic.path;
            string modifiedPath = originalPath + "_Lewd";

            if (ContentFinder<Texture2D>.Get(modifiedPath + "_north", false) != null)
            {
                if (p?.jobs?.curDriver != null &&
                (p?.jobs?.curDriver is JobDriver_Sex ||
                (p.RaceHasSexNeed() && xxx.need_sex(p) >= xxx.SexNeed.Frustrated)))
                {
                    GraphicData originalGraphicData = p.ageTracker.CurKindLifeStage.bodyGraphicData;
                    __instance.nakedGraphic = GraphicDatabase.Get(originalGraphicData.graphicClass, modifiedPath, (originalGraphicData.shaderType == null ? ShaderTypeDefOf.Cutout.Shader : originalGraphicData.shaderType.Shader), originalGraphicData.drawSize, originalGraphicData.color, originalGraphicData.colorTwo, originalGraphicData, originalGraphicData.shaderParameters);
                }
            }

            

        }

    }
}
