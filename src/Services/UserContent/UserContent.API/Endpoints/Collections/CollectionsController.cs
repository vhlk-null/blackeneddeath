using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace UserContent.API.Endpoints.Collections;

public record CreateCollectionFormRequest
{
    public string Collection { get; init; } = string.Empty;
    public IFormFile? CoverImage { get; init; }
}

public record UpdateCollectionFormRequest
{
    public string Collection { get; init; } = string.Empty;
    public IFormFile? CoverImage { get; init; }
}

public record AddAlbumToCollectionRequest(Guid AlbumId);
public record AddBandToCollectionRequest(Guid BandId);

[ApiController]
[Route("collections")]
[Tags("Collections")]
[Authorize]
public class CollectionsController(IUserContentService service) : ControllerBase
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<CollectionSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserCollections(Guid userId, CancellationToken ct)
        => Ok(await service.GetUserCollectionsAsync(userId, ct));

    [HttpGet("{collectionId:guid}")]
    [ProducesResponseType(typeof(CollectionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCollection(Guid collectionId, CancellationToken ct)
        => Ok(await service.GetCollectionAsync(collectionId, ct));

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CollectionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCollection([FromForm] CreateCollectionFormRequest form, CancellationToken ct)
    {
        CreateCollectionRequest? request = JsonSerializer.Deserialize<CreateCollectionRequest>(form.Collection, JsonOptions);
        if (request is null) return BadRequest("Could not deserialize collection data.");

        Stream? imageStream = await ToStreamAsync(form.CoverImage, ct);
        CollectionDto result = await service.CreateCollectionAsync(request, imageStream, form.CoverImage?.ContentType, form.CoverImage?.FileName, ct);
        return CreatedAtAction(nameof(GetCollection), new { collectionId = result.Id }, result);
    }

    [HttpPut("{collectionId:guid}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CollectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCollection(Guid collectionId, [FromForm] UpdateCollectionFormRequest form, CancellationToken ct)
    {
        UpdateCollectionRequest? request = JsonSerializer.Deserialize<UpdateCollectionRequest>(form.Collection, JsonOptions);
        if (request is null) return BadRequest("Could not deserialize collection data.");

        CollectionDto result = await service.UpdateCollectionAsync(collectionId, request, ct);

        if (form.CoverImage is not null)
        {
            Stream imageStream = await ToStreamAsync(form.CoverImage, ct) ?? Stream.Null;
            result = await service.UpdateCollectionCoverAsync(collectionId, imageStream, form.CoverImage.ContentType, form.CoverImage.FileName, ct);
        }

        return Ok(result);
    }

    [HttpDelete("{collectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCollection(Guid collectionId, CancellationToken ct)
    {
        await service.DeleteCollectionAsync(collectionId, ct);
        return NoContent();
    }

    [HttpPost("{collectionId:guid}/albums")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAlbum(Guid collectionId, AddAlbumToCollectionRequest request, CancellationToken ct)
    {
        await service.AddAlbumToCollectionAsync(collectionId, request.AlbumId, ct);
        return NoContent();
    }

    [HttpDelete("{collectionId:guid}/albums/{albumId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAlbum(Guid collectionId, Guid albumId, CancellationToken ct)
    {
        await service.RemoveAlbumFromCollectionAsync(collectionId, albumId, ct);
        return NoContent();
    }

    [HttpPost("{collectionId:guid}/bands")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddBand(Guid collectionId, AddBandToCollectionRequest request, CancellationToken ct)
    {
        await service.AddBandToCollectionAsync(collectionId, request.BandId, ct);
        return NoContent();
    }

    [HttpDelete("{collectionId:guid}/bands/{bandId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveBand(Guid collectionId, Guid bandId, CancellationToken ct)
    {
        await service.RemoveBandFromCollectionAsync(collectionId, bandId, ct);
        return NoContent();
    }

    private static async Task<Stream?> ToStreamAsync(IFormFile? file, CancellationToken ct)
    {
        if (file is null) return null;
        MemoryStream stream = new();
        await file.CopyToAsync(stream, ct);
        stream.Position = 0;
        return stream;
    }
}
