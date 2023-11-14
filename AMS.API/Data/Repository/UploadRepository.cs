namespace ProjectOversight.API.Data.Repository
{
    public class UploadRepository : Repository<ProjectOversight.API.Data.Model.Upload>, Interface.IUploadRepository
    {
        public UploadRepository(ProjectOversightContext projectOversightContext): base(projectOversightContext)
        {

        }
    }
}
