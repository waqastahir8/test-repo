﻿// <auto-generated />
using System;
using AmeriCorps.Users.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmeriCorps.Users.Data.Migrations.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20240507231034_Collection")]
    partial class Collection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("users")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsForeign")
                        .HasColumnType("boolean");

                    b.Property<bool>("MovingWithinSixMonths")
                        .HasColumnType("boolean");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street1")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street2")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("address", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("attribute", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ListingId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("collection", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.CommunicationMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsPreferred")
                        .HasColumnType("boolean");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("communication_method", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Education", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("DateAttendedFrom")
                        .HasColumnType("date");

                    b.Property<DateOnly>("DateAttendedTo")
                        .HasColumnType("date");

                    b.Property<bool>("DegreeCompleted")
                        .HasColumnType("boolean");

                    b.Property<string>("DegreeTypePursued")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MajorAreaOfStudy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("education", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("boolean");

                    b.Property<string>("PickListId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SpeakingAbility")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("WritingAbility")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("language", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.MilitaryService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("PickListId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("military_service", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Reference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("CanContact")
                        .HasColumnType("boolean");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Contacted")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("DateContacted")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RelationshipLength")
                        .HasColumnType("integer");

                    b.Property<string>("TypeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("reference", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Relative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnnualIncome")
                        .HasColumnType("integer");

                    b.Property<string>("HighestEducationLevel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("relative", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.SavedSearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Filters")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastRun")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("NotificationsOn")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("saved_search", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("PickListId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("skill", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("ExternalAccountId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PreferredName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<bool>("Searchable")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("user", "users");
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Address", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Attribute", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Attributes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Collection", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Collection")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.CommunicationMethod", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("CommunicationMethods")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Education", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Education")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Language", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Languages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.MilitaryService", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("MilitaryService")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Reference", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("References")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Relative", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Relatives")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.SavedSearch", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("SavedSearches")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.Skill", b =>
                {
                    b.HasOne("AmeriCorps.Users.Data.Core.User", null)
                        .WithMany("Skills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmeriCorps.Users.Data.Core.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Attributes");

                    b.Navigation("Collection");

                    b.Navigation("CommunicationMethods");

                    b.Navigation("Education");

                    b.Navigation("Languages");

                    b.Navigation("MilitaryService");

                    b.Navigation("References");

                    b.Navigation("Relatives");

                    b.Navigation("SavedSearches");

                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
