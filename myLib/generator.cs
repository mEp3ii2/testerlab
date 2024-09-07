using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLib
{
    internal class generator
    {
        Random random = new Random();
        private readonly string[] firstNames = { "Alice", "Bob", "Charlie", "Diana", "Evelyn", "Frank", "Grace", "Henry", "Ivy", "Jack", "Karen", "Leo", "Mia", "Nate", "Olivia", "Paul", "Quinn", "Riley", "Sophia", "Tom", "Uma", "Vera", "Will", "Xena", "Yara", "Zane" };
        private readonly string[] lastNames = {"Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker"};

        private readonly List<Bitmap> images;
        private string GetFirstname()
        {
            return firstNames[random.Next(firstNames.Length)];
        }

        private string GetLastname()
        {
            return lastNames[random.Next(lastNames.Length)];
        }

        private uint GetPIN()
        {
            return (uint)random.Next(9999);
        }

        private uint GetAcctNo()
        {

            return (uint)random.Next(1, 99999);
        }

        private int GetBalance()
        {
            return random.Next(-99999999, 99999999);
        }

        public generator()
        {
            images = new List<Bitmap>();
            // Generate a few really basic icons
            // Probably not the best way to do it, but it works :)
            for (var i = 0; i < 10; i++)
            {
                var image = new Bitmap(64, 64);
                for (var x = 0; x < 64; x++)
                {
                    for (var y = 0; y < 64; y++)
                    {
                        image.SetPixel(x, y, Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
                    }
                }
                images.Add(image);
            }
        }

        private Bitmap GetProfilePic()
        {
            return images[random.Next(images.Count)];
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance,out Bitmap pic)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = GetBalance();
            pic = GetProfilePic();
        }
    }
}
