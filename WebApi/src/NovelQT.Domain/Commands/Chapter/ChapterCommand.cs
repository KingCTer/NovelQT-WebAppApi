using NovelQT.Domain.Core.Commands;
using System;

namespace NovelQT.Domain.Commands.Author;

public abstract class ChapterCommand : Command
{
    public Guid Id { get; protected set; }

    public int Order { get; protected set; }
    public string Name { get; protected set; }
    public string Url { get; protected set; }
    public string Content { get; protected set; }

    public Guid BookId { get; protected set; }

}
