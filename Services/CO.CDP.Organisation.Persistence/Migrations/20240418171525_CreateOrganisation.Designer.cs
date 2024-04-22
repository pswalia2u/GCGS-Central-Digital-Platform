﻿// <auto-generated />
using System;
using System.Collections.Generic;
using CO.CDP.Organisation.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CO.CDP.Organisation.Persistence.Migrations
{
    [DbContext(typeof(OrganisationContext))]
    [Migration("20240418171525_CreateOrganisation")]
    partial class CreateOrganisation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CO.CDP.Organisation.Persistence.Organisation", b =>
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

                    b.Property<List<int>>("Roles")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "CO.CDP.Organisation.Persistence.Organisation.Address#OrganisationAddress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("CountryName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Locality")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("StreetAddress")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ContactPoint", "CO.CDP.Organisation.Persistence.Organisation.ContactPoint#OrganisationContactPoint", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("FaxNumber")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Telephone")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasColumnType("text");
                        });

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("CO.CDP.Organisation.Persistence.Organisation", b =>
                {
                    b.OwnsMany("CO.CDP.Organisation.Persistence.Organisation+OrganisationIdentifier", "AdditionalIdentifiers", b1 =>
                        {
                            b1.Property<int>("OrganisationId")
                                .HasColumnType("integer");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("LegalName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Scheme")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Uri")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OrganisationId", "Id");

                            b1.ToTable("Organisations_AdditionalIdentifiers");

                            b1.WithOwner()
                                .HasForeignKey("OrganisationId");
                        });

                    b.OwnsOne("CO.CDP.Organisation.Persistence.Organisation+OrganisationIdentifier", "Identifier", b1 =>
                        {
                            b1.Property<int>("OrganisationId")
                                .HasColumnType("integer");

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("LegalName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Scheme")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Uri")
                                .IsRequired()
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
