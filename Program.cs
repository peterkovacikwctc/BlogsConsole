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
                    var postQuery = db.Posts.OrderBy(p => p.PostId);
                    
                    // string variable for input (convert to integer)
                    string input;

                    // variables
                    int blogIdChoice;
                    bool repeat;
                        

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
                            
                            if (query == null) {
                                Console.WriteLine("There are no blogs.");
                                Console.WriteLine("Choose option 2 to add a blog.\n");
                            }
                            
                            else {
                                
                                do {
                                    // prompt user to select blog 
                                    Console.WriteLine("Select a blog to view its posts.");
                                    
                                    // if query is not null
                                    foreach (var item in query)
                                    {
                                        Console.WriteLine($"{item.BlogId}) {item.Name}");

                                    }
                                    input = Console.ReadLine();
                                    try {
                                        blogIdChoice = int.Parse(input);
                                        repeat = false;
                                    }
                                    // error: not an integer
                                    catch {
                                        Console.WriteLine("\nInvalid choice.");
                                        Console.WriteLine("Please enter the number of one of the blogs.\n");
                                        blogIdChoice = -99;
                                        repeat = true;
                                    }
                                    // error: integer is out of range
                                    maxBlogNumber = db.Blogs.Max(b => b.BlogId);
                                    if ((blogIdChoice < 1 || blogIdChoice > maxBlogNumber) && blogIdChoice != -99) { 
                                        Console.WriteLine("\nInvalid choice.");
                                        Console.WriteLine("Please enter the number of one of the blogs.\n");
                                        repeat = true;
                                    }
                                } while (repeat == true); // repeat loop if either error occurs (not an integer or out of range)
                                
                                // enter post details
                                Post post = new Post();
                                
                                // blog ID of post taken from selection
                                post.BlogId = blogIdChoice;
                                
                                // title of post
                                Console.WriteLine("Enter the title of the post: ");
                                post.Title = Console.ReadLine();
                                
                                // content of post
                                Console.WriteLine("Enter the content of the post: ");
                                post.Content = Console.ReadLine();

                                // save to Posts table
                                db.AddPost(post);
                                logger.Info("Post added - {name}", post.Title);
                            }

                            break;

                        case "4":
                            
                            // select posts from all blogs or from particular blogs    
                            Console.WriteLine("Select which posts to display: ");
                            
                            Console.WriteLine("0) Posts from all blogs");
                            // display id number and name for each blog
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");

                            }
                            
                            // user selects choice
                            blogIdChoice = int.Parse(Console.ReadLine());
                            Console.WriteLine(""); // blank line for formatting

                            // determine number of blogs (range of valid answers)
                            maxBlogNumber = db.Blogs.Max(b => b.BlogId);
                            
                            // 0 - display all posts
                            if (blogIdChoice == 0) {
                                
                                foreach (var item in postQuery)
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}");
                                    Console.WriteLine($"Title: {item.Title}");
                                    Console.WriteLine($"Content: {item.Content}\n");
                                }
                            }

                            // choose particular post (1 - max PostID)
                            else if (blogIdChoice > 0 && blogIdChoice <= maxBlogNumber) {
                                
                                foreach (var item in postQuery)
                                {
                                    // only display post items corresponding to correct Blog Id
                                    if (item.BlogId == blogIdChoice) {
                                        Console.WriteLine($"Blog: {item.Blog.Name}");
                                        Console.WriteLine($"Title: {item.Title}");
                                        Console.WriteLine($"Content: {item.Content}\n");
                                    }
                                }
                            }
                            // if choice is outside of range, it is invalid
                            else {
                                Console.WriteLine("Invalid input. Choose an integer from the menu.");
                            }
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
