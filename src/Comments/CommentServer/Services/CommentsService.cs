using CommentServer.Interfaces;
using CommentServer.Models;
using CommentServer.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommentServer.Services
{
    public class CommentsService : CommentService.CommentServiceBase
    {
        private readonly ILogger<CommentsService> _log;
        private readonly ICommentsRepository _comments;

        public CommentsService(ILogger<CommentsService> log, ICommentsRepository comments)
        {
            _log = log;
            _comments = comments;
        }
        public override async Task<CommentIdResponse> AddComment(CommentRequest request, ServerCallContext context)
        {
            var comment = new Comment
            {
                ProductId = request.ProductId,
                LeavedAt = DateTime.Parse(request.LeavedAt),
                Username = request.Username,
                Description = request.Description,
                Id = Guid.NewGuid()
            };
            _log.LogInformation("New comment formed");
            comment = await _comments.Create(comment);
            if (comment != null)
            {
                _log.LogInformation($"Added new comment to database: Id: {comment.Id}," +
                    $" ProductId: {comment.ProductId}, Leaved at: {comment.LeavedAt.ToShortDateString()}");
                return new CommentIdResponse { Id = comment.Id.ToString() };
            } else
            {
                return new CommentIdResponse { Id = null };
            }
        }

        public override async Task GetComment(ProductRequest request, IServerStreamWriter<CommentResponse> responseStream, ServerCallContext context)
        {
            var productComments = await _comments.GetCommentsByProductId(request.Id);
            if (productComments == null || !productComments.Any())
            {
                _log.LogWarning($"Didn't found comments for product with id: {request.Id}");
                return;
            }
            foreach(var comment in productComments)
            {
                var message = new CommentResponse
                {
                    Id = comment.Id.ToString(),
                    Description = comment.Description,
                    LeavedAt = comment.LeavedAt.ToShortDateString(),
                    Username = comment.Username
                };
                await responseStream.WriteAsync(message);
            }
        }
    }
}
