using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deletefile_sftp
{
    class Program
    {
        int i = 0;

        static void Main(string[] args)
        {
            startApp();
        }


        static void startApp()
        {
            //เวลาเริ่มทำงาน
            var Timestart = DateTime.Now;
            String username = "username";
            String password = "password";
            String host = "public_ip";


            Console.WriteLine("please input path delete");

            using (SftpClient client = new SftpClient(new PasswordConnectionInfo(host, username, password)))
            {
                client.Connect();

                // รับค่าจากแป้นพิมพ์
                var input = Convert.ToString(Console.ReadLine());
                //  var path = "/public_html/wp-content/uploads/";

                //ถ้าเจอ folder นี้ให้ไปเรียกใช้ฟังชั่น ShowFiles 
                //if (client.Exists(path + input))
                if (client.Exists(input))
                {
                    var serverFolder = input;
                    new Program().ShowFiles(client, serverFolder, Timestart);
                }
                //ถ้าไม่เจอให้เริ่มการทำงานใหม่
                else
                {
                    Console.WriteLine("file not found");
                    startApp();
                }
                client.Disconnect();
            }


            Console.ReadLine();
        }

        private void ShowFiles(SftpClient client, string serverFolder, DateTime Timestart)
        {
            var paths = client.ListDirectory(serverFolder);
            //แสดง file ที่อยู่ใน folder ทั้งหมด
            foreach (var path in paths)
            {
                var name = path.Name; //ชื่อรูปภาพ

                //ถ้าชื่อรูปภาพมีคำว่า @2x ให้ลบทันทีโดยจะไปเรียกใช้ฟังค์ชั่น Delete หากไม่สนใจชื่อรูปภาพให้ คอมเม้น if ทิ้งได้เลย
                if (name.Contains("@2x"))
                {
                    //path ที่อยู่ของรูปภาพ
                    var pathDelete = path.FullName;
                    //เรียกใช้ฟังค์ชั่น Delete
                    Delete(client, pathDelete);
                }
            }

            //หลังจากทำงานเสร็จ
            //เวลาทำงานเสร็จ
            var TimeEnd = DateTime.Now;
            //เวลาทำงานเสร็จ - เวลาเริ่มทำงาน = ใช้เวลาในการลบรูปภาพทั้งหมด
            var total = TimeEnd - Timestart;
            //บอกเป็นข้อความว่า ใช้เวลาในการลบรูปภาพทั้งหมด กี่ ชั่วโมง/นาที
            Console.WriteLine("time :" + total);
            // ให้เริ่มเป็น 0
            i = 0; 

            startApp();


            //  Console.WriteLine("File :" + path.Name);
        }



        private void Delete(SftpClient client, string file)
        {

            //ถ้าไฟล์นี้มีจริง
            if (client.Exists(file))
            {
                //จำนวนไฟล์ที่ลบจะ run ไปเรื่อยๆ จะได้รู้ว่าตอนนี้ลบไปกี่ไฟล์แล้ว
                i += 1;

                //ลบไฟล์
                client.Delete(file);

                //ลบไปถึงไฟล์ไหนแล้วและลบไฟล์ชื่อว่าอะไร
                Console.WriteLine(" " + i + " Deleted :" + file + " success");


            }
        }


    }
}
