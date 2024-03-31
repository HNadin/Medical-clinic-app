﻿// <auto-generated />
using System;
using Medical_clinic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Medical_clinic.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231022163932_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Medical_clinic.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CertificationTraining")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WorkScheduleData")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Medical_clinic.Models.Nurse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CertificationTraining")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WorkScheduleData")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Nurses");
                });

            modelBuilder.Entity("Medical_clinic.Models.ProvideService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int?>("NurseId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("NurseId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ProvideServices");
                });

            modelBuilder.Entity("Medical_clinic.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Medical_clinic.Models.Doctor", b =>
                {
                    b.HasOne("Medical_clinic.Models.Service", null)
                        .WithMany("Doctors")
                        .HasForeignKey("ServiceId");
                });

            modelBuilder.Entity("Medical_clinic.Models.Nurse", b =>
                {
                    b.HasOne("Medical_clinic.Models.Service", null)
                        .WithMany("Nurses")
                        .HasForeignKey("ServiceId");
                });

            modelBuilder.Entity("Medical_clinic.Models.ProvideService", b =>
                {
                    b.HasOne("Medical_clinic.Models.Doctor", "Doctor")
                        .WithMany("ProvideServices")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Medical_clinic.Models.Nurse", "Nurse")
                        .WithMany("ProvideServices")
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Medical_clinic.Models.Service", "Service")
                        .WithMany("ProvideServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Nurse");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Medical_clinic.Models.Doctor", b =>
                {
                    b.Navigation("ProvideServices");
                });

            modelBuilder.Entity("Medical_clinic.Models.Nurse", b =>
                {
                    b.Navigation("ProvideServices");
                });

            modelBuilder.Entity("Medical_clinic.Models.Service", b =>
                {
                    b.Navigation("Doctors");

                    b.Navigation("Nurses");

                    b.Navigation("ProvideServices");
                });
#pragma warning restore 612, 618
        }
    }
}
