"NuGet.exe" "pack" "..\AmBlitz\AmBlitz.csproj" -Properties Configuration=Debug -IncludeReferencedProjects -symbols
"NuGet.exe" "pack" "..\AmBlitz.Mongo\AmBlitz.Mongo.csproj" -Properties Configuration=Debug -IncludeReferencedProjects -symbols
"NuGet.exe" "pack" "..\AmBlitz.RedisCache\AmBlitz.RedisCache.csproj" -Properties Configuration=Debug -IncludeReferencedProjects -symbols
"NuGet.exe" "pack" "..\AmBlitz.RabbitMQ\AmBlitz.RabbitMQ.csproj" -Properties Configuration=Debug -IncludeReferencedProjects -symbols
