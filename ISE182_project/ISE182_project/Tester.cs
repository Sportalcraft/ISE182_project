using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project
{
    // Test our Code
    class Tester
    {
        #region Public Testers

        public void run()
        {
            Console.WriteLine("Welcome to the tester.\nThis tester will take 1-2 minutes to end due to time delaytion before sending messages,\nto make sure they are all sent on a difrrent time.\nPlease be patient and wait until 'Test finished' will be printed, or until an exception trown");
            boldingText("WARNING : it is best to NOT run this at the unuversity, though it should be ok.\nDon't take that risk", ConsoleColor.Red);
            Console.WriteLine("Press any key to start the tester, or close the program");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine();

            TestUser();
            Console.WriteLine();
            Console.WriteLine();
            TestMessage();
            Console.WriteLine();
            Console.WriteLine();

            boldingText("Passed all tests!", ConsoleColor.Green);
            boldingText("Test Finished", ConsoleColor.Cyan);
        }

        public void testUniversityConection()
        {
            try
            {
                ChatRoom.register("Tal");
            }
            catch
            {

            }

            ChatRoom.login("Tal");

            ChatRoom.send("This us My Message");
        }

        #endregion

        #region singletone

        private static Tester instance;

        private Tester() { }

        public static Tester Instence
        {
            get
            {
                if (instance == null)
                    instance = new Tester();
                return instance;
            }
        }

        #endregion

        #region Private Testers

        private void TestUser()
        {
            registerTest();
            Console.WriteLine();
            LogInTest();
            Console.WriteLine();
            logoutTest();
            Console.WriteLine();

            boldingText("Passed User tests!", ConsoleColor.Green);
        }

        private void TestMessage()
        {
            sendTest();
            Console.WriteLine();
            last20Test();
            Console.WriteLine();
            SortTimeMessageTest();
            Console.WriteLine();
            requestFromTest();
            Console.WriteLine();

            boldingText("Passed Message tests!", ConsoleColor.Green);
        }

        #endregion

        #region User Testing

        private void registerTest()
        {
            bool passed = true;
            string UserName = RandomString(8);

            ChatRoom.register(UserName);

            try
            {
                ChatRoom.register(UserName);
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("registerd an existing user!");
            passed = true;

            try
            {
                ChatRoom.register("");
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("registerd with empty string!");
            passed = true;

            try
            {
                ChatRoom.register(null);
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("registerd with null!");
            passed = true;

            Console.WriteLine("Passed registration!");
        }

        private void LogInTest()
        {
            bool passed = true;
            string UserName = "Yossi";

            if (ChatRoom.isLoggedIn())
                throw new Exception("Already loggedin");

            try
            {
                ChatRoom.register(UserName);
            }
            catch
            {

            }

            ChatRoom.login(UserName);

            if (!ChatRoom.isLoggedIn())
                throw new Exception("falid to loggin");

            try
            {
                ChatRoom.login(UserName);
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("login while loggedin!");
            passed = true;

            try
            {
                ChatRoom.login("");
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("loggedin with empty string!");
            passed = true;

            try
            {
                ChatRoom.login(null);
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("loggedin with null!");
            passed = true;

            try
            {
                ChatRoom.login(RandomString(15));
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("loggedin with not registerd user!");
            passed = true;

            Console.WriteLine("Passed loggin!");
        }

        private void logoutTest()
        {
            bool passed = true;
            string UserName = "Yossi";

            LogIn(UserName);

            if (!ChatRoom.isLoggedIn())
                throw new Exception("loggedin first!");

            ChatRoom.logout();

            if (ChatRoom.isLoggedIn())
                throw new Exception("didn't loggedout!");

            try
            {
                ChatRoom.logout();
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("logout while loggedout!");
            passed = true;

            Console.WriteLine("Passed logout!");
        }

        #endregion

        #region Message Testing

        private void sendTest()
        {
            bool passed = true;
            string UserName = "Yossi";

            LogIn(UserName);


            try
            {
                ChatRoom.send("");
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("message must be atleast 1 character!");
            passed = true;

            try
            {
                ChatRoom.send(null);
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new Exception("messagecan't be null!");
            passed = true;

            try
            {
                string morethan150 = "";

                for (int i = 1; i <= 200; i++)
                    morethan150 += "A";

                ChatRoom.send(morethan150);
            }
            catch
            {
                //Good
            }
            if (!passed)
                throw new Exception("message must be no more than 150 characters!");
            passed = true;

            try
            {
                string message = "";

                for (int i = 1; i <= 30; i++)
                {
                    message = "Message #" + i;
                    System.Threading.Thread.Sleep(1000);
                    ChatRoom.send(message);
                }
            }
            catch(Exception e)
            {
                passed = false;
                boldingText(e.Message, ConsoleColor.Blue);
                Console.ReadKey();
            }

            if (!passed)
                throw new Exception("faild sending a message!");
            passed = true;

            Console.WriteLine("Passed send Test!");
        }

        private void last20Test()
        {
            string message;
            string UserName = "Yossi";

            LogIn(UserName);

            for (int i = 0; i < 30; i++)
            {
                message = "" + i;
                ChatRoom.send(message);
                System.Threading.Thread.Sleep(1001);
            }

            IMessage[] last20 = ChatRoom.request20Messages().ToArray();

            for (int i = 0; i < 20; i++)
            {
                Message temp = (Message)last20[i];

                if (!temp.MessageContent.Equals((i + 10).ToString()))
                    throw new Exception("Falid to retrivedlast 20 correctly!");
            }

            Console.WriteLine("Passed last20 test!");
        }

        private void SortTimeMessageTest()
        {
           ArrayList last20 = new ArrayList(ChatRoom.request20Messages().ToArray());
           ArrayList last20copy = last20.Clone() as ArrayList;
           Random rnd = new Random();
           ICollection<IMessage> UnSorted = new List<IMessage>();
            int rando;

            while (last20.Count != 0)
            {
                rando = rnd.Next(0, last20.Count);

                UnSorted.Add(last20[rando] as IMessage);
                last20.RemoveAt(rando);
            }

            UnSorted = MessageService.Instence.sort(UnSorted, MessageService.Sort.Time, false);

            IMessage[] NotSorted = UnSorted.ToArray();

            for (int i = 0; i < 20; i++)
            {
                if (!(last20copy[i] as IMessage).Equals(NotSorted[i] as IMessage))
                    throw new Exception("Falid to sortmessage by their time!");
            }

            Console.WriteLine("Passed sort test!");
        }

        private void requestFromTest()
        {
            ICollection<IMessage> temp;
            string UserName = RandomString(10);

            try
            {
                ChatRoom.logout();
            }
            catch
            {

            }

            temp = ChatRoom.requestAllMessagesfromUser(UserName, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");

            ChatRoom.register(UserName);

            temp = ChatRoom.requestAllMessagesfromUser(UserName, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");

            ChatRoom.login(UserName);

            temp = ChatRoom.requestAllMessagesfromUser(UserName, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");


            for (int i = 0; i < 30; i++)
            {
                ChatRoom.send(UserName + " #" + i);
                System.Threading.Thread.Sleep(1000);
            }

            IMessage[] sent = ChatRoom.requestAllMessagesfromUser(UserName, 32).ToArray();

            for (int i = 0; i < 30; i++)
            {
                if (!(sent[i] as IMessage).MessageContent.Equals(UserName + " #" + i))
                    throw new Exception("Didn't recved message correctly!");
            }

            Console.WriteLine("Passed request reom test!");
        }

        #endregion

        #region helpers

        private static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void LogIn(string UserName)
        {
            try
            {
                ChatRoom.logout();
            }
            catch
            {

            }

            try
            {
                ChatRoom.register(UserName);
            }
            catch
            {

            }

            try
            {
                ChatRoom.login(UserName);
            }
            catch
            {

            }
        }

        private void boldingText(string text, ConsoleColor color) 
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        #endregion
    }
}