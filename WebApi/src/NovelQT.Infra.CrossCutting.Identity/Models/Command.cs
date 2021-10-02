namespace NovelQT.Infra.CrossCutting.Identity.Models
{
    public class Command
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<CommandInFunction> CommandInFunctions { get; set; }
    }
}
