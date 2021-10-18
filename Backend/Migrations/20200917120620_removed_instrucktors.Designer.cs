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
    [Migration("20200917120620_removed_instrucktors")]
    partial class removed_instrucktors
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Models.Card", b =>
                {
                    b.Property<string>("CardID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Oid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.HasKey("CardID");

                    b.HasIndex("RoomID");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Backend.Models.CheckTime", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ScannerMacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CardID");

                    b.HasIndex("ScannerMacAddress");

                    b.ToTable("CheckTimes");
                });

            modelBuilder.Entity("Backend.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Added")
                        .HasColumnType("datetime2");

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

            modelBuilder.Entity("Backend.Models.Scanner", b =>
                {
                    b.Property<string>("MacAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Added")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MacAddress");

                    b.ToTable("Scanners");
                });

            modelBuilder.Entity("Backend.Models.Card", b =>
                {
                    b.HasOne("Backend.Models.Room", null)
                        .WithMany("Cards")
                        .HasForeignKey("RoomID");
                });

            modelBuilder.Entity("Backend.Models.CheckTime", b =>
                {
                    b.HasOne("Backend.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardID");

                    b.HasOne("Backend.Models.Scanner", "Scanner")
                        .WithMany()
                        .HasForeignKey("ScannerMacAddress");
                });

            modelBuilder.Entity("Backend.Models.Room", b =>
                {
                    b.HasOne("Backend.Models.Scanner", "Scanner")
                        .WithMany()
                        .HasForeignKey("ScannerMacAddress");
                });
#pragma warning restore 612, 618
        }
    }
}
