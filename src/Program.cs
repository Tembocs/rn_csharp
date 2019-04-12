using System;
using System.IO;

namespace rn
{
    /// Finds the location of the images
    /// Copy images into the newly created image directory
    class Copier
    {
        // These variables should not be public to avoid external modification.
        // This is where Windows 10 store login images
        const string assetsSegment = "\\AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";
        
        // Find the user directory
        string homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        // A new directory on the desktop to copy images to
        string new_image_directory;

        // Original image directory after inserting the home directory
        string original_image_directory;

        // Index for renaming files and counting
        int index;

        public Copier()
        {
            original_image_directory = homePath + assetsSegment;
            new_image_directory = homePath + "\\Desktop\\processed_images";
            index = 0;
        }

        /// Check if there was previous version of image directory and delete it.
        /// Create a new image directory.
        public void prepareDirectory()
        {
            if (Directory.Exists(new_image_directory))
            {
                Console.WriteLine("-- Previous directory exists, deleting ...");

                // There is a previous directory delete it
                try
                {
                    Directory.Delete(new_image_directory, true);
                }
                catch (IOException ioException)
                {
                    Console.WriteLine(ioException.Message);
                }
                catch (UnauthorizedAccessException uAccessException)
                {
                    Console.WriteLine(uAccessException.Message);
                }

                // Then create a new directory
                try
                {
                    Directory.CreateDirectory(new_image_directory);
                }
                catch (IOException ioException)
                {
                    Console.WriteLine(ioException.Message);
                }
                catch (UnauthorizedAccessException uAccessException)
                {
                    Console.WriteLine(uAccessException.Message);
                }
            } else {
                // No previous directory, create a new one.
                try
                {
                    Directory.CreateDirectory(new_image_directory);
                }
                catch (IOException ioException)
                {
                    Console.WriteLine(ioException.Message);
                }
                catch (UnauthorizedAccessException uAccessException)
                {
                    Console.WriteLine(uAccessException.Message);
                }
            }
        }

        /// Copy image files into the new directory and rename each file.
        public void copyFiles()
        {
            // int index = 1;
            string[] files = Directory.GetFiles(original_image_directory);

            foreach (var aFile in files)
            {
                var destinationFileName = "image_" + index.ToString() + ".jpg";
                var newFileName = new_image_directory + "\\" + destinationFileName;
                File.Copy(aFile, newFileName);
                index++;
            }
        }

        public int fileCount()
        {
            return index;
        }
    }


    class Program
    {
        /// Staring point.
        static void Main(string[] args)
        {
            string decorator = new String('*', 50);

            Console.WriteLine($"\n{decorator}");
            Console.WriteLine("-- Starting ...");

            var copier = new Copier();
            copier.prepareDirectory();
            Console.WriteLine("-- Processing ...");
            copier.copyFiles();

            Console.WriteLine($"-- Image files processed: {copier.fileCount()}");
            Console.WriteLine("-- done.");
            Console.WriteLine($"{decorator}\n");
        }
    }
}
