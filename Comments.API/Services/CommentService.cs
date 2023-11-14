using Comments.API.Data.Models;
using Comment = Comments.API.Data.Models.Comments;
using Comments.API.Services.Interface;
using Comments.API.Data.Repository.Interface;

namespace Comments.API.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _repository;
        public CommentService(IUnitOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CommentHierarchy>> GetComments()
        {
            try
            {
                var commentHierarchy = await _repository.CommentHierarchy.FindAllAsync();

                foreach(var obj in commentHierarchy)
                {
                    var comment = await _repository.Comment.FindById(x => x.CommentHierarchyId == obj.Id);
                    obj.Description = comment.Description;
                }

                return commentHierarchy;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public async Task<bool> AddComment(User user,CommentHierarchy commentHierarchy)
        {
            try
            {
                commentHierarchy.CreatedDate = DateTime.Now;
                commentHierarchy.UpdatedDate = DateTime.Now;
                commentHierarchy.CreatedBy = user.Id.ToString();
                commentHierarchy.UpdatedBy = user.Id.ToString();

                var commthierarchy = await _repository.CommentHierarchy.CreateAsync(commentHierarchy);

                var comment = new Comment()
                {
                    CommentHierarchyId = commthierarchy.Id,
                    Description = commentHierarchy.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id.ToString(),
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                };

                await _repository.Comment.CreateAsync(comment);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateComment(User user,CommentHierarchy commentHierarchy)
        {
            try
            {
                var commenthierarchy = await _repository.CommentHierarchy.FindById(x => x.Id == commentHierarchy.Id);
                commenthierarchy.LiteralName = commentHierarchy.LiteralName;
                commenthierarchy.Description = commentHierarchy.Description;
                commenthierarchy.ProjectName = commentHierarchy.ProjectName;
                commenthierarchy.TableName = commentHierarchy.TableName;
                commenthierarchy.UpdatedBy = user.Id.ToString();
                commenthierarchy.UpdatedDate = DateTime.Now;
                await _repository.CommentHierarchy.UpdateAsync(commenthierarchy);

                var comment = await _repository.Comment.FindById(x => x.CommentHierarchyId == commentHierarchy.Id);
                comment.Description = commentHierarchy.Description;
                comment.UpdatedBy = user.Id.ToString();
                comment.UpdatedDate = DateTime.Now;
                await _repository.Comment.UpdateAsync(comment);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
