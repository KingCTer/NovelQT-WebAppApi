namespace NovelQT.Infra.CrossCutting.Identity.Models
{
    public class Permission
    {
        public string RoleId { get; set; }

        public string FunctionId { get; set; }
        public Function Function { get; set; }

        public string CommandId { get; set; }
        public Command Command { get; set; }
    }
}
