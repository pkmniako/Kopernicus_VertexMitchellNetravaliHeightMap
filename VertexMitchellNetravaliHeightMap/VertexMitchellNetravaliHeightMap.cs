using System;
using System.Diagnostics.CodeAnalysis;
using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration.Parsing;
using Kopernicus.ConfigParser.Enumerations;
using UnityEngine;

namespace NiakoKerbalMods {

	namespace NiakoKopernicus {

		///	<summary>
		///	Kopernicus Parser for the PQSMod VertexBilinealHeightMap
		///	</summary>

    	[RequireConfigType(ConfigType.Node)]
    	[SuppressMessage("ReSharper", "UnusedMember.Global")]
		public class VertexMitchellNetravaliHeightMap : ModLoader<PQSMod_VertexMitchellNetravaliHeightMap> {
			/// <summary>
			/// Reference to the heightmap to be used in this PQS mod
			/// </summary>
			[ParserTarget("map")]
			public MapSOParserGreyScale<MapSO> HeightMap
			{
				get { return Mod.heightMap; }
				set { Mod.heightMap = value; }
			}

			/// <summary>
			/// Height map offset
			/// </summary>
			[ParserTarget("offset")]
			public NumericParser<Double> HeightMapOffset
			{
				get { return Mod.heightMapOffset; }
				set { Mod.heightMapOffset = value; }
			}

			/// <summary>
			/// Heightmap deformity, that is, in meters, the altitude difference
			/// between the deepest and the tallest point of the heightmap, unless
			/// <see cref="ScaleDeformityByRadius"/> is set 
			/// </summary>
			[ParserTarget("deformity")]
			public NumericParser<Double> HeightMapDeformity
			{
				get { return Mod.heightMapDeformity; }
				set { Mod.heightMapDeformity = value; }
			}

			/// <summary>
			/// If set to true, <see cref="HeightMapDeformity"/> works as a scalar
			/// instead of being an absolute height
			/// </summary>
			[ParserTarget("scaleDeformityByRadius")]
			public NumericParser<Boolean> ScaleDeformityByRadius
			{
				get { return Mod.scaleDeformityByRadius; }
				set { Mod.scaleDeformityByRadius = value; }
			}

			/// <summary>
			/// Parameter B for the Mitchell-Netravali algorithm
			/// </summary>
			[ParserTarget("B")]
			public NumericParser<Double> B {
				get { return Mod.B; }
				set { Mod.B = value; }
			}

			/// <summary>
			/// Parameter C for the Mitchell-Netravali algorithm
			/// </summary>
			[ParserTarget("C")]
			public NumericParser<Double> C {
				get { return Mod.C; }
				set { Mod.C = value; }
			}
		}
	}
}