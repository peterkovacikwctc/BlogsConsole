using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {            
            logger.Info("Program started");

            try
            {
                Display display = new Display();
                string menuChoice;
                var db = new BloggingContext();

                do {
                    // display menu
                    
                    display.menu();
                    menuChoice = Console.ReadLine();
                    Console.WriteLine("");
                    
                    switch(menuChoice) 
                    {
                        case "1":
                            // Display all Blogs from the database
                            var query = db.Blogs.OrderBy(b => b.Name);

                            Console.WriteLine("All blogs in the database:");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }
                            break;

                        case "2":
                            // Create and save a new Blog
                            Console.Write("Enter a name for a new Blog: ");
                            var name = Console.ReadLine();

                            var blog = new Blog { Name = name };

                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                            break;

                        case "3":
                            // code block
                            break;

                        case "4":
                            // code block
                            break;

                        case "q":
                            display.quit();
                            break;

                        default:
                            display.defaultMessage(menuChoice);
                            break;
                    }
                } while(menuChoice != "q");
            }
            
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
