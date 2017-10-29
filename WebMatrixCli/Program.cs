using Microsoft.Web.VersionManager;
using Microsoft.WebMatrix.Core;
using Microsoft.WebMatrix.Core.Server;
using Microsoft.WebMatrix.ProcessModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebMatrixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "list")
            {
                ListSites();
                return;
            }

            if (args.Length == 2 && args[0] == "start")
            {
                int id;

                if(int.TryParse(args[1], out id))
                {
                    StartSite(id);
                    return;
                }

                StartSite(args[1]);
                return;
            }

            PrintUsage();
        }

        static void StartSite(string name)
        {
            var site = ManagementUnit.ReadOnlyServerManager.Sites.SingleOrDefault(s => s.Name == name);

            if (site == null)
            {
                Console.WriteLine($"Site with name {name} not found");
                return;
            }

            site.Start();
        }

        static void StartSite(int id)
        {
            var site = ManagementUnit.ReadOnlyServerManager.Sites.SingleOrDefault(s => s.Id == id);

            if (site == null)
            {
                Console.WriteLine($"Site with id #{id} not found");
                return;
            }

            site.Start();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  ");
            Console.WriteLine("  List sites:");
            Console.WriteLine("    ");
            Console.WriteLine("    wm list");
            Console.WriteLine("  ");
            Console.WriteLine("  Start site:");
            Console.WriteLine("    ");
            Console.WriteLine("    wm start [id-or-name]");
            Console.WriteLine("    ");
            Console.WriteLine("    Example:");
            Console.WriteLine("      wm start 4");
            Console.WriteLine("      wm start MySite");
            Console.WriteLine("      wm start \"Site with spaces\"");
            Console.WriteLine();

            ListSites();
        }

        static void ListSites()
        {
            Console.WriteLine("Sites:");
            Console.WriteLine("");

            foreach (var site in ManagementUnit.ReadOnlyServerManager.Sites.OrderBy(s => s.Id > 10000 ? -s.Id : s.Id))
            {
                Console.WriteLine($"  #{site.Id} \t{site.Name}");
            }
        }
    }
}
