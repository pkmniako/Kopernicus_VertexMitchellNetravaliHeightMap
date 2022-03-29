all:	clear
	@dotnet build --configuration Release -o bin
	cp -fr bin/VertexMitchellNetravaliHeightMap.dll "D:\Users\Niako\SteamLibrary\steamapps\common\Kerbal Space Program - 1.12\GameData\000_TESTING\VertexMitchellNetravaliHeightMap.dll"

clear:
	@rm -r -f bin