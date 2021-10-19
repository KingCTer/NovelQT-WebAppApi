using NovelQT.Domain.Core.Commands;
using System;

namespace NovelQT.Domain.Commands.Author
{
    public abstract class AuthorCommand : Command
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

    }
}
