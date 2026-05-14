using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Application.Services.Albums.Jobs
{
    public interface IAlbumReleaseJob
    {
        Task ExecuteAsync(Guid guild, string albumTitle, string ablumSlug, int releaseYear, CancellationToken cancellationToken);
    }
}
