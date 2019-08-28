namespace src.misc {
	public static class MathHelper {
		public static float mapValue(float value, float oA, float oB, float dA, float dB) {
			return (value - oA) / (oB - oA) * (dB - dA) + dA;
		}
	}
}