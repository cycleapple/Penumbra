Build Penumbra first, then run dotnet run --project regression-tests/Regression.csproj -c Release.
The checks load the actual built GameData library and verify category 23 and shared item cache invalidation.
Override the API13 library directory with -p:DalamudTestPath=/path/to/dalamud if needed.
