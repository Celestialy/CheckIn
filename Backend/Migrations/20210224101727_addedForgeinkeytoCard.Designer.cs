﻿// <auto-generated />
using System;
using Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210224101727_addedForgeinkeytoCard")]
    partial class addedForgeinkeytoCard
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CheckIn.Shared.Models.Card", b =>
                {
                    b.Property<string>("_card")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("_card");

                    b.HasIndex("RoomId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.CheckTime", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScannerMacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("ScannerMacAddress");

                    b.ToTable("CheckTimes");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Added")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScannerMacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ScannerMacAddress");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Scanner", b =>
                {
                    b.Property<string>("MacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Added")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MacAddress");

                    b.ToTable("Scanners");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Card", b =>
                {
                    b.HasOne("CheckIn.Shared.Models.Room", "Room")
                        .WithMany("Cards")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.CheckTime", b =>
                {
                    b.HasOne("CheckIn.Shared.Models.Scanner", "Scanner")
                        .WithMany()
                        .HasForeignKey("ScannerMacAddress");

                    b.Navigation("Scanner");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Room", b =>
                {
                    b.HasOne("CheckIn.Shared.Models.Scanner", "Scanner")
                        .WithMany()
                        .HasForeignKey("ScannerMacAddress");

                    b.Navigation("Scanner");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Room", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
