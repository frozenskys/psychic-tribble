using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TribbleServer.Models;

namespace TribbleServer.Migrations
{
    [DbContext(typeof(TribbleContext))]
    partial class TribbleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("TribbleServer.Models.Tribble", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Colour");

                    b.Property<string>("Furryness");

                    b.Property<bool>("Hungry");

                    b.HasKey("Id");

                    b.ToTable("Tribbles");
                });
        }
    }
}
