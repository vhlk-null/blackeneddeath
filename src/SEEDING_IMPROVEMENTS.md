# Seeding Implementation Improvements

## Current Issues

1. **Hybrid Seeding Approach**: Base entities via migrations, junction tables at runtime - confusing
2. **Fragile Check Logic**: Only checks `AlbumBands.AnyAsync()` - doesn't verify base entity integrity
3. **GUID Duplication**: Same GUIDs hardcoded in multiple seed files
4. **No Transaction Support**: Partial failures leave inconsistent state
5. **Missing Namespace**: CountrySeed.cs missing namespace
6. **Static Mutable State**: `_shouldSeedData` flag in ModelBuilder extensions

## Recommended Solution: Runtime-Only Seeding

### Benefits
- ✅ Full control over seeding logic and order
- ✅ Better error handling and transaction support
- ✅ Can check/update seed data without new migrations
- ✅ Consistent approach (one place to look)
- ✅ Easier to add conditional seeding (dev/staging/prod)

### Implementation Steps

#### 1. Extract GUIDs to Constants Class

```csharp
// Data/Seed/SeedConstants.cs
namespace Archive.API.Data.Seeds
{
    public static class SeedConstants
    {
        // Countries
        public static class Countries
        {
            public static readonly Guid Norway = Guid.Parse("c0000000-0000-0000-0000-000000000001");
            public static readonly Guid Sweden = Guid.Parse("c0000000-0000-0000-0000-000000000002");
            public static readonly Guid Finland = Guid.Parse("c0000000-0000-0000-0000-000000000003");
            public static readonly Guid Poland = Guid.Parse("c0000000-0000-0000-0000-000000000004");
        }

        // Bands
        public static class Bands
        {
            public static readonly Guid Darkthrone = Guid.Parse("b0000000-0000-0000-0000-000000000001");
            public static readonly Guid Burzum = Guid.Parse("b0000000-0000-0000-0000-000000000002");
            public static readonly Guid Emperor = Guid.Parse("b0000000-0000-0000-0000-000000000003");
            public static readonly Guid Mayhem = Guid.Parse("b0000000-0000-0000-0000-000000000004");
            public static readonly Guid Dissection = Guid.Parse("b0000000-0000-0000-0000-000000000005");
            public static readonly Guid Behemoth = Guid.Parse("b0000000-0000-0000-0000-000000000006");
        }

        // Albums
        public static class Albums
        {
            public static readonly Guid TransilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            public static readonly Guid Filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            public static readonly Guid NightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            public static readonly Guid DeMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            public static readonly Guid StormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            public static readonly Guid TheSatanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");
        }

        // Genres
        public static class Genres
        {
            public static readonly Guid Metal = Guid.Parse("10000000-0000-0000-0000-000000000001");
            public static readonly Guid BlackMetal = Guid.Parse("20000000-0000-0000-0000-000000000001");
            public static readonly Guid RawBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000001");
            public static readonly Guid AtmosphericBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000003");
            public static readonly Guid SymphonicBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000004");
        }
    }
}
```

#### 2. Improved DatabaseSeeder with Transactions

```csharp
// Extenstions/DatabaseSeeder.cs
namespace Archive.API.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ArchiveContext>();

            try
            {
                logger.LogInformation("Checking if database seeding is required...");

                // Better check: verify key entities exist
                if (await IsAlreadySeededAsync(context))
                {
                    logger.LogInformation("Database already seeded. Skipping...");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // Use transaction for atomicity
                await using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    await SeedBaseEntitiesAsync(context, logger);
                    await SeedJunctionTablesAsync(context, logger);

                    await transaction.CommitAsync();
                    logger.LogInformation("Database seeding completed successfully!");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task<bool> IsAlreadySeededAsync(ArchiveContext context)
        {
            // Check multiple tables for robustness
            return await context.Countries.AnyAsync() &&
                   await context.Genres.AnyAsync() &&
                   await context.Albums.AnyAsync() &&
                   await context.Bands.AnyAsync();
        }

        private static async Task SeedBaseEntitiesAsync(ArchiveContext context, ILogger logger)
        {
            // Seed in dependency order
            if (!await context.Countries.AnyAsync())
            {
                logger.LogInformation("Seeding Countries...");
                await context.Countries.AddRangeAsync(CountrySeed.GetCountries());
                await context.SaveChangesAsync();
            }

            if (!await context.Genres.AnyAsync())
            {
                logger.LogInformation("Seeding Genres...");
                await context.Genres.AddRangeAsync(GenreSeed.GetGenres());
                await context.SaveChangesAsync();
            }

            if (!await context.Tracks.AnyAsync())
            {
                logger.LogInformation("Seeding Tracks...");
                await context.Tracks.AddRangeAsync(TrackSeed.GetTracks());
                await context.SaveChangesAsync();
            }

            if (!await context.Bands.AnyAsync())
            {
                logger.LogInformation("Seeding Bands...");
                await context.Bands.AddRangeAsync(BandSeed.GetBands());
                await context.SaveChangesAsync();
            }

            if (!await context.Albums.AnyAsync())
            {
                logger.LogInformation("Seeding Albums...");
                await context.Albums.AddRangeAsync(AlbumSeed.GetAlbums());
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJunctionTablesAsync(ArchiveContext context, ILogger logger)
        {
            if (!await context.AlbumBands.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumBands...");
                await context.AlbumBands.AddRangeAsync(AlbumRelationshipsSeed.GetAlbumBands());
            }

            if (!await context.AlbumGenres.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumGenres...");
                await context.AlbumGenres.AddRangeAsync(AlbumRelationshipsSeed.GetAlbumGenres());
            }

            if (!await context.AlbumCountries.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumCountries...");
                await context.AlbumCountries.AddRangeAsync(AlbumRelationshipsSeed.GetAlbumCountries());
            }

            if (!await context.AlbumTracks.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumTracks...");
                await context.AlbumTracks.AddRangeAsync(AlbumRelationshipsSeed.GetAlbumTracks());
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Junction tables seeded successfully!");
        }
    }
}
```

#### 3. Remove HasData() from ModelBuilder Extensions

```csharp
// Remove these blocks from ArchiveModelBuilderExtensions.cs:
// - Line 7: Remove static _shouldSeedData field
// - Line 9-12: Remove EnableSeeding() method
// - Lines 48-51, 97-98, 119-120, 151-152, 179-180: Remove all "if (_shouldSeedData) entity.HasData(...)" blocks
```

#### 4. Update ArchiveContext OnModelCreating

```csharp
// Data/ArchiveContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Remove this line:
    // if (_environment?.IsDevelopment() == true)
    //     modelBuilder.EnableSeeding();

    modelBuilder.SetupGenre();
    modelBuilder.SetupCountry();
    modelBuilder.SetupTrack();
    modelBuilder.SetupBand();
    modelBuilder.SetupAlbum();
    modelBuilder.SetupStreamingLink();
    modelBuilder.SetupAlbumBand();
    modelBuilder.SetupAlbumGenre();
    modelBuilder.SetupAlbumCountry();
    modelBuilder.SetupAlbumTrack();
    modelBuilder.SetupBandGenre();
}
```

#### 5. Fix CountrySeed.cs Namespace

```csharp
// Data/Seed/CountrySeed.cs - Add namespace
namespace Archive.API.Data.Seeds
{
    public static class CountrySeed
    {
        // ... existing code
    }
}
```

#### 6. Update Seed Classes to Use Constants

```csharp
// Example: BandSeed.cs
public static List<Band> GetBands()
{
    return new List<Band>
    {
        new Band
        {
            Id = SeedConstants.Bands.Darkthrone,
            Name = "Darkthrone",
            Bio = "Norwegian black metal band formed in 1986.",
            CountryId = SeedConstants.Countries.Norway,
            FormedYear = 1986,
            Status = BandStatus.Active
        },
        // ... etc
    };
}
```

### Migration Path

1. Create new migration to remove seed data: `dotnet ef migrations add RemoveSeedData`
2. Implement SeedConstants class
3. Update DatabaseSeeder with transaction support
4. Update all seed classes to use SeedConstants
5. Remove `HasData()` calls from ModelBuilder extensions
6. Test thoroughly in development environment

## Alternative: Improve Current Hybrid Approach (Minimal Changes)

If you don't want to refactor everything:

### Quick Wins
1. ✅ Add namespace to CountrySeed.cs
2. ✅ Extract GUIDs to SeedConstants
3. ✅ Improve seeding check to verify multiple tables
4. ✅ Add transaction support to junction table seeding
5. ✅ Complete BandGenre relationship configuration
6. ✅ Better logging for each seeding step

### Updated DatabaseSeeder (Minimal Version)

```csharp
private static async Task<bool> IsAlreadySeededAsync(ArchiveContext context)
{
    // Check both base entities (from migrations) and junction tables
    var hasBaseData = await context.Albums.AnyAsync() &&
                      await context.Bands.AnyAsync();
    var hasJunctionData = await context.AlbumBands.AnyAsync() &&
                          await context.AlbumGenres.AnyAsync();

    return hasBaseData && hasJunctionData;
}

private static async Task SeedJunctionTablesAsync(ArchiveContext context, ILogger logger)
{
    // Use transaction
    await using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        // ... existing seeding code

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        logger.LogInformation("Junction tables seeded successfully!");
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

## Summary

**Recommended**: Move to runtime-only seeding for consistency and flexibility.

**Quick fix**: Add transaction support, improve checks, extract GUID constants.
