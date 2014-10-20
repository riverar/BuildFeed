using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NServiceKit.DataAnnotations;
using NServiceKit.DesignPatterns.Model;
using NServiceKit.Redis;

using Required = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace BuildFeed.Models
{
    [DataObject]
    public class Build : IHasId<long>
    {
        [Key]
        [AutoIncrement]
        [Index]
        public long Id { get; set; }

        [@Required]
        [DisplayName("Major Version")]
        public byte MajorVersion { get; set; }

        [@Required]
        [DisplayName("Minor Version")]
        public byte MinorVersion { get; set; }

        [@Required]
        [DisplayName("Build Number")]
        public ushort Number { get; set; }

        [DisplayName("Build Revision")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public ushort? Revision { get; set; }

        [DisplayName("Lab String")]
        public string Lab { get; set; }

        [DisplayName("Build Time")]
        [DisplayFormat(ConvertEmptyStringToNull = true, ApplyFormatInEditMode = true, DataFormatString = "{0:yyMMdd-HHmm}")]
        public DateTime? BuildTime { get; set; }


        [@Required]
        [DisplayName("Time Added")]
        public DateTime Added { get; set; }

        [@Required]
        [DisplayName("Source Type")]
        [EnumDataType(typeof(TypeOfSource))]
        public TypeOfSource SourceType { get; set; }

        [DisplayName("Source Details")]
        public string SourceDetails { get; set; }


        [RegularExpression("http://betawiki\\.net/.+")]
        [DisplayName("BetaWiki (Client)")]
        public Uri BetaWikiUri { get; set; }

        [RegularExpression("http://betawiki\\.net/.+")]
        [DisplayName("BetaWiki (Server)")]
        public Uri BetaWikiServerUri { get; set; }

        [RegularExpression("http://www\\.betaarchive\\.com/wiki/.+")]
        [DisplayName("BetaArchive Wiki")]
        public Uri BetaArchiveUri { get; set; }

        [RegularExpression("http://longhorn\\.ms/.+")]
        [DisplayName("Longhorn.ms")]
        public Uri LonghornMsUri { get; set; }

        [RegularExpression("https://winworldpc\\.com/.+")]
        [DisplayName("WinWorldPC Library")]
        public Uri WinWorldPCUri { get; set; }

        [DisplayName("Flight Level")]
        [EnumDataType(typeof(LevelOfFlight))]
        public LevelOfFlight FlightLevel { get; set; }

        public string FullBuildString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}.{1}.{2}", MajorVersion, MinorVersion, Number);

                if (Revision.HasValue)
                {
                    sb.AppendFormat(".{0}", Revision);
                }

                if (!string.IsNullOrWhiteSpace(Lab))
                {
                    sb.AppendFormat(".{0}", Lab);
                }

                if (BuildTime.HasValue)
                {
                    sb.AppendFormat(".{0:yyMMdd-HHmm}", BuildTime);
                }

                return sb.ToString();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<Build> Select()
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                return client.GetAll();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Build SelectById(long id)
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                return client.GetById(id);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<Build> SelectInBuildOrder()
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                return client.GetAll()
                    .OrderByDescending(b => b.BuildTime)
                    .ThenByDescending(b => b.MajorVersion)
                    .ThenByDescending(b => b.MinorVersion)
                    .ThenByDescending(b => b.Number)
                    .ThenByDescending(b => b.Revision);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<BuildVersion> SelectBuildVersions()
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                var results = client.GetAll()
                    .GroupBy(b => new BuildVersion() { Major = b.MajorVersion, Minor = b.MinorVersion })
                    .Select(b => new BuildVersion() { Major = b.First().MajorVersion, Minor = b.First().MinorVersion })
                    .OrderByDescending(y => y.Major)
                    .ThenByDescending(y => y.Minor);
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<int> SelectBuildYears()
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                var results = client.GetAll().Where(b => b.BuildTime.HasValue)
                    .GroupBy(b => b.BuildTime.Value.Year)
                    .Select(b => b.First().BuildTime.Value.Year)
                    .OrderByDescending(y => y);
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<string> SelectBuildLabs()
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                var results = client.GetAll()
                    .Where(b => !string.IsNullOrWhiteSpace(b.Lab))
                    .GroupBy(b => b.Lab.ToLower())
                    .Select(b => b.First().Lab.ToLower())
                    .OrderBy(s => s);
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public static void Insert(Build item)
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                item.Id = client.GetNextSequence();
                client.Store(item);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static void Update(Build item)
        {
            Build old = Build.SelectById(item.Id);
            item.Added = old.Added;

            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                client.Store(item);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public static void InsertAll(IEnumerable<Build> items)
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                client.StoreAll(items);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static void DeleteById(long id)
        {
            using (RedisClient rClient = new RedisClient(DatabaseConfig.Host, DatabaseConfig.Port, db: DatabaseConfig.Database))
            {
                var client = rClient.As<Build>();
                client.DeleteById(id);
            }
        }
    }

    public enum TypeOfSource
    {
        [Display(Name = "Public Release")]
        PublicRelease,

        [Display(Name = "Public Leak")]
        InternalLeak,

        [Display(Name = "Update (GDR)")]
        UpdateGDR,

        [Display(Name = "Update (LDR)")]
        UpdateLDR,

        [Display(Name = "App Package")]
        AppPackage,

        [Display(Name = "Build Tools")]
        BuildTools,

        [Display(Name = "Documentation")]
        Documentation,

        [Display(Name = "Logging")]
        Logging,

        [Display(Name = "Private Leak")]
        PrivateLeak
    }

    public enum LevelOfFlight
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    public struct BuildVersion
    {
        public byte Major { get; set; }
        public byte Minor { get; set; }
    }
}