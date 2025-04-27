using Kopernicus.Components;
using Kopernicus.Configuration;
using Kopernicus.Constants;
using System;
using System.Collections.Generic;
using Kopernicus.Configuration.Parsing;
using UnityEngine;
using UnityEngine.Rendering;

namespace NiakoKerbalMods
{

	namespace NiakoKopernicus
	{
		/// <summary>
		/// Variant of <see cref="PQSMod_VertexHeightMap"/> that uses bicubic interpolation (Mitchell Netravali) instead
		/// of bilineal interpolation. 
		/// </summary>
		public class PQSMod_VertexMitchellNetravaliHeightMap : PQSMod_VertexHeightMap
		{
#region Parameters

			/// <summary>
			/// B value for the Mitchell-Netravali Filter
			/// <para>If one wants to change this in runtime (Outside of the Kopernicus parser), remember to
			/// call <see cref="PrecalculateConstants"/> again
			/// </summary>
			public double B = 1.0f;
			/// <summary> 
			/// C value for the Mitchell-Netravali Filter
			/// <para>If one wants to change this in runtime (Outside of the Kopernicus parser), remember to
			/// call <see cref="PrecalculateConstants"/> again
			/// </summary>
			public double C = 0.0f;

			/// <summary>
			/// Stores wherever the constants been precalculated already in <see cref="PrecalculateConstants"/>
			/// </summary>
			protected bool hasConstantsPrecalculated = false;

			/// <summary>
			/// Cache Y-coordinate samples, used in <see cref="RunMitchellNetravali"/>. See this function for more
			/// </summary>
			private double[] PY = new double[4];

			/// <summary>
			/// Cache X-coordinate samples, used in <see cref="RunMitchellNetravali"/>. See this function for more
			/// </summary>
			private double[] PX = new double[4];

#endregion

#region Constants

			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _n6BnC;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _n32BnC2;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _32BCn2;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _6BC;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _2B2C;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _2BCn3;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _n52Bn2C3;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _n2BnC;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _2BC;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _6B;
			/// <summary> Constant, see <see cref="PrecalculateConstants"/> </summary>
			private double _n3B1;

			/// <summary>
			/// Precalculates constants used in the Mitchell-Netravali algorithm, based on
			/// the values of <see cref="B"/> and <see cref="C"/>.
			/// <para>Called on PQS mod setup, and when any height is sampled (unless the constants have
			/// been calculated previously)</para>
			/// </summary>
			public void PrecalculateConstants() {
				_n6BnC = (-1/6.0) * B - C;
				_n32BnC2 = (-3/2.0) * B - C + 2;
				_32BCn2 = -_n32BnC2;
				_6BC = -_n6BnC;
				_2B2C = 0.5f * B + 2 * C;
				_2BCn3 = 2 * B + C - 3;
				_n52Bn2C3 = (-5/2.0) * B - 2 * C + 3;
				_n2BnC = -0.5f * B - C;
				_2BC = -_n2BnC;
				_6B = (1/6.0) * B;
				_n3B1 = (-1/3.0) * B + 1;

				hasConstantsPrecalculated = true;
			}

#endregion

#region Mitchell Netravali

			/// <summary>
			/// Run the Mitchell-Netravali algorithm for 4 samples using the
			/// precalculated constants of <see cref="PrecalculateConstants"/> 
			/// </summary>
			public double RunMitchellNetravali(double P0, double P1, double P2, double P3, double d) {
				double output = (_n6BnC*P0 + _n32BnC2*P1 + _32BCn2*P2 + _6BC*P3) * d*d*d
								+ (_2B2C*P0 + _2BCn3*P1 + _n52Bn2C3*P2 - C*P3) * d*d
								+ (_n2BnC*P0 + _2BC*P2) * d
								+ _6B*P0 + _n3B1*P1 + _6B*P2;

				return output;
			}

			/// <summary>
			/// Clamp an <see cref="int"/> between two values, but looping around if a limit is reached
			/// (similar to how angles work)
			/// </summary>
			protected int ClampLoop(int value, int min, int max) {
				int d = max - min;
				return value < min ? value + d : (value >= max ? value - d : value);
			}

			/// <summary>
			/// Clamp an <see cref="int"/> between two values
			/// </summary>
			protected int Clamp(int value, int min, int max) {
				return value < min ? min : (value >= max ? max - 1 : value);
			}

			/// <summary>
			/// Calculates a bicubic interpolated sample of the heightmap <see cref="PQSMod_VertexHeightMap.heightMap"/> 
			/// </summary>
			/// <param name="u">U coordinate of the UV to sample, as handled by KSP's PQS system</param>
			/// <param name="v">V coordinate of the UV to sample, as handled by KSP's PQS system</param>
			public double InterpolateHeights(double u, double v) {
				//If for some reason the constants have not been calculated
				if(!hasConstantsPrecalculated) {
					PrecalculateConstants();
				}

				//Calculate necesary variables
				int x0 = (int)Math.Floor(u * heightMap.Width);
				int y0 = (int)Math.Floor(v * heightMap.Height);
				double u0 = x0/(double)heightMap.Width;
				double v0 = y0/(double)heightMap.Height;

				double uD = (u - u0) * heightMap.Width;
				double vD = (v - v0) * heightMap.Height;

				//Calculate height (Interpolate)
				for(int j = -1; j < 3; j++) {
					Int32 y = Clamp(y0 + j, 0, heightMap.Height);
					for(int i = -1; i < 3; i++) {
						Int32 x = ClampLoop(x0 + i, 0, heightMap.Width);
						PX[i + 1] = heightMap.GetPixelFloat(x, y);
					}
					PY[j+1] = RunMitchellNetravali(PX[0], PX[1], PX[2], PX[3], uD);
				}

				double output = RunMitchellNetravali(PY[0], PY[1], PY[2], PY[3], vD);

				return output;
			}

#endregion

#region PQSMod

			public override void OnSetup() {
				base.OnSetup();
				PrecalculateConstants();
			}

			public override void OnVertexBuildHeight(PQS.VertexBuildData data) {
				data.vertHeight += heightMapOffset + heightMapDeformity * (double)InterpolateHeights(data.u, data.v);
			}

#endregion
		}
	}
}