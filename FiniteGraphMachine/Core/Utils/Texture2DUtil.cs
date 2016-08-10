using UnityEngine;

namespace DTFiniteGraphMachine {
	public static class Texture2DUtil {
    public static Texture2D CreateTextureWithColor(Color color, int width = 1, int height = 1) {
      Color[] pixels = new Color[width * height];

      for (int i = 0; i < pixels.Length; i++) {
        pixels[i] = color;
      }

      Texture2D tex = new Texture2D(width, height);
      tex.SetPixels(pixels);
      tex.Apply();

      return tex;
    }
	}
}