using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class ProjectObjectiveDto
    {
        public List<ProjectObjective> ProjectObjectives { get; set; }
        public List<ProjectObjectiveMapping> ProjectObjectiveMappings { get; set; }
    }
}
