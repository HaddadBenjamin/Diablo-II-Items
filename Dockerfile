# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
# WORKDIR /source
	
# copy csproj and restore as distinct layers
COPY *.sln .
COPY */*.csproj ./aspnetapp/
RUN dotnet restore #  Restaure les d�pendances d�un projet.
	
# copy everything else and build app
COPY */. ./aspnetapp/
WORKDIR /aspnetapp
RUN dotnet publish -c release -o /app --no-restore # Publie l�application et ses d�pendances dans un dossier pour le d�ploiement sur un syst�me d�h�bergement.
# -c release : configuration de release (de production) : comment faire pour publier en test, hom, prod de fa�on �xplicite ?
# -o /app : sp�cifie le chemin d�acc�s du r�pertoire de sortie.
# --no-restore : n'effectue pas de restauration implicite � l�ex�cution de la commande.
	
# final stage/image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY --from=build /app ./ # Logiquement on ne devrait avoir besoin que de �a, rien d'autre, donc j'ai l'impression que mon image est lourde pour rien l�.
ENTRYPOINT ["dotnet", "aspnetapp.dll"] # La dll ne devrait pas �tre bonne, faire la commande et voir ce que �a me g�n�re.
		
# Comment je m'assure que c'est bien le projet DiabloII.Application qui est runn� ?