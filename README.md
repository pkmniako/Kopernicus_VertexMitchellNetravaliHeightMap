Check the [**wiki**](https://github.com/pkmniako/Kopernicus_VertexMitchellNetravaliHeightMap/wiki) for updated information.

# Better Heightmap Algorithm for Kerbal Space Program

Mod for Kerbal Space Program that adds a custom PQS Mod which implements a variant of ``VertexHeightMap`` using _Mitchell-Netravali Filtering_ instead of _Bilineal Filtering_.

This reduces the amount of _pixelization_ at cliffs and other decently-detailed areas of a heightmap.

This mod still requires testing (Just in case), so use it with caution.

---

## Usage

With a compiled .dll file inside the ``GameData``, you can use the ``VertexMitchellNetravaliHeightMap`` PQS Mod in [Kopernicus](https://github.com/Kopernicus/Kopernicus) Configuration Files.

The PQSs mod also contains two new parameters: ``B`` and ``C``. These control what type of filtering is done. Values ``B = 1/3, C = 1/3`` and ``B = 1, C = 0`` have been tested and yield decent results (Though the later is by far the most smooth).

An example of a ``VertexMitchellNetravaliHeightMap`` node:

```default
VertexMitchellNetravaliHeightMap
{
	map = 000_TESTING/PluginData/Niko_height.png
	offset = -1920
	deformity = 7100
	scaleDeformityByRadius = false
	order = 1
	enabled = true

	B = 0.33333333
	C = 0.33333333
}
```

By default, ``B`` and ``C`` are equal to ``1/3``.

---
---

## Comparison

_VertexMitchellNetravaliHeightMap, B = 1, C = 0_

![](README_IMGs/comparison_B1_C0.png)

_VertexHeightMap, Stock_

![](README_IMGs/comparison_VertexHeightMap.png)

---
---

## Compilation

### Compilation dependencies

- Have a folder `lib/gamedata` in the root of the project be a `GameData` folder with [Kopernicus](https://github.com/Kopernicus/Kopernicus) installed
- Have a folder `lib/ksp` in the root of the project be a KSP's `KSP_Data/Managed`[^1] folder

Recommended to use symbolic links. You can also edit `VertexMitchellNetravaliHeightMap.csproj` if you so wish to change the include paths.

### Compilation command

Two options:
- Run the Makefile with `make`
. Run ``dotnet build --configuration Release -o bin``

---
---


## Mathematics

Taked from [Wikipedia](https://en.wikipedia.org/wiki/Mitchell%E2%80%93Netravali_filters), using _Mitchell_'s and _Netravali_'s cubic alternate resampling formula:

![Formula from Wikipedia](README_IMGs/formula1.png)

---
---

[^1]: In Windows, or if using Proton, this should be `KSP_x64_Data/Managed`