using System.Linq.Expressions;

namespace Library.Application.Services.Albums.Specifications;

public class AlbumByLabelSpec(Guid labelId) : Specification<Album>
{
    private readonly LabelId _labelId = LabelId.Of(labelId);

    public override Expression<Func<Album, bool>> Criteria =>
        a => a.LabelId == _labelId;
}
