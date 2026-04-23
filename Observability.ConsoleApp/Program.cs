using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Observability.ConsoleApp;

internal class Program
{
   private  static void Main(string[] args)
    {
        // OpenTelemetry'yi yapılandırmak için Sdk.CreateTracerProviderBuilder() yöntemini kullanarak bir TracerProvider oluşturabiliriz. Bu, uygulamanın izleme verilerini toplamak ve göndermek için gerekli altyapıyı sağlar. Örneğin, aşağıdaki gibi bir kod ekleyebiliriz:
        using TracerProvider traceProvider = Sdk.CreateTracerProviderBuilder().AddSource("ConsoleApp.Trace").AddConsoleExporter().Build();
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
        using (var activity = ActivitySourceProvider.ActivitySource.StartActivity())
        {
            Console.WriteLine("process1");
            Console.WriteLine("process2");

        };
        Console.WriteLine("process 3");


    }
}