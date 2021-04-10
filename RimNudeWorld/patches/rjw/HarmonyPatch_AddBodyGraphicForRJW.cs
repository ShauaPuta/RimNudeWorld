using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rjw;
using Verse;
using UnityEngine;
using RimWorld;

namespace RimNudeWorld
{
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal",
        new Type[] { 
            typeof(Vector3),
            typeof(float),
            typeof(bool),
            typeof(Rot4),
            typeof(Rot4),
            typeof(RotDrawMode),
            typeof(bool),
            typeof(bool),
            typeof(bool)
        })]
    class HarmonyPatch_AddBodyGraphicForRJW
    {
        public static void Postfix(PawnRenderer __instance, Pawn ___pawn, Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, bool portrait, bool headStump, bool invisible)
        {
            Pawn p = ___pawn;
            if (!Genital_Helper.has_penis_fertile(p) && !Genital_Helper.has_penis_infertile(p) && !Genital_Helper.has_multipenis(p))
            {
                return;
            }

            string originalPath = __instance.graphics.nakedGraphic.path;
            string modifiedPath = originalPath + "_Lewd";

            if (ContentFinder<Texture2D>.Get(modifiedPath + "_north", false) != null)
            {
                if (p?.jobs?.curDriver != null && p.jobs.curDriver is JobDriver_Sex)
                {
                    GraphicData originalGraphicData = p.ageTracker.CurKindLifeStage.bodyGraphicData;
                    Graphic lewdGraphic = CachedGraphics.LewdGraphics.TryGetValue(modifiedPath);

                    if(lewdGraphic != null)
                    {
                        Mesh lewdMesh = lewdGraphic.MeshAt(bodyFacing);
                        rootLoc.y = (AltitudeLayer.LayingPawn - 1).AltitudeFor();
                        GenDraw.DrawMeshNowOrLater(lewdMesh, rootLoc, Quaternion.AngleAxis(angle, Vector3.up), lewdGraphic.MatAt(bodyFacing), portrait);
                    }
                }
            }
        }
    }

}
