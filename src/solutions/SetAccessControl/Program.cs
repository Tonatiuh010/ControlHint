using Engine.DAL;
using CsvHelper;
using System;
using System.Globalization;
using Engine.Constants;
using Engine.BO;
using Engine.BL;
using DataSet.Map;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Engine.Services;
using Engine.BO.AccessControl;
using Engine.BL.Actuators;

namespace DataSet {
    public static class Program
    {
        public static string Path { get; set; } = string.Empty;
        public static List<string> Files { get; set; } = new();
        public static ExceptionManager ExceptionManager { get; set; } = new ExceptionManager(ShowError);

        public static void Main(string[] args)
        {
            SetConfiguration();
            
            CsvProcess departament = new(C.DepartamentsCsv, csv => {
                csv.Context.RegisterClassMap<DepartamentMap>();
                var x =  csv.GetRecords<Department>();
                foreach(var d in x.ToList())
                {
                    var bl = new DepartmentBL();
                    bl.SetDepartament(d, C.PROCESS_USER);
                }
            });

            CsvProcess job = new(C.JobsCsv, csv => {
                csv.Context.RegisterClassMap<JobMap>();
                var x = csv.GetRecords<Job>();
                foreach (var j in x.ToList())
                {
                    var bl = new JobBL();
                    bl.SetJob(j, C.PROCESS_USER);
                }
            });

            CsvProcess access = new(C.AccessCsv, csv => {
                csv.Context.RegisterClassMap<AccessLevelMap>();
                var x = csv.GetRecords<AccessLevel>();
                    
                foreach (var l in x.ToList())
                {
                    var bl = new AccessLevelBL();
                    bl.SetAccessLevel(l, C.PROCESS_USER);
                }
            });

            CsvProcess position = new(C.PositionsCsv, csv => {
                csv.Context.RegisterClassMap<PositionMap>();
                var x = csv.GetRecords<Position>();

                foreach(var p in x.ToList())
                {
                    var bl = new PositionBL();
                    bl.SetPosition(p, C.GLOBAL_USER);
                }
            });

            CsvProcess employee = new(C.EmployeeCsv, csv => {
                csv.Context.RegisterClassMap<EmployeeMap>();
                var x = csv.GetRecords<Employee>();

                foreach (var e in x.ToList()) {
                    var bl = new EmployeeBL();
                    e.Image.Bytes = Utils.GetImage(e.Image.Url);                    
                    bl.SetEmployee(e, C.PROCESS_USER);
                }
            });
            
        }

        public static void SetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            var b = builder.Build();

            var sectionSets = b.GetSection("DataSet");

            Path = sectionSets.GetSection("Path").Value;
            var files = sectionSets.GetSection("Files").AsEnumerable();

            ConnectionString.SetConnectionString(() => b.GetConnectionString(C.ACCESS_DB), C.ACCESS_DB);
            BinderBL.Start();
            BinderBL.SetDalError(ShowError);
        }

        public static void ShowError(Exception ex, string msg)
        {
            var now = DateTime.Now;
            Console.WriteLine($"[{now:g}]: {msg} - {ex.Message}");
        }

    }

    public class CsvProcess
    {
        public delegate void CsvAction(CsvReader csv);

        public string? FileName { get; set; }
        public CsvAction OnRead { get; set; }
        public CsvProcess(string fileName, CsvAction onRead)
        {
            using var stream = new StreamReader($"{Program.Path}{fileName}");
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
            OnRead = onRead;
            OnRead(csv);
        }

    }
}