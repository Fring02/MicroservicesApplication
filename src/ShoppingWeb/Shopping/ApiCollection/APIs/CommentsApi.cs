using Grpc.Core;
using Grpc.Net.Client;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;
using Shopping.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.APIs
{
    public class CommentsApi : ICommentsApi
    {
        private readonly CommentService.CommentServiceClient client;

        public CommentsApi(CommentService.CommentServiceClient client)
        {
            this.client = client;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            var request = new CommentRequest
            {
                Description = comment.Description,
                LeavedAt = comment.LeavedAt.ToShortDateString(),
                Username = comment.Username,
                ProductId = comment.ProductId
            };
            var response = await client.AddCommentAsync(request);
            return !string.IsNullOrEmpty(response.Id);
        }

        public async Task<IEnumerable<Comment>> GetComments(string productId)
        {
            var request = new ProductRequest { Id = productId };
            var comments = new LinkedList<Comment>();
            using var stream = client.GetComment(request);
            while(await stream.ResponseStream.MoveNext())
            {
                var response = stream.ResponseStream.Current;
                comments.AddLast(new Comment
                {
                    Id = Guid.Parse(response.Id),
                    Description = response.Description,
                    LeavedAt = DateTime.Parse(response.LeavedAt),
                    ProductId = productId,
                    Username = response.Username
                });
            }
            return comments;
        }
    }
}
