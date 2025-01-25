using UnityEngine;

public static class PerlinNoise {
    public static float GetPerlinValue(float x, float y, float scale, float offsetX, float offsetY) {
        return Mathf.PerlinNoise((x + offsetX) * scale, (y + offsetY) * scale);
    }
}