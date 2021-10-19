using NovelQT.Domain.Core.Commands;
using System;

namespace NovelQT.Domain.Commands.Category
{
    public abstract class CategoryCommand : Command
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

    }
}
