using Microsoft.Web.VersionManager;
using Microsoft.WebMatrix.Core;
using Microsoft.WebMatrix.Core.Server;
using Microsoft.WebMatrix.ProcessModel;
using Microsoft.WebMatrix.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        static IisHelperService IisExpressHelperService { get; } = (IisHelperService)Activator.CreateInstance(typeof(IisHelperService).Assembly.GetType("Microsoft.WebMatrix.Core.IisExpressHelperServiceImplementation"), new object[] { false });

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
            var site = GetSite(name);

            if (site == null)
            {
                Console.WriteLine($"Site with name {name} not found");
                return;
            }

            StartSite(site);
        }

        static void StartSite(int id)
        {
            var site = GetSite(id);

            if (site == null)
            {
                Console.WriteLine($"Site with id #{id} not found");
                return;
            }

            StartSite(site);
        }

        static void StartSite(Microsoft.Web.Administration.Site site)
        {
            var iisExpressSite = GetIisExpressSite(site);

            iisExpressSite.Start();

            var binding = site.Bindings.First();

            var url = BindingUtility.GetClickableUrlLinkFromBinding("localhost", binding.Protocol, binding.BindingInformation);

            ProcessHelper.TryOpenWithWindows(url);
        }

        static Site GetIisExpressSite(Microsoft.Web.Administration.Site site)
        {
            return IisExpressHelperService.SiteManager.GetSite(site.Name);
        }

        static Microsoft.Web.Administration.Site GetSite(string name)
        {
            return ManagementUnit.ReadOnlyServerManager.Sites.SingleOrDefault(s => s.Name == name);
        }

        static Microsoft.Web.Administration.Site GetSite(int id)
        {
            return ManagementUnit.ReadOnlyServerManager.Sites.SingleOrDefault(s => s.Id == id);
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
