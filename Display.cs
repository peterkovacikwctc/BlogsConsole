using System;
using System.IO;

namespace BlogsConsole
{
    public class Display {

        public void menu() {
            Console.WriteLine("\nEnter your selection: ");
            Console.WriteLine("1) Display all blogs");
            Console.WriteLine("2) Add blog");
            Console.WriteLine("3) Create post");
            Console.WriteLine("4) Display posts");
            Console.WriteLine("Enter q to quit");
        }

        public void quit() {
            Console.WriteLine("Goodbye!");
        }

        public void defaultMessage(string menuChoice) {
            Console.WriteLine($"{menuChoice} is not a valid response.");
        }

        public void headerBlogNames(int numberBlogs) {
            if (numberBlogs == 1)
                Console.WriteLine($"1 blog in the database:");
            else if (numberBlogs > 1)
                Console.WriteLine($"{numberBlogs} blogs in the database:");
            else
                Console.WriteLine($"0 blogs in the database:");
        }

    }
}