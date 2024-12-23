﻿using ECOMM.Business.Abstract;
using ECOMM.Business.Concrete;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using ECOMM.Data.Shared.Concrete;

using Microsoft.Extensions.DependencyInjection;

namespace ECOMM.Business.Configuration
{
    public static class BusinessExtension
    {
        public static void BusinessDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>(); services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IFavouritesService, FavouritesService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        public static void RepositoryDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
