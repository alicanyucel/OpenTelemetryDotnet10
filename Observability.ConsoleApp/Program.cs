using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Observability.ConsoleApp;

internal class Program
{
   private  static void Main(string[] args)
    {
        // OpenTelemetry'yi yapılandırmak için Sdk.CreateTracerProviderBuilder() yöntemini kullanarak bir TracerProvider oluşturabiliriz. Bu, uygulamanın izleme verilerini toplamak ve göndermek için gerekli altyapıyı sağlar. Örneğin, aşağıdaki gibi bir kod ekleyebiliriz:
        using TracerProvider traceProvider = Sdk.CreateTracerProviderBuilder().AddSource("ConsoleApp.Trace")
            .ConfigureResource(configure =>
            {
                configure.AddService("MyConsoleService");
                configure.AddAttributes(new List<KeyValuePair<string, object>>()
                {
                    new("test.key","test value"),
                    new("machine.name",Environment.MachineName),
                    new("process.path",Environment.ProcessPath)
                });
            })
            .AddConsoleExporter().AddOtlpExporter().
            Build();
        ServiceHelper serviceHelper = new();
        serviceHelper.Method1();
        Console.ReadLine();
    }
}
// neyi trace edecekiz? Trace etmek istediğimiz işlemi belirlemek için ActivitySource kullanabiliriz. Örneğin, bir işlem başlatmak ve bitirmek için aşağıdaki gibi bir kod ekleyebiliriz:
internal static class ActivitySourceProvider
{
    // manuel takip etmek istediğimiz işlemi tanımlamak için bir ActivitySource oluşturuyoruz. Bu, izleme verilerini toplamak ve göndermek için kullanılacak olan kaynak adını belirtir. Örneğin, "Observability.ConsoleApp" gibi bir ad kullanabiliriz:
    public static readonly ActivitySource ActivitySource = new("ConsoleApp.Trace","1.0.0");
}
internal class ServiceHelper
{
    public void Method1()
    {
        // ActivitySource kullanarak bir işlem başlatmak için StartActivity() yöntemini kullanabiliriz. Bu, işlemi başlatır ve izleme verilerini toplamaya başlar. İşlem tamamlandığında, using bloğunun sonunda otomatik olarak sona erer ve izleme verileri gönderilir. Örneğin, aşağıdaki gibi bir kod ekleyebiliriz:
        using var activity = ActivitySourceProvider.ActivitySource.StartActivity()!;
        activity.ActivityTraceFlags=ActivityTraceFlags.Recorded;
        activity.AddTag("user.id", "1");
        activity.SetTag("user.id", "2");
        Console.WriteLine("process 1");
        Console.WriteLine("Process 2");
        Console.WriteLine("process 3");
        Method2();


    }
    public void Method2()
    { 
        using var activity=ActivitySourceProvider.ActivitySource.StartActivity();
        {
            Console.WriteLine("process4");
        }

    }
}