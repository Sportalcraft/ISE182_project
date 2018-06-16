
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
        private const string password = "123456";

        [TestInitialize]
        public void Init()
        {
            ChatRoom.start(); //HERE
        }

        #region isLoggedIn

        [TestMethod]
        public void isLoggedInTest_Positive()
        {
            string UserName = RandomString(8);
            int GroupID = RandomInt();

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
                ChatRoom.register(UserName, GroupID, password);
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
                ChatRoom.register("", GroupID, password);
                Assert.Fail("registered with empty string");
            }
            catch (Exception e)
            {
               //GOOD
            }

            //Null registration
            try
            {
                ChatRoom.register(null, GroupID, password); ;
                Assert.Fail("registered with null");
            }
            catch (Exception e)
            {
                //GOOD
            }

            // try to register with an existing user

            try
            {
                ChatRoom.register("Elephant", GroupID, password);
            }
            catch { }

            try
            {
                ChatRoom.register("Elephant", GroupID, password);
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
                ChatRoom.login(UserName, GroupID, password);
                Assert.Fail("login when loggedin");
            }
            catch (Exception e)
            {
                //GOOD
            }

            // "" LogIn
            try
            {
                ChatRoom.login("", GroupID, password);
            }
            catch (Exception e)
            {
               //GOOD
            }

            // Null LogIn
            try
            {
                ChatRoom.login(null, GroupID, password);
            }
            catch (Exception e)
            {
               //GOOD
            }

            // logIn whithout register
            try
            {
                ChatRoom.login(RandomString(8), GroupID, password);
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
                    message = "Send Test #" + i;
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
            string UserName = RandomString(6);
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

        #region sortByTime

        [TestMethod()]
        public void sortByTimeTest_Positive()
        {
            ChatRoom.sort(ChatRoom.Sort.Nickname, false); //Not sorted enymore by time
            ICollection<IMessage> UnSorted = ChatRoom.getMessages();

            IMessage[] NotSorted = UnSorted.ToArray();
            DateTime[] sortedDates = new DateTime[NotSorted.Length];

            for (int i = 0; i < NotSorted.Length; i++)
                sortedDates[i] = NotSorted[i].Date;

            Array.Sort(sortedDates);

            ChatRoom.sort(ChatRoom.Sort.Time, false); // sort  by time
            ICollection<IMessage> sorted = ChatRoom.getMessages();
            IMessage[] SortedByChatRoom = sorted.ToArray();



            for (int i = 0; i < sortedDates.Length; i++)
            {
             //   try
                {
                    if (!(sortedDates[i]).Equals(SortedByChatRoom[i].Date))
                        Assert.Fail("Falid to sort message by their time!");
                }
              //  catch
                {
              //      Assert.Fail("Falid to sort message by their time! exeption was raised.");
                }
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
                ChatRoom.send("Nick Name Sort Test #"+i);
                System.Threading.Thread.Sleep(1001);
            }

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
            string UserName = RandomString(7);
            int GroupID = RandomInt();
            LogIn(UserName, GroupID);

            for (int i = 0; i < 30; i++)
            {
                ChatRoom.send("User Filter Test #" + i);
                System.Threading.Thread.Sleep(1000);
            }

            ChatRoom.filterByUser(UserName, GroupID);
            IMessage[] sent = ChatRoom.getMessages().ToArray();

            for (int i = 0; i < 30; i++)
            {
                if (!(sent[i]).MessageContent.Equals("User Filter Test #" + i))
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
        public void requestMessagesfromGroupTest_Positive()
        {
            string UserName;
            int GroupID = RandomInt();

            IMessage[] temp;

            for (int i = 0; i < 30; i++)
            {
                UserName = RandomString(7);

                LogIn(UserName, GroupID);

                ChatRoom.send("Group Filter Test #" + i);
                System.Threading.Thread.Sleep(1001);
            }

            ChatRoom.filterByGroup(GroupID);
            temp = ChatRoom.getMessages().Reverse().Take(30).Reverse().ToArray();

            for (int i = 0; i < 30; i++)
            {
                if (!(temp[i] as IMessage).MessageContent.Equals("Group Filter Test #" + i))
                    Assert.Fail("Didn't recved message correctly!");
            }
        }

        [TestMethod()]
        public void requestMessagesfromGroupTest_Negative()
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
                ChatRoom.register(UserName, GroupID, password);
                ChatRoom.login(UserName, GroupID, password);
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