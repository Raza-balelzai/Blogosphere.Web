﻿using Blogosphere.Web.Models.Domain;

namespace Blogosphere.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId);

    }
}
