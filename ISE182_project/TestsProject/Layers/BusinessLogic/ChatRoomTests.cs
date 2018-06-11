
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISE182_project;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISE182_project.Layers.CommunicationLayer;
using System.Collections.Generic;

namespace ISE182_project.Layers.BusinessLogic
{
    [TestClass]
    public class ChatRoomTests
    {

        [TestInitialize]
        public void Init()
        {
            ChatRoom.start(ChatRoom.Place.Home); //HERE
        }

        #region isLoggedIn

        [TestMethod]
        public void isLoggedInTest_Positive()
        {
            string UserName = RandomString(8);
            int GroupID = RandomInt();

            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            // Make sure there is no user currenly connected
            try
            {
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }

            LogIn(UserName, GroupID);

            // check if a user is now connected
            try
            {
                Assert.IsTrue(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        [TestMethod()]
        public void isLoggedInTest_Negative()
        {
            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            try
            {
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        #endregion

        #region register

        [TestMethod()]
        public void registerTest_Positive()
        {
            string UserName = RandomString(8);
            int GroupID = RandomInt();

            // if already logged in - logout
            try
            {
                ChatRoom.logout();
            }
            catch { }

            // try to register
            try
            {
                ChatRoom.register(UserName, GroupID);
            }
            catch
            {
                Assert.Fail("falid to check if a user is loggedIn");
            }
        }

        [TestMethod()]
        public void registerTest_Negative()
        {
            int GroupID = RandomInt();

            // "" registration
            try
            {
                ChatRoom.register("", GroupID);
                Assert.Fail("registered with empty string");
            }
            catch (Exception e)
            {
               //GOOD
            }

            //Null registration
            try
            {
                ChatRoom.register(null, GroupID); ;
                Assert.Fail("registered with null");
            }
            catch (Exception e)
            {
                //GOOD
            }

            // try to register with an existing user

            try
            {
                ChatRoom.register("Elephant", GroupID);
            }
            catch { }

            try
            {
                ChatRoom.register("Elephant", GroupID);
                Assert.Fail("registered with existing user");
            }
            catch (Exception e)
            {
                
            }
        }

        #endregion

        #region login

        [TestMethod()]
        public void loginTest_Positive()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            // logout
            try
            {
                ChatRoom.logout();
            }
            catch { }
        }

        [TestMethod()]
        public void loginTest_Negative()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            //Login when logged in
            try
            {
                ChatRoom.login(UserName, GroupID);
                Assert.Fail("login when loggedin");
            }
            catch (Exception e)
            {
                //GOOD
            }

            // "" LogIn
            try
            {
                ChatRoom.login("", GroupID);
            }
            catch (Exception e)
            {
               //GOOD
            }

            // Null LogIn
            try
            {
                ChatRoom.login(null, GroupID);
            }
            catch (Exception e)
            {
               //GOOD
            }

            // logIn whithout register
            try
            {
                ChatRoom.login(RandomString(15), GroupID);
            }
            catch (Exception e)
            {
               //GOOD
            }
        }

        #endregion

        #region logout

        [TestMethod()]
        public void logoutTest_Positive()
        {
            string UserName = RandomString(7);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            //logout
            try
            {
                ChatRoom.logout();
                Assert.IsFalse(ChatRoom.isLoggedIn());
            }
            catch
            {
                Assert.Fail("Faild to logout");
            }
        }

        [TestMethod()]
        public void logoutTest_Negative()
        {
            //logout if logged in
            try
            {
                ChatRoom.logout();
            }
            catch { }

            //logout when loggedout
            try
            {
                ChatRoom.logout();
            }
            catch (Exception e)
            {
               //GOOD
            }
        }

        #endregion

        #region send

        [TestMethod()]
        public void sendTest_Positive()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            try
            {
                string message = "";

                for (int i = 1; i <= 30; i++)
                {
                    message = "Message #" + i;
                    ChatRoom.send(message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Faild to send a message!");
            }
        }

        [TestMethod()]
        public void sendTest_Negative()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            LogIn(UserName, GroupID);

            // "" sending
            try
            {
                ChatRoom.send("");
                Assert.Fail("Empty message was sent!");
            }
            catch
            {
                //Good
            }

            //null Sending
            try
            {
                ChatRoom.send(null);
                Assert.Fail("null message was sent!");
            }
            catch
            {
                //Good
            }

            // more tham 150 sending
            try
            {
                string morethan150 = "";

                for (int i = 1; i <= 200; i++)
                    morethan150 += "A";

                ChatRoom.send(morethan150);

                Assert.Fail("too long message was sent!");
            }
            catch
            {
                //Good
            }
        }

        #endregion

        #region request20Messages

        [TestMethod()]
        public void request20MessagesTest_Positive()
        {
            string UserName = RandomString(5);
            int GroupID = RandomInt();

            string message;
            

            LogIn(UserName, GroupID);

            for (int i = 0; i < 30; i++)
            {
                message = "" + i;
                ChatRoom.send(message);
                System.Threading.Thread.Sleep(1001);
            }

            IMessage[] last20 = ChatRoom.request20Messages().ToArray();

            for (int i = 0; i < 20; i++)
            {
                if (!last20[i].MessageContent.Equals((i + 10).ToString()))
                    Assert.Fail("Falid to retrivedlast 20 correctly!");
            }
        }

        [TestMethod()]
        public void request20MessagesTest_Negative()
        {
            //NONE
        }

        #endregion

        #region sortByTime

        [TestMethod()]
        public void sortByTimeTest_Positive()
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

            ChatRoom.sort(ChatRoom.Sort.Time, false);
            UnSorted = ChatRoom.getMessages();

            IMessage[] NotSorted = UnSorted.ToArray();

            for (int i = 0; i < 20; i++)
            {
                if (!(last20copy[i] as IMessage).Equals(NotSorted[i] as IMessage))
                    throw new Exception("Falid to sortmessage by their time!");
            }

            ChatRoom.sort(ChatRoom.Sort.Time, true);
            UnSorted = ChatRoom.getMessages();

            NotSorted = UnSorted.ToArray();

            for (int i = 0; i < 20; i++)
            {
                if (!(last20copy[i] as IMessage).Equals(NotSorted[19 - i] as IMessage))
                    Assert.Fail("Falid to sortmessage by their time desending!");
            }
        }

        [TestMethod()]
        public void sortByTimeTest_Negative()
        {
            //NONE
        }

        #endregion

        #region sortByNickName

        [TestMethod()]
        public void sortByNickNameTest_Positive()
        {
            string UserName;
            int GroupID = RandomInt();

            string[] namestarts = new string[] { "zzzzz", "aaaa", "abcde", "qqqqq", "bacc", "th", "m", "udssss", "yy", "f" };         
            string[] returnedNames;

            for (int i = 0; i < namestarts.Length; i++)
            {
                UserName = namestarts[i] + RandomString(7);
                LogIn(UserName, GroupID);
                ChatRoom.send("Hello world");
                System.Threading.Thread.Sleep(1001);
            }

            IMessage[] temp = ChatRoom.request20Messages().Take(namestarts.Length).ToArray();
            ChatRoom.sort(ChatRoom.Sort.Nickname, false);
            IMessage[] mesgs = ChatRoom.getMessages().ToArray();

            returnedNames = new string[mesgs.Length];

            for (int i = 0; i < mesgs.Length; i++)
            {
                returnedNames[i] = mesgs[i].UserName;
            }

            Array.Sort(returnedNames);

            for (int i = 0; i < returnedNames.Length; i++)
            {
                if (!returnedNames[i].Equals(mesgs[i].UserName))
                    Assert.Fail("Failed tio sort by nick name!");
            }

            ChatRoom.sort(ChatRoom.Sort.Nickname, true);
            mesgs = ChatRoom.getMessages().ToArray();

            returnedNames = new string[mesgs.Length];

            for (int i = 0; i < mesgs.Length; i++)
            {
                returnedNames[i] = mesgs[i].UserName;
            }

            Array.Sort(returnedNames);
            returnedNames = returnedNames.Reverse().ToArray();

            for (int i = 0; i < returnedNames.Length; i++)
            {
                if (!returnedNames[i].Equals(mesgs[i].UserName))
                    Assert.Fail("Failed to sort by nick name desending!");
            }
        }

        [TestMethod()]
        public void sortByNickNameTest_Negative()
        {
            //NONE
        }

        #endregion

        #region requestAllMessagesfromUser

        [TestMethod()]
        public void requestAllMessagesfromUserTest_Positive()
        {
            string UserName = RandomString(10);
            int GroupID = RandomInt();

            ICollection<IMessage> temp;

            try
            {
                ChatRoom.logout();
            }
            catch { }

            temp = ChatRoom.requestMessagesfromUser(UserName, GroupID);

            if (temp.Count != 0)
               Assert.Fail("nedd to be emty arry!1");

            ChatRoom.register(UserName, GroupID);

            temp = ChatRoom.requestMessagesfromUser(UserName, GroupID);

            if (temp.Count != 0)
                Assert.Fail("nedd to be emty arry!2");

            for (int i = 0; i < 30; i++)
            {
                ChatRoom.send(UserName + " #" + i);
                System.Threading.Thread.Sleep(1000);
            }

            IMessage[] sent = ChatRoom.requestMessagesfromUser(UserName, GroupID).ToArray();

            for (int i = 0; i < 30; i++)
            {
                if (!(sent[i] as IMessage).MessageContent.Equals(UserName + " #" + i))
                    Assert.Fail("Didn't recved message correctly!");
            }
        }

        [TestMethod()]
        public void requestAllMessagesfromUserTest_Negative()
        {
            //NONE
        }

        #endregion

        #region requestAllMessagesfromGroup

        [TestMethod()]
        public void requestAllMessagesfromGroupTest_Positive()
        {
            string UserName;
            int GroupID = RandomInt();

            IMessage[] temp;

            for (int i = 0; i < 30; i++)
            {
                UserName = RandomString(16);

                LogIn(UserName, GroupID);

                ChatRoom.send("GroupFilterTest #" + i);
                System.Threading.Thread.Sleep(1001);
            }

            temp = ChatRoom.requestMessagesfromGroup(GroupID).Reverse().Take(30).Reverse().ToArray();

            for (int i = 0; i < 30; i++)
            {
                if (!(temp[i] as IMessage).MessageContent.Equals("GroupFilterTest #" + i))
                    Assert.Fail("Didn't recved message correctly!");
            }
        }

        [TestMethod()]
        public void requestAllMessagesfromGroupTest_Negative()
        {
           //NONE
        }

        #endregion




        #region Healpers

        private static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int RandomInt()
        {
            Random random = new Random();
            return random.Next(1, 35);
        }

        private static void LogIn(string UserName, int GroupID)
        {
            //logout if logged in
            try
            {
                ChatRoom.logout();
            }
            catch { }

            //rgister + login if not registered
            try
            {
                ChatRoom.register(UserName, GroupID);
            }
            catch { }

            //Make sure user is connected
            try
            {
                bool t = ChatRoom.isLoggedIn();

                if (!t)
                    Assert.Fail("Didn't logged in 100  + "+UserName+" + " + GroupID);
            }
            catch(Exception e)
            {
                Assert.Fail("Didn't logged in 123 : " + e.Message);
            }
        }

        #endregion
    }
}