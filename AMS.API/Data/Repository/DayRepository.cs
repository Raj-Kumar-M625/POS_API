using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class DayRepository : Repository<Day>, IDayRepository
    {
        public DayRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
