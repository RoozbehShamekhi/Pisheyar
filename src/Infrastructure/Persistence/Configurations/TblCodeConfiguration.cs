﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pisheyar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pisheyar.Infrastructure.Persistence.Configurations
{
    public class TblCodeConfiguration : IEntityTypeConfiguration<TblCode>
    {
        public void Configure(EntityTypeBuilder<TblCode> entity)
        {
            entity.HasKey(e => e.CodeId);

            entity.ToTable("Tbl_Code");

            entity.Property(e => e.CodeId)
                .HasColumnName("Code_ID");

            entity.Property(e => e.CodeCgid)
                .HasColumnName("Code_CGID");

            entity.Property(e => e.CodeDisplay)
                .IsRequired()
                .HasColumnName("Code_Display")
                .HasMaxLength(128);

            entity.Property(e => e.CodeGuid)
                .HasColumnName("Code_Guid")
                .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.CodeIsActive)
                .IsRequired()
                .HasColumnName("Code_IsActive")
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.CodeName)
                .IsRequired()
                .HasColumnName("Code_Name")
                .HasMaxLength(128);
        }
    }
}