﻿// <auto-generated />
using System;
using System.Collections.Generic;
using CO.CDP.OrganisationInformation.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CO.CDP.OrganisationInformation.Persistence.Migrations
{
    [DbContext(typeof(OrganisationInformationContext))]
    partial class OrganisationInformationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CO.CDP.OrganisationInformation.Persistence.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<int>>("Types")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "CO.CDP.OrganisationInformation.Persistence.Organisation.Address#OrganisationAddress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("AddressLine1")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("AddressLine2")
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .HasColumnType("text");

                            b1.Property<string>("PostCode")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ContactPoint", "CO.CDP.OrganisationInformation.Persistence.Organisation.ContactPoint#OrganisationContactPoint", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .HasColumnType("text");

                            b1.Property<string>("Telephone")
                                .HasColumnType("text");

                            b1.Property<string>("Url")
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("CO.CDP.OrganisationInformation.Persistence.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("CO.CDP.OrganisationInformation.Persistence.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ComplexProperty<Dictionary<string, object>>("ContactInfo", "CO.CDP.OrganisationInformation.Persistence.Tenant.ContactInfo#TenantContactInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Phone")
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("CO.CDP.OrganisationInformation.Persistence.Organisation", b =>
                {
                    b.OwnsMany("CO.CDP.OrganisationInformation.Persistence.Organisation+OrganisationIdentifier", "AdditionalIdentifiers", b1 =>
                        {
                            b1.Property<int>("OrganisationId")
                                .HasColumnType("integer");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("LegalName")
                                .HasColumnType("text");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Scheme")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Uri")
                                .HasColumnType("text");

                            b1.HasKey("OrganisationId", "Id");

                            b1.ToTable("Organisations_AdditionalIdentifiers");

                            b1.WithOwner()
                                .HasForeignKey("OrganisationId");
                        });

                    b.OwnsOne("CO.CDP.OrganisationInformation.Persistence.Organisation+OrganisationIdentifier", "Identifier", b1 =>
                        {
                            b1.Property<int>("OrganisationId")
                                .HasColumnType("integer");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("LegalName")
                                .HasColumnType("text");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Scheme")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Uri")
                                .HasColumnType("text");

                            b1.HasKey("OrganisationId");

                            b1.ToTable("Organisations");

                            b1.WithOwner()
                                .HasForeignKey("OrganisationId");
                        });

                    b.Navigation("AdditionalIdentifiers");

                    b.Navigation("Identifier")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
