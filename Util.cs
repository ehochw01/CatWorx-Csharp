using System;
using System.IO;
using System.Collections.Generic;
using SkiaSharp;

namespace CatWorx.BadgeMaker {
    class Util {
        public static void PrintEmployees(List<Employee> employees) {
            for (int i=0 ; i < employees.Count; i++) {
                string template = "{0,-10}\t{1,-20}\t{2}";
                Console.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoUrl()));
            }
        }
        public static void MakeCSV(List<Employee> employees) {
            if (!Directory.Exists("data")) {
                Directory.CreateDirectory("data");
            }

            // this code block temporarily uses the StreamWriter file
            using (StreamWriter file = new StreamWriter("data/employees.csv")) {
                file.WriteLine("ID,Name,PhotoUrl");

                // loop over employees
                for (int i=0; i < employees.Count; i++) {
                    // write each employee to the file
                    string template = "{0},{1},{2}";
                    file.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoUrl()));
                }
            }
        }
        // will use async/await syntax because the method we use from the HttpClient object is asynchronous 
        // Task is the required return type for an async method that returns no value
        public static void MakeBadges(List<Employee> employees) {
        // Import the badge template image file that will work as the background image.
        SKImage newImage = SKImage.FromEncodedData(File.OpenRead("badge.png"));
        SKData data = newImage.Encode();
        data.SaveTo(File.OpenWrite("data/employeeBadge.png"));
        // Customize each employee's badge by adding information specific to each employee—namely, the employee's name, picture, and id number.
        // Add this new image file to the data folder.
        }


    }
}