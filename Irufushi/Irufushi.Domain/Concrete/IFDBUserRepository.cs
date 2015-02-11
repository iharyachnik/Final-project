using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract.Abstract;
using Abstract.Entities;
using System.Data.Entity;

namespace Irufushi.Domain.Concrete
{
    public class IFDBUserRepository : IUserPagesRepository
    {
        EFDBContext context = new EFDBContext();

        public IQueryable<UserProfile> UserProfiles
        {
            get { return context.UserProfiles; }
        }

        public void SaveUser(UserProfile user)
        {
            context.UserProfiles.Add(user);
        }

        public UserProfile GetUser(int id)
        {
            UserProfile User;

            User = context.UserProfiles.First(m => m.UserId == id);

            return User;
        }

        public void SaveProfile(UserProfile user)
        {
            if (user == null) return;
            UserProfile dbUser = context.UserProfiles.Find(user.UserId);

            if (dbUser != null)
            {
                AboutUser about = context.AboutUsers.Find(user.UserId);
                if (about == null) user.AboutUser.Id = user.UserId;
                Contacts contacts = context.Contacts.Find(user.UserId);
                if (contacts == null) user.Contacts.Id = user.UserId;
                Location location = context.Locations.Find(user.UserId);
                if (location == null) user.Location.Id = user.UserId;

                SaveAboutUser(user.AboutUser);
                SaveContacts(user.Contacts);
                SaveLocation(user.Location);
            }
            else
            {
                return;
            }

            context.SaveChanges();
        }

        public void SaveAboutUser(AboutUser about)
        {
            AboutUser dbAbout = context.AboutUsers.Find(about.Id);

            if (dbAbout == null)
            {
                context.AboutUsers.Add(about);
            }
            else
            {
                dbAbout.FirstName = about.FirstName;
                dbAbout.LastName = about.LastName;
                dbAbout.BirthDate = about.BirthDate;
                dbAbout.Gender = about.Gender;
            }
            context.SaveChanges();
        }

        public void SaveContacts(Contacts contacts)
        {
            Contacts dbContacts = context.Contacts.Find(contacts.Id);

            if (dbContacts == null)
            {
                context.Contacts.Add(contacts);
            }
            else
            {
                dbContacts.PhoneNum = contacts.PhoneNum;
                dbContacts.MobPhoneNum = contacts.MobPhoneNum;
                dbContacts.Skype = contacts.Skype;
                dbContacts.ContEmail = contacts.ContEmail;
                dbContacts.ICQ = contacts.ICQ;
            }
            context.SaveChanges();
        }

        public void SaveLocation(Location location)
        {
            Location dbLocation = context.Locations.Find(location.Id);

            if (dbLocation == null)
            {
                context.Locations.Add(location);
            }
            else
            {
                dbLocation.Country = location.Country;
                dbLocation.City = location.City;
                dbLocation.Address = location.Address;
            }
            context.SaveChanges();
        }

        public void AddFriend(FriendShip friendship)
        {
            if (friendship.UserId == 0 || friendship.FriendId == 0 
                || friendship.UserId == friendship.FriendId) return;

            var dbFriendshipsUserId = context.FriendShips
                .Where(x => x.UserId == friendship.UserId);
            foreach (var item in dbFriendshipsUserId)
            {
                if (item.FriendId == friendship.FriendId) return;
            }
            var dbFriendshipsFriendId = context.FriendShips
                .Where(x => x.FriendId == friendship.FriendId);
            foreach (var item in dbFriendshipsFriendId)
            {
                if (item.UserId == friendship.UserId) return;
            }

            context.FriendShips.Add(friendship);
            context.SaveChanges();
        }

        public void DeleteFriend(FriendShip friendship)
        {
            if (friendship.UserId == 0 || friendship.FriendId == 0) return;

            var friendshipQueryUID = context.FriendShips
                .Where(x => x.UserId == friendship.UserId)
                .Where(x => x.FriendId == friendship.FriendId).FirstOrDefault();

            var friendshipQueryFID = context.FriendShips
                .Where(x => x.FriendId == friendship.UserId)
                .Where(x => x.UserId == friendship.FriendId).FirstOrDefault();

            if(friendshipQueryUID != null) context.FriendShips.Remove(friendshipQueryUID);
            if(friendshipQueryFID != null) context.FriendShips.Remove(friendshipQueryFID);

            context.SaveChanges();
        }

        public List<UserProfile> GetFriends(int id)
        {
            var userIds = context.FriendShips.Where(x => x.UserId == id);
            var friendIds = context.FriendShips.Where(x=> x.FriendId == id);

            List<int> friendshipList = new List<int>();
            foreach (var item in userIds)
            {
                friendshipList.Add(item.FriendId);
            }
            foreach (var item in friendIds)
            {
                friendshipList.Add(item.UserId);
            }

            List<UserProfile> users = new List<UserProfile>();
            foreach (var item in friendshipList)
            {
                users.Add(GetUser(item));
            }

            return users;
        }

        public bool IsFriend(int idUser, int idFriend)
        {
            if (idUser == 0 || idFriend == 0) return false;

            var friendshipUID = context.FriendShips.Where(x => x.UserId == idUser)
                .Where(x => x.FriendId == idFriend);

            foreach (var item in friendshipUID)
            {
                if (item.UserId == idUser && item.FriendId == idFriend)
                    return true;
            }

            var friendshipFID = context.FriendShips.Where(x => x.FriendId == idUser)
                .Where(x => x.UserId == idFriend);

            foreach (var item in friendshipFID)
            {
                if (item.UserId == idFriend && item.FriendId == idUser)
                    return true;
            }

            return false;
        }

        public List<UserProfile> SearchUsers(string firstName, string lastName,
            string country, string city)
        {
            var req = false;
            var and = false;
            string searchRequest = "Select UserId from UserProfile";

            if (firstName != null || lastName != null)
            {
                searchRequest += " INNER JOIN AboutUsers ON UserProfile.UserId = AboutUsers.Id";
                req = true;
                
            }
            if (country != null || city != null)
            {
                searchRequest += " INNER JOIN Locations ON UserProfile.UserId = Locations.Id";
                req = true;
            }
            if (req)
            {
                searchRequest += " where ";
            }
                 
            if (firstName != null)
            {
                var searchPattern = "AboutUsers.FirstName LIKE '%" + firstName + "%'";
                searchRequest += searchPattern;
                and = true;
            }
            if (lastName != null)
            {
                if (and) searchRequest += " and ";
                var searchPattern = "AboutUsers.LastName LIKE '%" + lastName + "%'";
                searchRequest += searchPattern;
                and = true;
            }
            if (country != null)
            {
                if (and) searchRequest += " and ";
                var searchPattern = "Locations.Country LIKE '%" + country + "%'";
                searchRequest += searchPattern;
                and = true;
            }
            if (city != null)
            {
                if (and) searchRequest += " and ";
                var searchPattern = "Locations.City LIKE '%" + city + "%'";
                searchRequest += searchPattern;
            }

            List<int> userIds = context.Database.SqlQuery<int>(searchRequest).ToList();
            List<UserProfile> users = new List<UserProfile>();

            foreach (var item in userIds)
            {
                users.Add(GetUser(item));
            }

            return users;
        }

        public List<Message> GetMessages(int uId, int fId)
        {
            List<Message> messages = context.Messages
                .Where(x => (x.ReceiverId == uId && x.SenderId == fId || x.SenderId == uId && x.ReceiverId == fId))
                .OrderByDescending(x => x.SendDateTime).ToList();

            return messages;
        }

        public List<AboutUser> GetDialogs(int id)
        {
            var dialogsIn = context.Messages.Where(x => x.ReceiverId == id)
                .OrderByDescending(x => x.SendDateTime);
            var dialogsOut = context.Messages.Where(x => x.SenderId == id)
                .OrderByDescending(x => x.SendDateTime);

            List<int> users = new List<int>();

            foreach (var item in dialogsIn)
            {
                users.Add(item.SenderId);
            }
            foreach (var item in dialogsOut)
            {
                users.Add(item.ReceiverId);
            }

            List<UserProfile> up = new List<UserProfile>();
            List<AboutUser> about = new List<AboutUser>();

            foreach (var item in users)
            {
                up.Add(GetUser(item));
            }
            up = up.Distinct().ToList();
            foreach (var item in up)
            {
                about.Add(item.AboutUser);
            }

            return about;           
        }

        public void AddMessage(Message message)
        {
            if (message.Content == null && message.SenderId == 0
                && message.ReceiverId == 0)
                return;

            context.Messages.Add(message);
            context.SaveChanges();
        }
    }
}
