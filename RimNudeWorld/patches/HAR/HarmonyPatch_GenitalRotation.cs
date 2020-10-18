using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AlienRace;
using HarmonyLib;
using RimWorld;
using Verse;
using rjw;
using UnityEngine;

namespace RimNudeWorld {
	/*
	[HarmonyPatch(typeof(AlienRace.HarmonyPatches), "DrawAddons")]
	class HarmonyPatch_RotateGenitals {
		

		[HarmonyAfter(new string[] { "erdelf.HumanoidAlienRaces" })]
		[HarmonyReversePatch(HarmonyReversePatchType.Snapshot)]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {

			MethodInfo drawMeshNowOrLater = AccessTools.Method(typeof(GenDraw), "DrawMeshNowOrLater");


			List<CodeInstruction> codes = instructions.ToList();
			for (int i = 0; i < codes.Count(); i++) {

				//Instead of calling drawmeshnoworlater, add pawn to the stack and call my special static method
				if (codes[i].OperandIs(drawMeshNowOrLater)) {

					yield return new CodeInstruction(OpCodes.Ldarg_2); //load pawn onto the stack

					yield return new CodeInstruction(OpCodes.Ldloc_S, 4); //load current bodyaddon onto the stack

					yield return new CodeInstruction(OpCodes.Ldarg_3); // load original Quaternion onto the stack

					yield return new CodeInstruction(OpCodes.Ldarg_S, 4); //load rotation onto the stack

					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(HarmonyPatch_RotateGenitals), nameof(HarmonyPatch_RotateGenitals.RotatePregnantGenitals)));

				}

				else {
					yield return codes[i];
				}
			}
		}

		public static void RotatePregnantGenitals(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow, Pawn pawn, AlienPartGenerator.BodyAddon bodyAddon, Quaternion quat2, Rot4 rot) {

			if (bodyAddon?.path != null && bodyAddon.path.Length >= 5 && bodyAddon.path.Contains("Penis") && pawn.IsVisiblyPregnant()) {

				if (rot == Rot4.East) {
					quat = Quaternion.AngleAxis(-1f * bodyAddon.angle - 30, Vector3.up) * quat2;
				}
				else if (rot == Rot4.West) {
					quat = Quaternion.AngleAxis(bodyAddon.angle + 30, Vector3.up) * quat2;
				}

			}

			GenDraw.DrawMeshNowOrLater(mesh, loc, quat, mat, drawNow);

		}
		
	}

	*/
}
