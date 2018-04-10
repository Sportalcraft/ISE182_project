using ISE182_project.Layers.BusinessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project
{
    class Tester
    {
        public void run()
        {
            //TestUser();
            //TestMessage();
            Console.WriteLine("This test was run succesfully, some method won't work the second time, due to the persistent layer.\nNo test were done now");
        }

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

        #region Testers

        private void TestUser()
        {
            registerTest();
            LogInTest();
            logoutTest();
        }

        private void TestMessage()
        {
            sendTest();
            last20Test();
            SortMessageTest();
            requestFromTest();
        }

        #endregion

        #region User Testing

        private void registerTest()
        {
            bool passed = true;

            ChatRoom.register("Yossi");

            try
            {
                ChatRoom.register("Yossi");
                passed = false;
            }
            catch
            {
                Console.WriteLine("Passed registration!");
                return;
            }
            if (!passed)
                throw new InvalidOperationException("registerd an existing user!");
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
                throw new InvalidOperationException("registerd with empty string!");
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
                throw new InvalidOperationException("registerd with null!");
            passed = true;

            Console.WriteLine("Passed registration!");
        }

        private void LogInTest()
        {
            bool passed = true;

            if (ChatRoom.isLoggedIn())
                throw new InvalidOperationException("Already loggedin");

            ChatRoom.login("Yossi");

            if (!ChatRoom.isLoggedIn())
                throw new InvalidOperationException("falid to loggin");

            try
            {
                ChatRoom.login("Yossi");
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new InvalidOperationException("login while loggedin!");
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
                throw new InvalidOperationException("loggedin with empty string!");
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
                throw new InvalidOperationException("loggedin with null!");
            passed = true;

            try
            {
                ChatRoom.login("nbgknbtroig reofmefnrkgnrpfcj ewriob tobjrwpgj rteoigjvwp");
                passed = false;
            }
            catch
            {
                //Good
            }

            if (!passed)
                throw new InvalidOperationException("loggedin with not registerd user!");
            passed = true;

            Console.WriteLine("Passed loggin!");
        }

        private void logoutTest()
        {
            bool passed = true;

            if (!ChatRoom.isLoggedIn())
                throw new InvalidOperationException("loggedin first!");

            ChatRoom.logout();

            if (ChatRoom.isLoggedIn())
                throw new InvalidOperationException("didn't loggedout!");

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
                throw new InvalidOperationException("logout while loggedout!");
            passed = true;

            Console.WriteLine("Passed logout!");
        }
        
        #endregion

        #region Message Testing

        private void sendTest()
        {
            bool passed = true;

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
                throw new Exception("message must be atleast 1 character!");
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

                    ChatRoom.send(message);
                }
            }
            catch
            {
                //Good
            }
            if (!passed)
                throw new Exception("faild sending a message!");
            passed = true;

            Console.WriteLine("Passed send Test!");
        }

        private void last20Test()
        {
            string message;

            ChatRoom.login("Yossi");

            for (int i = 0; i < 30; i++)
            {
                message = "" + i;
                ChatRoom.send(message);
                System.Threading.Thread.Sleep(1000);
            }

            ArrayList last20 = ChatRoom.request20Messages();


            for (int i = 0; i < 20; i++)
            {
                Message temp = (Message)last20[i];

                if (!temp.MessageContent.Equals((i + 10).ToString()))
                    throw new Exception("Falid to retrivedlast 20 correctly!");
            }

            Console.WriteLine("Passed last20 test!");
        }

        private void SortMessageTest()
        {
            ArrayList last20 = ChatRoom.request20Messages();
            ArrayList last20copy = (ArrayList)last20.Clone();
            Random rnd = new Random();
            ArrayList UnSorted = new ArrayList();
            int rando;

            while (last20.Count != 0)
            {
                rando = rnd.Next(0, last20.Count);

                UnSorted.Add(last20[rando]);
                last20.RemoveAt(rando);
            }

            //MessageService.sort(UnSorted); // need to return to private method later

            for (int i = 0; i < 20; i++)
            {
                if (!((Message)last20copy[i]).Equals((Message)UnSorted[i]))
                    throw new Exception("Falod to sort!");
            }

            Console.WriteLine("Passed sort test!");
        }

        private void requestFromTest()
        {
            ArrayList temp;
            string USER = "Cat";

            temp = ChatRoom.requestAllMessagesfromUser(USER, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");

            ChatRoom.register(USER);

            temp = ChatRoom.requestAllMessagesfromUser(USER, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");

            ChatRoom.login(USER);

            temp = ChatRoom.requestAllMessagesfromUser(USER, 32);

            if (temp.Count != 0)
                throw new Exception("nedd to be emty arry!");


            for (int i = 0; i < 30; i++)
            {
                ChatRoom.send(USER + " #" + i);
                System.Threading.Thread.Sleep(1000);
            }

            ArrayList sent = ChatRoom.requestAllMessagesfromUser(USER, 32);

            for (int i = 0; i < 30; i++)
            {
                if (!((Message)sent[i]).MessageContent.Equals(USER + " #" + i))
                    throw new Exception("Didn't recved message correctly!");
            }

            Console.WriteLine("Passed request reom test!");
        }

        #endregion
    }
}
