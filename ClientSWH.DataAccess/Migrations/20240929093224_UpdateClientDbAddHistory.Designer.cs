﻿// <auto-generated />
using System;
using ClientSWH.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClientSWH.DataAccess.Migrations
{
    [DbContext(typeof(ClientSWHDbContext))]
    [Migration("20240929093224_UpdateClientDbAddHistory")]
    partial class UpdateClientDbAddHistory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.DocumentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("did");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_date")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("DocDate")
                        .HasMaxLength(5)
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("docdate");

                    b.Property<Guid>("DocId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("docid");

                    b.Property<string>("IdSha256")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("idsha256");

                    b.Property<string>("Idmd5")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("idmd5");

                    b.Property<string>("ModeCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("modecode");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modify_date");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("number");

                    b.Property<long>("Pid")
                        .HasColumnType("bigint")
                        .HasColumnName("pid");

                    b.Property<int>("SizeDoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("size_doc");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SizeDoc"));

                    b.HasKey("Id");

                    b.HasIndex("Pid");

                    b.ToTable("documents", (string)null);
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.HistoryPkgEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_date")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("Newst")
                        .HasColumnType("integer")
                        .HasColumnName("newst");

                    b.Property<int>("Oldst")
                        .HasColumnType("integer")
                        .HasColumnName("oldst");

                    b.Property<int>("Pid")
                        .HasColumnType("integer");

                    b.Property<int?>("StatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("history_pkg", (string)null);
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.PackageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("pid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_date")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modify_date");

                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("packages", (string)null);
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.StatusEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("MkRes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("mkres");

                    b.Property<bool>("RunWf")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("runwf");

                    b.Property<bool>("SendMess")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("sendmess");

                    b.Property<string>("StatusName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("stname");

                    b.HasKey("Id");

                    b.ToTable("pkg_status", (string)null);
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Hidden")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.DocumentEntity", b =>
                {
                    b.HasOne("ClientSWH.DataAccess.Entities.PackageEntity", "Package")
                        .WithMany("Documents")
                        .HasForeignKey("Pid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Package");
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.HistoryPkgEntity", b =>
                {
                    b.HasOne("ClientSWH.DataAccess.Entities.StatusEntity", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.PackageEntity", b =>
                {
                    b.HasOne("ClientSWH.DataAccess.Entities.StatusEntity", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientSWH.DataAccess.Entities.UserEntity", "User")
                        .WithMany("Packages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.PackageEntity", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("ClientSWH.DataAccess.Entities.UserEntity", b =>
                {
                    b.Navigation("Packages");
                });
#pragma warning restore 612, 618
        }
    }
}
