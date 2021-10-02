namespace NovelQT.Infra.CrossCutting.Identity.Models
{
    public class Function
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public string ParentId { get; set; }

        public IList<CommandInFunction> CommandInFunctions { get; set; }

    }
}
