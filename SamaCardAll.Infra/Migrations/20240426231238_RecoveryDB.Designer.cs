﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SamaCardAll.Infra;

#nullable disable

namespace SamaCardAll.Infra.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240426231238_RecoveryDB")]
    partial class RecoveryDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("SamaCardAll.Infra.Models.Card", b =>
                {
                    b.Property<int>("IdCard")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<short>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bank")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Expiration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdCard");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.Customer", b =>
                {
                    b.Property<int>("IdCustomer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<short>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomerName")
                        .HasColumnType("TEXT");

                    b.HasKey("IdCustomer");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.Installments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<short>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Installment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InstallmentValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("MonthYear")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SpendIdSpend")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SpendIdSpend");

                    b.ToTable("Installments");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.Spend", b =>
                {
                    b.Property<int>("IdSpend")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int>("CardIdCard")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("CustomerIdCustomer")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<short>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Expenses")
                        .HasColumnType("TEXT");

                    b.Property<int>("InstallmentPlan")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InstallmentValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserIdUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdSpend");

                    b.HasIndex("CardIdCard");

                    b.HasIndex("CustomerIdCustomer");

                    b.HasIndex("UserIdUser");

                    b.ToTable("Spends");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<short>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("IdUser");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.Installments", b =>
                {
                    b.HasOne("SamaCardAll.Infra.Models.Spend", "Spend")
                        .WithMany()
                        .HasForeignKey("SpendIdSpend")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Spend");
                });

            modelBuilder.Entity("SamaCardAll.Infra.Models.Spend", b =>
                {
                    b.HasOne("SamaCardAll.Infra.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardIdCard")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamaCardAll.Infra.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerIdCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamaCardAll.Infra.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserIdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Customer");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
