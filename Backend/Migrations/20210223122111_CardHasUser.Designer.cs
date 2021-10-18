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
    [Migration("20210223122111_CardHasUser")]
    partial class CardHasUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("CheckIn.Shared.Models.Card", b =>
                {
                    b.Property<string>("CardID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("HasUser")
                        .HasColumnType("bit");

                    b.Property<int?>("RoomID")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TimeAdded")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("CardID");

                    b.HasIndex("RoomID");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.CheckTime", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("CardID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ScannerMacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("CardID");

                    b.HasIndex("ScannerMacAddress");

                    b.ToTable("CheckTimes");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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
                    b.HasOne("CheckIn.Shared.Models.Room", null)
                        .WithMany("Cards")
                        .HasForeignKey("RoomID");
                });

            modelBuilder.Entity("CheckIn.Shared.Models.CheckTime", b =>
                {
                    b.HasOne("CheckIn.Shared.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardID");

                    b.HasOne("CheckIn.Shared.Models.Scanner", "Scanner")
                        .WithMany()
                        .HasForeignKey("ScannerMacAddress");

                    b.Navigation("Card");

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
