all:	clear
	@dotnet build --configuration Release -o bin

clear:
	@rm -r -f bin
