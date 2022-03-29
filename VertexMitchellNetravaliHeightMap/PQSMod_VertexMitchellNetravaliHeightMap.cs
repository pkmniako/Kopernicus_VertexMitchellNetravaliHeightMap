using Kopernicus.Components;
using Kopernicus.Configuration;
using Kopernicus.Constants;
using System;
using System.Collections.Generic;
using Kopernicus.Configuration.Parsing;
using UnityEngine;
using UnityEngine.Rendering;

namespace NiakoKerbalMods {

	namespace NiakoKopernicus {
		/// <summary>
		/// PQSMod that adds Bilineal Filtering for the stock VertexHeightMap PQSMod
		/// </summary>
		public class PQSMod_VertexMitchellNetravaliHeightMap : PQSMod_VertexHeightMap {
			private double GetGreyscaleValueFromMap(int x, int y, int width, int height) {
				return (double)heightMap.GetPixelFloat(((double)x)/width, ((double)y)/height);
			}

			/// <summary> B value for the Mitchell-Netravali Filter </summary>
			public float B = 0.3333333333f;
			/// <summary> C value for the Mitchell-Netravali Filter </summary>
			public float C = 0.3333333333f;

			/// <summary> Have the constants been precalculated already? </summary>
			protected bool hasConstantsPrecalculated = false;

			/*
				Constants
			*/
			private double _n6BnC;
			private double _n32BnC2;
			private double _32BCn2;
			private double _6BC;
			private double _2B2C;
			private double _2BCn3;
			private double _n52Bn2C3;
			private double _n2BnC;
			private double _2BC;
			private double _6B;
			private double _n3B1;
			private double _maxD;

			/// <summary> Precalculates constants that are used at OnVertexBuildHeight </summary>
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

				double iWidth = 1.0/heightMap.Width;
				double iHeight = 1.0/heightMap.Height;
				_maxD = Math.Sqrt(iWidth * iWidth + iHeight * iHeight);

				Debug.Log("[VertexMitchellNetravaliHeightMap] New Constant values for B = " + B + " and C = " + C);
			}

			public double RunMitchellNetravali(double P0, double P1, double P2, double P3, double d) {
				double output = (_n6BnC*P0 + _n32BnC2*P1 + _32BCn2*P2 + _6BC*P3) * d*d*d
								+ (_2B2C*P0 + _2BCn3*P1 + _n52Bn2C3*P2 - C*P3) * d*d
								+ (_n2BnC*P0 + _2BC*P2) * d
								+ _6B*P0 + _n3B1*P1 + _6B*P2;

				return output;
			}

			protected double ClampLoop(double value, double min, double max) {
				double d = max - min;
				return value < min ? value + d : (value >= max ? value - d : value);
			}

			protected double Clamp(double value, double min, double max) {
				return value < min ? min : (value >= max ? max : value);
			}

			public override void OnSetup() {
				base.OnSetup();
				PrecalculateConstants();
			}

			public override void OnVertexBuildHeight(PQS.VertexBuildData data) {
				//base.OnVertexBuildHeight(data); //Normal Bilineal Filtered Heightmap

				//Calculate necesary variables
				double iWidth = 1.0/heightMap.Width;
				double iHeight = 1.0/heightMap.Height;

				double x0 = Math.Floor(data.u * heightMap.Width);
				double y0 = Math.Floor(data.v * heightMap.Height);
				double u0 = x0/heightMap.Width;
				double v0 = y0/heightMap.Height;

				double uD = (data.u - u0) * heightMap.Width;
				double vD = (data.v - v0) * heightMap.Height;

				//Calculate height (Interpolate)
				double[] PY = new double[4];

				for(int j = -1; j < 3; j++) {
					double[] PX = new double[4];
					double mapV = Clamp((y0 + j)/heightMap.Height, 0, 1);
					for(int i = -1; i < 3; i++) {
						double mapU = ClampLoop((x0 + i)/heightMap.Width, -0.5, 0.5);
						PX[i + 1] = heightMap.GetPixelFloat(mapU, mapV);
					}
					PY[j+1] = RunMitchellNetravali(PX[0], PX[1], PX[2], PX[3], uD);
				}

				double heightNormalized = RunMitchellNetravali(PY[0], PY[1], PY[2], PY[3], vD);

				//Apply height
				data.vertHeight += heightMapOffset + heightMapDeformity * (heightNormalized < 0 ? 0 : heightNormalized);
			}
		}
	}
}