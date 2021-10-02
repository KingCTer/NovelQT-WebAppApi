namespace NovelQT.Infra.CrossCutting.Identity.Models
{
    public class CommandInFunction
    {
        public string CommandId { get; set; }
        public Command Command {  get; set; }

        public string FunctionId { get; set; }
        public Function Function {  get; set; }
    }
}
