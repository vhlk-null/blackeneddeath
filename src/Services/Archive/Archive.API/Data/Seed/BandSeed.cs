namespace Archive.API.Data.Seeds
{
    public static class BandSeed
    {
        public static List<Band> GetBands()
        {
            var norwayId = Guid.Parse("c0000000-0000-0000-0000-000000000001");
            var swedenId = Guid.Parse("c0000000-0000-0000-0000-000000000002");
            var polandId = Guid.Parse("c0000000-0000-0000-0000-000000000004");

            return new List<Band>
            {
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                    Name = "Darkthrone",
                    Bio = "Norwegian black metal band formed in 1986.",
                    CountryId = norwayId,
                    FormedYear = 1986,
                    Status = BandStatus.Active
                },
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"),
                    Name = "Burzum",
                    Bio = "Norwegian black metal solo project by Varg Vikernes.",
                    CountryId = norwayId,
                    FormedYear = 1991,
                    Status = BandStatus.OnHold
                },
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"),
                    Name = "Emperor",
                    Bio = "Norwegian black metal band, pioneers of symphonic black metal.",
                    CountryId = norwayId,
                    FormedYear = 1991,
                    DisbandedYear = 2001,
                    Status = BandStatus.SplitUp
                },
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"),
                    Name = "Mayhem",
                    Bio = "Norwegian black metal band, one of the genre's founders.",
                    CountryId = norwayId,
                    FormedYear = 1984,
                    Status = BandStatus.Active
                },
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000005"),
                    Name = "Dissection",
                    Bio = "Swedish melodic black/death metal band.",
                    CountryId = swedenId,
                    FormedYear = 1989,
                    DisbandedYear = 2006,
                    Status = BandStatus.SplitUp
                },
                new Band
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000006"),
                    Name = "Behemoth",
                    Bio = "Polish blackened death metal band.",
                    CountryId = polandId,
                    FormedYear = 1991,
                    Status = BandStatus.Active
                }
            };
        }
    }
}