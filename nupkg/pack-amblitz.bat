"NuGet.exe" "pack" "..\AmBlitz\AmBlitz.csproj" -Properties Configuration=Release -IncludeReferencedProjects -symbols
"NuGet.exe" "pack" "..\AmBlitz.Mongo\AmBlitz.Mongo.csproj" -Properties Configuration=Release -IncludeReferencedProjects -symbols
"NuGet.exe" "pack" "..\AmBlitz.RedisCache\AmBlitz.RedisCache.csproj" -Properties Configuration=Release -IncludeReferencedProjects -symbols
