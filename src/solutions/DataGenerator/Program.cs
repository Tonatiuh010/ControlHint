using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using Engine.BO.AccessControl;
using Engine.BL.Actuators;
using Engine.DAL;
using Engine.BL;
using System.IO;
using Engine.BO;
using Engine.Constants;
using Base;
using Microsoft.Extensions.Configuration;
public class Program
{
    public static Random _random = new Random();
    public static int RandomMinutes => RangeGenerator(-5, 30);
    // Variable Estatica (Lista de empleados)	
    // Generar Minutos random de entrada y salida

    public static List<Employee> employees = new();
    public static EmployeeBL employeeBL = new EmployeeBL();

    public static List<Check> checks = new();
    public static CheckBL checkBL = new CheckBL();

    public static void Main()
    {
        StartUp();
        DateTime _ref = DateTime.Today;
        DateTime firstDay = new DateTime(_ref.Year, 1, 1);

        int elapsedDays = (_ref - firstDay).Days;

        employees = employeeBL.GetEmployees();

        for (int day = 0; day <= elapsedDays; day++)
        {
            foreach (Employee employee in employees)
            {
                try
                {
                    DateTime tempDt = firstDay.AddDays(day);
                    Console.WriteLine("Day #" + tempDt.ToString("d"));

                    var inTime = employee.Shift?.InTime?.Add(new TimeSpan(0, RandomMinutes, 0));
                    var outTime = employee.Shift?.OutTime?.Add(new TimeSpan(0, RandomMinutes, 0));
                    var checkInDt = GetCheck(tempDt, (TimeSpan)inTime);
                    var checkOutDt = GetCheck(tempDt, (TimeSpan)outTime);
                    var check = new Check() {
                        CheckDt = checkInDt,
                        Employee = employee,
                        CheckType = "IN",
                        Device = new Engine.BO.FlowControl.Device()
                        {
                            Id = 1
                        }
                    };
                    var checkOut = new Check()
                    {
                        CheckDt = checkOutDt,
                        Employee = employee,
                        CheckType = "OUT",
                        Device = new Engine.BO.FlowControl.Device()
                        {
                            Id = 1
                        }
                    };

                    checkBL.SetCheckEmployee(check);
                    checkBL.SetCheckEmployee(checkOut);
                    Console.WriteLine($"in Date: {employee.Name} {checkInDt.ToString("G")}");
                    Console.WriteLine($"out Date: {employee.Name} {checkOutDt.ToString("G")}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }  
    }
    public static DateTime GetCheck( DateTime baseDate, TimeSpan time) => baseDate.AddTicks(time.Ticks);
    public static int RangeGenerator(int min, int max) => _random.Next(min, max);

    private static void StartUp()
    {
        var B = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

        Builder.Build(
            new List<string>()
            {
                C.ACCESS_DB,
                C.HINT_DB,
                C.DOCS_DB
            },
            B.Build()
            );
    }
}