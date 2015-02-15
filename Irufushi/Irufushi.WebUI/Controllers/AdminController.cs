using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Irufushi.WebUI.Models;
using Abstract.Abstract;
using Abstract.Entities;
using WebMatrix.WebData;

namespace Irufushi.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserPagesRepository _repository;

        public AdminController(IUserPagesRepository userRepository)
        {
            _repository = userRepository;
        }

        //[Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            UserModel viewModel = new UserModel
            {
                Users = _repository.UserProfiles
            };

            return View(viewModel);
        }

        //[Authorize(Roles="Admin")]
        public ActionResult ShowUser(int? id)
        {
            if (id != null) id = WebSecurity.CurrentUserId;
            UserProfile viewModel = _repository.GetUser((int)id);

            return View(viewModel);
        }
    }
}
