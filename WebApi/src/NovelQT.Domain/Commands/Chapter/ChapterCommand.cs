using NovelQT.Domain.Core.Commands;
using NovelQT.Domain.Models.Enum;
using System;

namespace NovelQT.Domain.Commands.Author;

public abstract class ChapterCommand : Command
{
    public Guid Id { get; protected set; }

    public int Order { get; protected set; }
    public string Name { get; protected set; }
    public string Url { get; protected set; }
    public string Content { get; protected set; }

    public IndexStatusEnum IndexStatus { get; set; }

    public Guid BookId { get; protected set; }

}
