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
                    var query = db.Blogs.OrderBy(b => b.BlogId);
                    
                    switch(menuChoice) 
                    {
                        case "1":
                            // Display all Blogs from the database
                            int maxBlogNumber = db.Blogs.Max(b => b.BlogId);
                            
                            // number of blog (singular)/blogs (plural or zero) in database
                            display.headerBlogNames(maxBlogNumber);

                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.BlogId}) {item.Name}");
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
                            // prompt user to select blog 
                            Console.WriteLine("Select a blog to view its posts.");
                            
                            // if query is null, message: no Blogs, then break switch

                            // if query is not null
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.BlogId}) {item.Name}");

                            }
                            string blogIdChoice = Console.ReadLine();

                            // enter post details
                            Post post = new Post();
                            
                            // blog ID of post taken from selection
                            post.BlogId = int.Parse(blogIdChoice);
                            
                            // title of post
                            Console.WriteLine("Enter the title of the post");
                            string postTitle = Console.ReadLine();
                            
                            // content of post
                            Console.WriteLine("Enter the content of the post");
                            string postConsole = Console.ReadLine();

                            // saved to Posts table
                            db.AddPost(post);
                            logger.Info("Post added - {name}", postTitle);
\
                            // *** remember to handle user errors

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
