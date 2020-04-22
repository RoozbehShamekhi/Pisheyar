﻿using System;
using System.Collections.Generic;
using System.Text;
using Pisheyar.Application.Common.Interfaces;
using Pisheyar.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Pisheyar.Domain.Enums;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace Pisheyar.Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<GetAllPostVm>
    {
        public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, GetAllPostVm>
        {
            private readonly IPisheyarMagContext _context;
            private readonly IMapper _mapper;

            public GetAllPostsQueryHandler(IPisheyarMagContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetAllPostVm> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
            {
                var posts = await _context.TblPost
                    .Where(x => !x.PostIsDelete)
                    .OrderByDescending(x => x.PostModifyDate)
                    .ProjectTo<GetAllPostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                if (posts.Count == 0)
                {
                    return new GetAllPostVm()
                    {
                        Message = "پستی یافت نشد",
                        State = (int)GetAllPostsState.NoPosts
                    };
                }

                foreach (var post in posts)
                {
                    var postCategory = await _context.TblPostCategory
                    .Where(x => x.PcPostGuid == post.PostGuid)
                    .OrderBy(x => x.PcCategoryGu.CategoryDisplay)
                    .Select(x => new GetAllPostCategoryNameDto
                    {
                        Guid = x.PcCategoryGu.CategoryGuid,
                        Title = x.PcCategoryGu.CategoryDisplay

                    }).FirstOrDefaultAsync(cancellationToken);

                    if (postCategory != null)
                    {
                        post.Category = postCategory;
                    }

                    var postTags = await _context.TblPostTag
                        .Where(x => x.PtPostGuid == post.PostGuid)
                        .OrderBy(x => x.PtTag.TagName)
                        .Select(x => new GetAllPostTagNameDto
                        {
                            Guid = x.PtTag.TagGuid,
                            Name = x.PtTag.TagName

                        }).ToListAsync(cancellationToken);

                    if (postTags.Count > 0)
                    {
                        post.Tags = postTags;
                    }
                }

                return new GetAllPostVm()
                {
                    Message = "عملیات موفق آمیز",
                    State = (int)GetAllPostsState.Success,
                    Posts = posts
                };
            }
        }
    }
}
