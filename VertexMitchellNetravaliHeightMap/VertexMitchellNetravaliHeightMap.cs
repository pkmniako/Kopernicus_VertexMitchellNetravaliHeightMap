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
			// The map texture for the planet
			[ParserTarget("map")]
			public MapSOParserGreyScale<MapSO> HeightMap
			{
				get { return Mod.heightMap; }
				set { Mod.heightMap = value; }
			}

			// Height map offset
			[ParserTarget("offset")]
			public NumericParser<Double> HeightMapOffset
			{
				get { return Mod.heightMapOffset; }
				set { Mod.heightMapOffset = value; }
			}

			// Height map offset
			[ParserTarget("deformity")]
			public NumericParser<Double> HeightMapDeformity
			{
				get { return Mod.heightMapDeformity; }
				set { Mod.heightMapDeformity = value; }
			}

			// Height map offset
			[ParserTarget("scaleDeformityByRadius")]
			public NumericParser<Boolean> ScaleDeformityByRadius
			{
				get { return Mod.scaleDeformityByRadius; }
				set { Mod.scaleDeformityByRadius = value; }
			}

			[ParserTarget("B")]
			public NumericParser<Double> B {
				get { return Mod.B; }
				set { Mod.B = value; }
			}

			[ParserTarget("C")]
			public NumericParser<Double> C {
				get { return Mod.C; }
				set { Mod.C = value; }
			}
		}
	}
}