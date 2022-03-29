using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration.Parsing;
using UnityEngine;

namespace NiakoKerbalMods {

	namespace NiakoKopernicus {

		///	<summary>
		///	Kopernicus Parser for the PQSMod VertexBilinealHeightMap
		///	</summary>

		[RequireConfigType(Kopernicus.ConfigParser.Enumerations.ConfigType.Node)]
		public class VertexMitchellNetravaliHeightMap : ModLoader<PQSMod_VertexMitchellNetravaliHeightMap> {
			/*
				PQS Input Values, mapped directly to the normal VertexHeightMap values
			*/
			[ParserTarget("map")]
			public MapSOParserRGB<MapSO> heightMap {
				get { return Mod.heightMap; }
				set { Mod.heightMap = value; }
			}

			[ParserTarget("deformity")]
			public NumericParser<double> heightMapDeformity {
				get { return Mod.heightMapDeformity; }
				set { Mod.heightMapDeformity = value; }
			}

			[ParserTarget("offset")]
			public NumericParser<double> heightMapOffset {
				get { return Mod.heightMapOffset; }
				set { Mod.heightMapOffset = value; }
			}

			[ParserTarget("scaleDeformityByRadius")]
			public NumericParser<bool> scaleDeformityByRadius {
				get { return Mod.scaleDeformityByRadius; }
				set { Mod.scaleDeformityByRadius = value; }
			}

			[ParserTarget("B")]
			public NumericParser<float> B {
				get { return Mod.B; }
				set { Mod.B = value; }
			}

			[ParserTarget("C")]
			public NumericParser<float> C {
				get { return Mod.C; }
				set { Mod.C = value; }
			}
		}
	}
}