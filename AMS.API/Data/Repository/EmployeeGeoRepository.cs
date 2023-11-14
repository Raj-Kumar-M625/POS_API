using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeGeoRepository : Repository<EmployeeGeo>, IEmployeeGeoRepository
    {
        public EmployeeGeoRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
