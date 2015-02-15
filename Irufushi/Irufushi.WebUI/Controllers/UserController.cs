using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abstract.Abstract;
using Abstract.Entities;
using Irufushi.Domain.Concrete;
using Irufushi.WebUI.Models;
using WebMatrix.WebData;

namespace Irufushi.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserPagesRepository _repository;

        public UserController(IUserPagesRepository userRepository)
        {
            _repository = userRepository;
        }

        [Authorize]
        public ActionResult Index(int? id)
        {
            if(id == null) id = WebSecurity.CurrentUserId;
            UserProfile user = _repository.GetUser((int)id);

            if (id != WebSecurity.CurrentUserId)
            {
                ViewBag.Button = true;
                if (_repository.IsFriend(WebSecurity.CurrentUserId, (int)id))
                    ViewBag.TypeButton = false;
                else ViewBag.TypeButton = true;
            }
            else ViewBag.Button = false;

            return View(user);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) id = WebSecurity.CurrentUserId;
            UserProfile user = _repository.GetUser((int)id);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserProfile model, int? id)
        {
            if (id == null) id = WebSecurity.CurrentUserId;
            if (model.AboutUser.Id == 0) model.AboutUser.Id = (int)id;
            if(model.UserId == 0) model.UserId = (int)id;
            if(model.Contacts.Id == 0) model.Contacts.Id = (int)id;
            if (model.Location.Id == 0) model.Location.Id = (int)id;

            _repository.SaveProfile(model);

            return RedirectToAction("Index", "User", new { id = (int)id });
        }

        [Authorize]
        public ActionResult AddFriend(int id)
        {
            FriendShip friendship = new FriendShip
            {
                UserId = WebSecurity.CurrentUserId,
                FriendId = id
            };

            _repository.AddFriend(friendship);

            return RedirectToAction("Index", "User", new { id = id });
        }

        [Authorize]
        public ActionResult DeleteFriend(int id)
        {
            FriendShip friendship = new FriendShip
            {
                UserId = WebSecurity.CurrentUserId,
                FriendId = id
            };

            _repository.DeleteFriend(friendship);

            return RedirectToAction("Index", "User", new { id = id });
        }

        [Authorize]
        public ActionResult ShowFriends(int? id)
        {
            if(id == null) id = WebSecurity.CurrentUserId;

            UserModel viewModel = new UserModel
            {
                Users = _repository.GetFriends((int)id)
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult ShowAllUsers()
        {
            List<UserProfile> userWM = _repository.UserProfiles.ToList();

            userWM.Remove(_repository.GetUser(WebSecurity.CurrentUserId));

            UserModel viewModel = new UserModel
            {
                Users = userWM
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Search()
        {
            SearchModel model = new SearchModel
            {
                Users = _repository.SearchUsers(null, null, null, null)
            };
            model.Users = model.Users.Where(x => x.UserId != WebSecurity.CurrentUserId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Search(SearchModel model)
        {
            model.Users = _repository.SearchUsers(model.FirstName, model.LastName, 
                model.Country, model.City);

            model.Users = model.Users.Where(x => x.UserId != WebSecurity.CurrentUserId);

            return View(model);
        }

        [Authorize]
        public ActionResult ShowDialogs()
        {
            MessageModel viewModel = new MessageModel
            {
                Dialogs = _repository.GetDialogs(WebSecurity.CurrentUserId)
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult ShowMessages(int? id)
        {
            MessageModel viewModel = new MessageModel
            {
                Messages = _repository.GetMessages(WebSecurity.CurrentUserId, (int)id)
            };

            if (Request.IsAjaxRequest())
                return View(viewModel);

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ShowMessages(MessageModel model, int id)
        {
            Message message = new Message
            {
                SendDateTime = DateTime.Now,
                SenderId = WebSecurity.CurrentUserId,
                ReceiverId = id,
                Content = model.NewMessage.Content
            };
            _repository.AddMessage(message);

            return RedirectToAction("ShowMessages", "User", new { id = id });
        }
    }
}
