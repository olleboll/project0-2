using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelperFunctions
{
	private static string[] jumpableTiles = {"water", "abyss"};
	public static bool isNonGroundTile(string tileName){
		for (int i = 0; i < jumpableTiles.Length; i++) {
			if (tileName.ToLower().Contains(jumpableTiles[i])) {
				return true;
			}
		}
		return false;
	}
	public static Vector3 getLerpedPosition(Vector3 start, Vector3 target, float interpolation){
		float minX = start.x;
		float maxX = target.x;
		float newX = Mathf.Lerp(minX, maxX, interpolation);

		float minY = start.y;
		float maxY = target.y;
		float newY = Mathf.Lerp(minY, maxY, interpolation);
		return new Vector3(newX, newY, 0);
	}
}
