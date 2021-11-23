using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using AlienRace;
using rjw;

namespace RimNudeWorld
{
	/*
	[HarmonyPatch(typeof(AlienRace.HarmonyPatches), "DrawAddons")]
	[HarmonyPriority(399)]
	public static class HarmonyPatch_AlienRace
	{

		const float PawnGenitalAngleIfPregnant = 15f;

		public static bool Prefix(bool portrait, Vector3 vector, Vector3 headOffset, Pawn pawn, Quaternion quat, Rot4 rotation, bool invisible)
		{

			if (!pawn.IsVisiblyPregnant()) return true;
			if (!(pawn.def is ThingDef_AlienRace alienProps) || invisible) return false;

			List<AlienPartGenerator.BodyAddon> addons = alienProps.alienRace.generalSettings.alienPartGenerator.bodyAddons;
			AlienPartGenerator.AlienComp alienComp = pawn.GetComp<AlienPartGenerator.AlienComp>();

			for (int i = 0; i < addons.Count; i++)
			{
				AlienPartGenerator.BodyAddon ba = addons[index: i];
				if (!ba.CanDrawAddon(pawn: pawn)) continue;

				AlienPartGenerator.RotationOffset offset;
				offset = rotation == Rot4.South ?
									ba.offsets.south :
									rotation == Rot4.North ?
										ba.offsets.north :
										rotation == Rot4.East ?
										ba.offsets.east :
										ba.offsets.west;



				Vector2 bodyOffset = (portrait ? offset?.portraitBodyTypes ?? offset?.bodyTypes : offset?.bodyTypes)?.FirstOrDefault(predicate: to => to.bodyType == pawn.story.bodyType)
								   ?.offset ?? Vector2.zero;
				Vector2 crownOffset = (portrait ? offset?.portraitCrownTypes ?? offset?.crownTypes : offset?.crownTypes)?.FirstOrDefault(predicate: to => to.crownType == alienComp.crownType)
									?.offset ?? Vector2.zero;

				//Defaults for tails 
				//south 0.42f, -0.3f, -0.22f
				//north     0f,  0.3f, -0.55f
				//east -0.42f, -0.3f, -0.22f   

				float genitalRotation = -1*PawnGenitalAngleIfPregnant;

				float moffsetX = 0.42f;
				float moffsetZ = -0.22f;

				float layerOffset = offset?.layerOffset ?? ba.layerOffset;

				float moffsetY = ba.inFrontOfBody ? 0.3f + ba.layerOffset : -0.3f - ba.layerOffset;
				float num = ba.angle;

				if (rotation == Rot4.North)
				{
					genitalRotation = 0;
					moffsetX = 0f;
					if (ba.layerInvert)
						moffsetY = -moffsetY;

					moffsetZ = -0.55f;
					num = 0;
				}

				if(rotation == Rot4.South)
                {
					genitalRotation = 0;
                }

				moffsetX += bodyOffset.x + crownOffset.x;
				moffsetZ += bodyOffset.y + crownOffset.y;

				if (rotation == Rot4.East)
				{
					genitalRotation *= -1;
					moffsetX = -moffsetX;
					num = -num; //Angle
				}

				Vector3 offsetVector = new Vector3(x: moffsetX, y: moffsetY, z: moffsetZ);

				//Angle calculation to not pick the shortest, taken from Quaternion.Angle and modified

				//assume drawnInBed means headAddon
				Graphic addonGraphic = alienComp.addonGraphics[i];
				addonGraphic.drawSize = (portrait && ba.drawSizePortrait != Vector2.zero ? ba.drawSizePortrait : ba.drawSize) * (ba.scaleWithPawnDrawsize ? alienComp.customDrawSize : Vector2.one) * 1.5f;

				Quaternion addonRotation = quat;
				if (ba?.hediffGraphics != null && ba.hediffGraphics.Count != 0 && ba.hediffGraphics[0]?.path != null && (ba.hediffGraphics[0].path.Contains("Penis") || ba.hediffGraphics[0].path.Contains("penis")))
				{
					addonRotation = Quaternion.AngleAxis(angle: genitalRotation, axis: Vector3.up);
				}

				GenDraw.DrawMeshNowOrLater(mesh: addonGraphic.MeshAt(rot: rotation), loc: vector + offsetVector.RotatedBy(angle: Mathf.Acos(f: Quaternion.Dot(a: Quaternion.identity, b: quat)) * 2f * 57.29578f),
				quat: Quaternion.AngleAxis(angle: num, axis: Vector3.up) * addonRotation, mat: addonGraphic.MatAt(rot: rotation), drawNow: portrait);


			}

			return false;
		}
	}
	*/
}
