using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using SkiaSharp;
using System.Threading.Tasks;

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
        async public static Task MakeBadges(List<Employee> employees) {
            int BADGE_WIDTH = 669;
            int BADGE_HEIGHT = 1044;

            int PHOTO_LEFT_X = 184;
            int PHOTO_TOP_Y = 215;
            int PHOTO_RIGHT_X = 486;
            int PHOTO_BOTTOM_Y = 517;

            int COMPANY_NAME_Y = 150;

            int EMPLOYEE_NAME_Y = 600;

            int EMPLOYEE_ID_Y = 730;

            // Import the badge template image file that will work as the background image.
            // Customize each employee's badge by adding information specific to each employee—namely, the employee's name, picture, and id number.
            // Add this new image file to the data folder.
            using(HttpClient client = new HttpClient()) {
                for (int i=0; i < employees.Count; i++) {
                    // Convert the photo URL into SKImage.
                    SKImage photo = SKImage.FromEncodedData(await client.GetStreamAsync(employees[i].GetPhotoUrl()));
                    // Convert badge template into SKImage.
                    SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));
                    // create the badge canvas
                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);
                    SKCanvas canvas = new SKCanvas(badge);
                    // Place the images onto a canvas.
                    canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                    canvas.DrawImage(photo, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y));
                    SKPaint paint = new SKPaint();
                    paint.TextSize = 42.0f;
                    paint.IsAntialias = true;
                    paint.Color = SKColors.White;
                    paint.IsStroke = false;
                    paint.TextAlign = SKTextAlign.Center;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial");
                    // write the company name
                    // Company name
                    canvas.DrawText(employees[i].GetCompanyName(), BADGE_WIDTH / 2f, COMPANY_NAME_Y, paint);
                    // write the employee name
                    paint.Color = SKColors.Black;
                    canvas.DrawText(employees[i].GetFullName(), BADGE_WIDTH / 2f, EMPLOYEE_NAME_Y, paint);
                    // write the employee id number
                    paint.Typeface = SKTypeface.FromFamilyName("Courier New");
                    canvas.DrawText(employees[i].GetId().ToString(), BADGE_WIDTH / 2f, EMPLOYEE_ID_Y, paint);
                    // create a new file
                    SKImage finalImage = SKImage.FromBitmap(badge);
                    SKData data = finalImage.Encode();
                    string template = "data/{0}_badge.png";
                    data.SaveTo(File.OpenWrite(string.Format(template, employees[i].GetId())));
                }
            }
        }


    }
}