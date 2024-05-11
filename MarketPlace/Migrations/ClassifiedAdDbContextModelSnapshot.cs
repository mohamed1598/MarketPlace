﻿// <auto-generated />
using System;
using MarketPlace.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarketPlace.Migrations
{
    [DbContext(typeof(ClassifiedAdDbContext))]
    partial class ClassifiedAdDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Marketplace.Domain.ClassifiedAd", b =>
                {
                    b.Property<Guid>("ClassifiedAdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("State")
                        .HasColumnType("int");

                    b.HasKey("ClassifiedAdId");

                    b.ToTable("ClassifiedAds", t =>
                        {
                            t.Property("ClassifiedAdId")
                                .HasColumnName("ClassifiedAdId1");
                        });
                });

            modelBuilder.Entity("Marketplace.Domain.Picture", b =>
                {
                    b.Property<Guid>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassifiedAdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("PictureId");

                    b.HasIndex("ClassifiedAdId");

                    b.ToTable("Picture", t =>
                        {
                            t.Property("PictureId")
                                .HasColumnName("PictureId1");
                        });
                });

            modelBuilder.Entity("Marketplace.Domain.ClassifiedAd", b =>
                {
                    b.OwnsOne("Marketplace.Domain.UserId", "ApprovedBy", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");
                        });

                    b.OwnsOne("Marketplace.Domain.ClassifiedAdId", "Id", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ClassifiedAdId");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");
                        });

                    b.OwnsOne("Marketplace.Domain.UserId", "OwnerId", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");
                        });

                    b.OwnsOne("Marketplace.Domain.ClassifiedAdText", "Text", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");
                        });

                    b.OwnsOne("Marketplace.Domain.ClassifiedAdTitle", "Title", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");
                        });

                    b.OwnsOne("Marketplace.Domain.Price", "Price", b1 =>
                        {
                            b1.Property<Guid>("ClassifiedAdId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("ClassifiedAdId");

                            b1.ToTable("ClassifiedAds");

                            b1.WithOwner()
                                .HasForeignKey("ClassifiedAdId");

                            b1.OwnsOne("Marketplace.Domain.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("PriceClassifiedAdId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("CurrencyCode")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<int>("DecimalPlaces")
                                        .HasColumnType("int");

                                    b2.Property<bool>("InUse")
                                        .HasColumnType("bit");

                                    b2.HasKey("PriceClassifiedAdId");

                                    b2.ToTable("ClassifiedAds");

                                    b2.WithOwner()
                                        .HasForeignKey("PriceClassifiedAdId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.Navigation("ApprovedBy");

                    b.Navigation("Id")
                        .IsRequired();

                    b.Navigation("OwnerId")
                        .IsRequired();

                    b.Navigation("Price");

                    b.Navigation("Text");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("Marketplace.Domain.Picture", b =>
                {
                    b.HasOne("Marketplace.Domain.ClassifiedAd", null)
                        .WithMany("Pictures")
                        .HasForeignKey("ClassifiedAdId");

                    b.OwnsOne("Marketplace.Domain.ClassifiedAdId", "ParentId", b1 =>
                        {
                            b1.Property<Guid>("PictureId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("PictureId");

                            b1.ToTable("Picture");

                            b1.WithOwner()
                                .HasForeignKey("PictureId");
                        });

                    b.OwnsOne("Marketplace.Domain.PictureId", "Id", b1 =>
                        {
                            b1.Property<Guid>("PictureId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("PictureId");

                            b1.HasKey("PictureId");

                            b1.ToTable("Picture");

                            b1.WithOwner()
                                .HasForeignKey("PictureId");
                        });

                    b.OwnsOne("Marketplace.Domain.PictureSize", "Size", b1 =>
                        {
                            b1.Property<Guid>("PictureId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Height")
                                .HasColumnType("int");

                            b1.Property<int>("Width")
                                .HasColumnType("int");

                            b1.HasKey("PictureId");

                            b1.ToTable("Picture");

                            b1.WithOwner()
                                .HasForeignKey("PictureId");
                        });

                    b.Navigation("Id")
                        .IsRequired();

                    b.Navigation("ParentId")
                        .IsRequired();

                    b.Navigation("Size")
                        .IsRequired();
                });

            modelBuilder.Entity("Marketplace.Domain.ClassifiedAd", b =>
                {
                    b.Navigation("Pictures");
                });
#pragma warning restore 612, 618
        }
    }
}
