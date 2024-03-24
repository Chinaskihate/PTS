﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PTS.Persistence.DbContexts;

#nullable disable

namespace PTS.Persistence.Migrations.TestTables
{
    [DbContext(typeof(TestDbContext))]
    partial class TestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PTS.Persistence.Models.Results.TaskResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("TaskVersionId")
                        .HasColumnType("integer");

                    b.Property<int>("TestResultId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestResultId");

                    b.ToTable("TaskResults");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Results.TestResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("SubmissionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TestId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Tests.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Tests.Versions.TestTaskVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TaskId")
                        .HasColumnType("integer");

                    b.Property<int>("TaskVersionId")
                        .HasColumnType("integer");

                    b.Property<int>("TestId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestTaskVersions");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Results.TaskResult", b =>
                {
                    b.HasOne("PTS.Persistence.Models.Results.TestResult", "TestResult")
                        .WithMany("TaskResults")
                        .HasForeignKey("TestResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestResult");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Results.TestResult", b =>
                {
                    b.HasOne("PTS.Persistence.Models.Tests.Test", "Test")
                        .WithMany("TestResults")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Tests.Versions.TestTaskVersion", b =>
                {
                    b.HasOne("PTS.Persistence.Models.Tests.Test", "Test")
                        .WithMany("TestTaskVersions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Results.TestResult", b =>
                {
                    b.Navigation("TaskResults");
                });

            modelBuilder.Entity("PTS.Persistence.Models.Tests.Test", b =>
                {
                    b.Navigation("TestResults");

                    b.Navigation("TestTaskVersions");
                });
#pragma warning restore 612, 618
        }
    }
}
