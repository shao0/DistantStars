﻿// <auto-generated />
using System;
using DistantStars.Server.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DistantStars.Server.DBContext.Migrations
{
    [DbContext(typeof(EFCoreContext))]
    [Migration("20210909065403_ModifyMenuType")]
    partial class ModifyMenuType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("DistantStars.Server.Model.Models.FileInfoModel", b =>
                {
                    b.Property<string>("MD5")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .HasColumnType("text");

                    b.Property<int>("FileType")
                        .HasColumnType("int");

                    b.HasKey("MD5");

                    b.ToTable("FileInfoModel");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.MenuInfo", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("MenuHeader")
                        .HasColumnType("text");

                    b.Property<string>("MenuIcon")
                        .HasColumnType("text");

                    b.Property<int>("MenuType")
                        .HasColumnType("int");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("TargetView")
                        .HasColumnType("text");

                    b.HasKey("MenuId");

                    b.ToTable("MenuInfo");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.RoleInfo", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("RoleInfo");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.RoleMenu", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("RoleMenu");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleInfoRoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserAccount")
                        .HasColumnType("text");

                    b.Property<string>("UserIcon")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("UserPassword")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleInfoRoleId");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.RoleMenu", b =>
                {
                    b.HasOne("DistantStars.Server.Model.Models.MenuInfo", "Menu")
                        .WithMany("RoleMenus")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistantStars.Server.Model.Models.RoleInfo", "Role")
                        .WithMany("RoleMenus")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.UserInfo", b =>
                {
                    b.HasOne("DistantStars.Server.Model.Models.RoleInfo", "RoleInfo")
                        .WithMany()
                        .HasForeignKey("RoleInfoRoleId");

                    b.Navigation("RoleInfo");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.MenuInfo", b =>
                {
                    b.Navigation("RoleMenus");
                });

            modelBuilder.Entity("DistantStars.Server.Model.Models.RoleInfo", b =>
                {
                    b.Navigation("RoleMenus");
                });
#pragma warning restore 612, 618
        }
    }
}
