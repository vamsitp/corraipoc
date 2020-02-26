namespace CorrAI.Core
{
    //using Microsoft.EntityFrameworkCore;
    //using Microsoft.Extensions.Configuration;
    //using Microsoft.Extensions.Logging;

    public partial class CorrAIDBContext
        //: DbContext
    {
        //public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder =>
        //{
        //    builder
        //    .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
        //    //.AddApplicationInsights(Utils.AppInsightsKey);
        //});

        //public CorrAIDBContext()
        //{
        //    /*
        //        CREATE TABLE [dbo].[WeatherForecast] (
        //            [ID]            INT             IDENTITY (1, 1) NOT NULL,
        //            [Summary]       NVARCHAR (MAX)  NOT NULL,
        //            [TemperatureC]  INT             NOT NULL,
        //            [TemperatureF]  INT             NULL,
        //            [Date]          DATETIME        NOT NULL,
        //            [TraceId]       NVARCHAR(MAX)       NULL
        //        );
        //     */
        //}

        //public CorrAIDBContext(IConfiguration configuration)
        //{
        //    this.Configuration = configuration;
        //}

        //public CorrAIDBContext(DbContextOptions<CorrAIDBContext> options, IConfiguration configuration)
        //    : base(options)
        //{
        //    this.Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        //public virtual DbSet<dynamic> WeatherForecast { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder
        //            .UseLoggerFactory(EFLoggerFactory)
        //            .UseSqlServer(Configuration.GetValue<string>("SqlConn"), options => options.EnableRetryOnFailure());
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<dynamic>(entity =>
        //    {
        //        entity.ToTable("WeatherForecast", "dbo");

        //        //entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();

        //        //entity.Property(e => e.Summary).IsRequired().HasMaxLength(25);

        //        //entity.Property(e => e.Date).HasColumnType("datetime");

        //        //entity.Property(e => e.TemperatureC).IsRequired().HasColumnType("int");

        //        //entity.Property(e => e.TemperatureF).HasColumnType("int");
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
