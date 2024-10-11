using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using ECOMM.Data.Shared.Concrete;
using ECOMM.Services.Abstract;
using ECOMM.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace ECOMM.Business.Configuration
{
    public static class BusinessExtension
    {
        public static void BusinessDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<ICommentService, CommentService>();
            //services.AddScoped<IPostService, PostService>();
            //services.AddScoped<ITagService, TagService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IUserService, UserService>();
        }

        public static void RepositoryDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
