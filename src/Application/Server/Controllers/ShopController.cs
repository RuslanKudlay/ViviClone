using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Application.EntitiesModels.Models.ViewsModel.RegistrationModels;
using Application.EntitiesModels.Models.ViewsModel;
using Application.EntitiesModels.Models.OrderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ChatModels;
using Application.BBL.ModelBinders;
using Microsoft.Extensions.Logging;

namespace Application.Server.Controllers
{
    public class ShopController : Controller
    {
        private readonly IGOWService _gowService;
        private readonly ICategoryService _categoryService;
        private readonly IWareService _wareService;
        private readonly ICategoryValuesService _categoryValuesService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IChatService _chatService;
        private readonly ILogger<AccountController> _logger;
        private readonly IWishListService _wishListService;

        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(IGOWService gowService,
                              ICategoryService categoryService,
                              IWareService wareService,
                              IBasketService basketService,
                              ICategoryValuesService categoryValuesService,
                              IOrderService orderService,
                              IChatService chatService,
                              UserManager<ApplicationUser> userManager, 
                              ILogger<AccountController> logger,
                              IWishListService wishListService)
        {
            _gowService = gowService;
            _categoryService = categoryService;
            _wareService = wareService;
            _basketService = basketService;
            _categoryValuesService = categoryValuesService;
            _orderService = orderService;
            _userManager = userManager;
            _chatService = chatService;
            _logger = logger;
            _wishListService = wishListService;
        }

        public IActionResult Index([ModelBinder(BinderType = typeof(SearchParamsModelBinder))]SearchWareParamsModel search = null)
        {
            var brandsCategoriesByWares = _wareService.GetWaresBySearchParams(search, User);
            return View("Index", brandsCategoriesByWares);
        }

        [Produces("application/json")]
        public IActionResult GetWaresBySearchParams([FromBody] SearchWareParamsModel searchParams)
        {
            return Ok(_wareService.GetWaresBySearchParams(searchParams, User));
        }

        public IActionResult UpdateSideSearchMenu([FromBody] SideSearchMenuModel sideSearchMenuModel)
        {
            return ViewComponent("SideMenuParamsComponent", sideSearchMenuModel);
        }

        public IActionResult UpdateBrandsSideMenu([FromBody] SideSearchMenuModel sideSearchMenuModel)
        {
            return ViewComponent("BrandsSideMenuComponent", sideSearchMenuModel);
        }

        public IActionResult UpdateWaresComponent([FromBody] QueryWaresModel wares)
        {
            return ViewComponent("WaresComponent", wares);
        }

        public IActionResult WareDetails(string subUrl)
        {
            var ware = _wareService.GetWareBySubUrl(subUrl);
            return View(ware);
        }

        [HttpPost]
        public JsonResult SetWareToBasket(WareModel ware)
        {
            _basketService.AddWareToBasket(ware.Id);
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        [HttpPost]
        public JsonResult ClearBasket()
        {
            _basketService.ClearBasket();
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        [HttpPost]
        public IActionResult AddWareToBasket(WareModel ware)
        {
            _basketService.AddWareToBasket(ware.SubUrl);

            return ViewComponent("BasketComponent", true);

        }

        [HttpPost]
        public JsonResult RemoveItemFromBasket(WareModel ware)
        {
            _basketService.RemoveItemFromBasket(ware.Id);
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        [HttpPost]
        public JsonResult SetCountWareInBasket(WareModel ware, int wareCount)
        {
            _basketService.SetCountWareInBasket(ware.Id, wareCount);
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        [HttpPost]
        public JsonResult UpdateBasket(Dictionary<WareModel, int> wares)
        {
            foreach (KeyValuePair<WareModel, int> item in wares)
            {
                _basketService.SetCountWareInBasket(item.Key.Id, item.Value);
            }
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        public JsonResult OpenBasket()
        {
            BasketModel basket = _basketService.GetBasket();
            return Json(basket);
        }

        public IActionResult GetBasket(bool isFullDisplay = false)
        {
            return ViewComponent("BasketComponent", isFullDisplay);
        }

        [HttpPost]
        public JsonResult Login(LoginViewModel userCredential)
        {
            return Json(userCredential);
        }

        [HttpGet]
        public IActionResult Authorization(string returnUrl = null)
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetModalBasket()
        {
            return ViewComponent("ModalBasket");
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder(RegisterViewModel userModel = null)
        {
            var basket = _basketService.GetBasket();

            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId != null)
            {
                var usId = int.Parse(userId);
                var newOrder = new OrderModel()
                {
                    CreatedDate = DateTime.Now,
                    User = new UserModel() { Id = usId },
                    CountOfWares = basket.BasketItems.Count(),
                };

                var orderDetails = new List<OrderDetailsModel>();

                foreach (var item in basket.BasketItems)
                {
                    orderDetails.Add(new OrderDetailsModel()
                    {
                        Ware = item.Ware,
                        Count = item.WareQuantity
                    });
                }

                newOrder.OrderDetails = orderDetails;

                var result = _orderService.AddOrUpdateOrder(newOrder);

                if (result != null)
                {
                    _basketService.ClearBasket();

                    return Json(new { successMsg = "Заказ оформлен", orderNumber = result.OrderNumber });
                }
                else
                {
                    return Json(new { troubleMsg = "Проблемы с оформлением заказа" });
                }
            }
            else
            {
                string randomPassword = Guid.NewGuid().ToString();
                string randomEmail = Guid.NewGuid().ToString() + "@random.com";
                string randomUserName = Guid.NewGuid().ToString();

                ApplicationUser notRealUser = new ApplicationUser()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.SecondName,
                    PhoneNumber = userModel.Phone,
                    UserName = randomUserName,
                    Email = randomEmail
                };

                var resultRegistration = await _userManager.CreateAsync(notRealUser, randomPassword);

                if (resultRegistration.Succeeded)
                {
                    string idUser = await _userManager.GetUserIdAsync(notRealUser);

                    string emailUser = idUser + "random" + userModel.Email;

                    var resutSetEmail = await _userManager.SetEmailAsync(notRealUser, emailUser);

                    if(!resutSetEmail.Succeeded)
                    {
                        return Json(new { troubleMsg = "Проблемы с оформлением заказа" });
                    }

                    if (idUser != null)
                    {
                        var usId = int.Parse(idUser);
                        var newOrder = new OrderModel()
                        {
                            CreatedDate = DateTime.Now,
                            User = new UserModel() { Id = usId },
                            CountOfWares = basket.BasketItems.Count(),
                        };

                        var orderDetails = new List<OrderDetailsModel>();

                        foreach (var item in basket.BasketItems)
                        {
                            orderDetails.Add(new OrderDetailsModel()
                            {
                                Ware = item.Ware,
                                Count = item.WareQuantity
                            });
                        }

                        newOrder.OrderDetails = orderDetails;

                        var result = _orderService.AddOrUpdateOrder(newOrder);

                        if (result != null)
                        {
                            _basketService.ClearBasket();

                            return Json(new { successMsg = "Заказ оформлен", orderNumber = result.OrderNumber });
                        }
                        else
                        {
                            return Json(new { troubleMsg = "Проблемы с оформлением заказа" });
                        }

                        return Json(new { error = "Ошибка авторизации" });
                    } 
                    else
                    {
                        return Json(new { troubleMsg = "Проблемы с получением id временного пользователя" });
                    }

                    return Json(new { troubleMsg = "Проблемы с регистрацией временного пользователя" });
                }
            }

            return Json(new { troubleMsg = "Проблемы с оформлением заказа" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult OrderHistory()
        {
            QueryOrderModel queryOrder = new QueryOrderModel()
            {
                UserId = int.Parse(_userManager.GetUserId(HttpContext.User)),
                Take = 10,
                Skip = 0
            };

            return View(_orderService.OrderQuery(queryOrder));
        }

        public JsonResult CreateChat()
        {
            var userOrSessionId = _userManager.GetUserId(HttpContext.User);

            var user = new ApplicationUser();

            ChatModel chatModel;

            if (userOrSessionId != null)
            {
                user = _userManager.FindByIdAsync(userOrSessionId).Result;
            }
            else
            {
                userOrSessionId = HttpContext.Session.Id;
            }

            if (HttpContext.Request.Cookies.ContainsKey("BD_Chat"))
            {
                chatModel = _chatService.GetChatByChatGuid(new Guid(HttpContext.Request.Cookies["BD_Chat"].ToString()));
            }
            else
            {
                chatModel = _chatService.CreateChat(userOrSessionId);

                var coockieOpt = new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(30)
                };

                HttpContext.Response.Cookies.Append("BD_Chat", chatModel.ChatGUID.ToString(), coockieOpt);
            }

            if (chatModel == null)
                return Json(StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера. Попробуйте позже."));

            return Json(new { chatModel.Id, chatModel.SessionOrUserId, UserName = user.FirstName ?? "", chatModel.ChatGUID, chatModel.ChatMessages });
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddWareToWishList(WareModel ware)
        {
            int currentUserId = int.Parse(_userManager.GetUserId(User));
            _wishListService.AddWare(ware, currentUserId);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveWareFromList(int wareId)
        {
            _wishListService.RemoveWare(wareId);

            return RedirectToAction("WishListUser", "Account");
        }
    }
}