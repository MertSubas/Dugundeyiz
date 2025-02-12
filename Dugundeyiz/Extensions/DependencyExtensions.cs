﻿using Dugundeyiz.Context;
using Dugundeyiz.Entity.Interfaces;
using Dugundeyiz.Identity;
using Dugundeyiz.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Dugundeyiz.Extensions
{
    public static class DependencyExtensions
    {

        public static void AddExtensions(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepostory<>), typeof(GenericRepostory<>));

            //services.AddScoped<IAboutService, AboutService>();
            //services.AddScoped<IBlogPostService, BlogPostService>();
            //services.AddScoped<ICommentService, CommentService>();
            //services.AddScoped<IReplyCommentService, ReplyCommentService>();
            //services.AddScoped<ISlider, SliderService>();
            //services.AddScoped<ISocial, SocialService>();

            //services.AddScoped<IEducationService, EducationService>();

            //services.AddScoped<IMovieService, MovieService>();


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;  //karakter istemesin
                options.Password.RequiredLength = 3;  //uzunluğu en az 3 karakter olsun
                options.Password.RequireUppercase = false; //büyük harf istemesin
                options.Password.RequireLowercase = false;  //Küçük harf istemesin
                options.Password.RequireDigit = false; //sayı istemesin
                                                       //options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvyzqw0123456789";  sadece bunlar kabul edilsin
                options.User.RequireUniqueEmail = false; //e mail eşsisiz olmalı
                options.Lockout.MaxFailedAccessAttempts = 3;  //3 yanlış denemeden sonra girişi altaki süre kadar durdur
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // üstteki sayı kadaryanlış girişten sonra 1 dk girişi engeller
            }).AddEntityFrameworkStores<DugundeyizContext>().AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(op =>
            {
                op.LoginPath = new PathString("/");   //giriş için sayfaya yönlendir
                op.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        // Kullanıcı giriş yapmamışsa anasayfaya yönlendirme
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
                //op.LogoutPath = new PathString("/Account/Logout"); //çıkış olursa sayfya yönlendir
                op.ExpireTimeSpan = TimeSpan.FromMinutes(900); //cookie ömrü dk
                                                               //op.AccessDeniedPath = new PathString("/Account/AccessDenied");
                op.AccessDeniedPath = "/";
                //op.AccessDeniedPath = new PathString("yetisi yok sayfası"); // yetkisi olmayinca yönlendirme
                op.SlidingExpiration = true; //üsstteki 10 dk dolmadan tekar login olursa tekrar süreyi başa alır
                op.Cookie = new CookieBuilder()
                {
                    Name = "IdentityAppCookie", //cookie adı
                    HttpOnly = true,  //sadece tarayıcıdan girilsin programlar yakalayamayacak

                };

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("PartnerPolicy", policy => policy.RequireRole("Partner"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
            });
        }
    }
}
