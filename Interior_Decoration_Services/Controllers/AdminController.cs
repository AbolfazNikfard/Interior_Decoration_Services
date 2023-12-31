﻿using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Enum;
using Interior_Decoration_Services.Models;
using Interior_Decoration_Services.Models.View_Models;
using Interior_Decoration_Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Interior_Decoration_Services.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ProjectContext _context;
        private readonly UserManager<User> _userManager;
        private const string SaveSampleImagePath = "assets/images/workSample-image";
        public AdminController(ProjectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BuyerList()
        {
            try
            {
                var buyers = _context.buyers
                    .Include(o => o.orders).Include(u => u.user)
                    .Select(b => new BuyerListViewModel
                    {
                        Id = b.id,
                        Name = b.user.Name,
                        Family = b.user.Family,
                        Email = b.user.Email,
                        SuccessfullOrders = b.orders.Where(o => o.Status == OrderStatus.finished).Count(),
                        CanceledOrder = b.orders.Where(o => o.Status == OrderStatus.canceled).Count()

                    }).ToList();
                return View(buyers);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #region Order
        public IActionResult OrderList(string WhichOrders = "pending")
        {
            try
            {
                ViewData["OrdersType"] = WhichOrders;
                IQueryable<Order> whichOrders;
                List<OrderViewModel> orders;
                if (WhichOrders == "pending")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.Pending);

                else if (WhichOrders == "doing")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.doing);

                else if (WhichOrders == "reffered")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.reffered);

                else if (WhichOrders == "canceled")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.canceled);

                else if (WhichOrders == "finished")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.finished);

                else
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.rejected);
                orders = whichOrders.Include(p => p.product)
                .Include(b => b.buyer).ThenInclude(u => u.user)
                .Select(o => new OrderViewModel
                {
                    orderId = o.Id,
                    orderDescription = o.Description,
                    productId = o.productId,
                    productName = o.product.Name,
                    productImage = o.product.productImage,
                    productPrice = o.product.Price,
                    userName = o.buyer.user.UserName,
                    orderDate = o.createdAt
                })
                .ToList();
                return View(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult DeterminingOrderStatus(int orderId, int status = 0, string wichOrdersList = "pending")
        {
            try
            {
                Order order = _context.orders.SingleOrDefault(o => o.Id == orderId);
                if (order == null)
                    return NotFound();
                order.Status = (OrderStatus)status;
                _context.orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction("OrderList", "Admin", new { WhichOrders = wichOrdersList });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #endregion
        #region EditUser
        [HttpGet]
        public async Task<IActionResult> EditUser(string userEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null) { return NotFound(); }
                UserProfileViewModel userModel = new UserProfileViewModel
                {
                    user = user
                };
                return View(userModel);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] UserProfileViewModel updateUser)
        {
            try
            {
                #region Validation
                bool isNumber;
                isNumber = long.TryParse(updateUser.user.PhoneNumber, out long phone);
                long number = phone;
                int count = 0;
                while (number > 0)
                {
                    number = number / 10;
                    count++;
                }
                if (updateUser.user.Name == null)
                {
                    ModelState.AddModelError("", "لطفا نام را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.Family == null)
                {
                    ModelState.AddModelError("", "لطفا نام خانوادگی را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.Address == null)
                {
                    ModelState.AddModelError("", "لطفا آدرس را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.PhoneNumber == null)
                {
                    ModelState.AddModelError("", "لطفا شماره تماس را وارد کنید");
                    return View(updateUser);
                }
                if (isNumber == false || count != 10)
                {
                    ModelState.AddModelError("", "شماره تماس معتبر نیست");
                    return View(updateUser);
                }
                if (phone == 0)
                {
                    ModelState.AddModelError("", "شماره تماس را بدون صفر اول وارد کنید");
                    return View(updateUser);
                }
                #endregion
                var user = await _userManager.FindByEmailAsync(updateUser.user.Email);
                if (user == null) { return NotFound(); }
                user.Name = updateUser.user.Name;
                user.Family = updateUser.user.Family;
                user.Address = updateUser.user.Address;
                user.PhoneNumber = updateUser.user.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Admin");
                else
                {
                    ModelState.AddModelError("", "در ویرایش مشکل پیش آمده است ");
                    return View(updateUser);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #endregion
        #region DeleteUser
        public async Task<IActionResult> DeleteUser(string userEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }
                buyer.IsDelete = true;

                _context.buyers.Update(buyer);
                user.IsDelete = true;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return StatusCode(500);

                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Admin");
        }
        #endregion
        #region WorkSample
        [HttpGet]
        public IActionResult AddWorkSample(int? sampleId, string Case)
        {
            WorkSamples Sample;
            if (Case == "Edit")
            {
                Sample = _context.workSamples.SingleOrDefault(s => s.Id == sampleId);
                if (Sample == null)
                    return NotFound();
            }

            else if (Case == "Add")
                Sample = new WorkSamples();

            else
                return NotFound();

            ViewData["Case"] = Case;

            var groups = _context.groups.ToList();
            AddWorkSampleViewModel ViewModel = new AddWorkSampleViewModel()
            {
                groups = groups,
                Sample = Sample
            };
            return View(ViewModel);
        }
        [HttpPost]
        public IActionResult AddWorkSample([FromForm] AddWorkSampleViewModel sampleModel, string Case)
        {
            try
            {
                ViewData["Case"] = Case;
                if (Case == "Add")
                {
                    if (sampleModel.SampleImage == null)
                    {
                        ModelState.AddModelError("", "لطفا برای نمونه کار عکس انتخاب کنید");
                        return View(sampleModel);
                    }
                    sampleModel.Sample.Image = saveImages.createImage(sampleModel.SampleImage.FileName.ToString(), sampleModel.SampleImage, SaveSampleImagePath);
                    sampleModel.Sample.createdAt = DateTime.Now;
                    _context.workSamples.Add(sampleModel.Sample);
                }
                else if (Case == "Edit")
                {
                    if (sampleModel.SampleImage != null)
                    {
                        sampleModel.Sample.Image = saveImages.createImage(sampleModel.SampleImage.FileName.ToString(), sampleModel.SampleImage, SaveSampleImagePath);
                        sampleModel.Sample.editedAt = DateTime.Now;
                    }

                    _context.workSamples.Update(sampleModel.Sample);
                }
                else
                    return NotFound();

                _context.SaveChanges();
                return Redirect("/Admin/WorkSampleList");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpGet]
        public IActionResult WorkSampleList(int groupId = 0, string sort = null, int limit = 20, int page = 1)
        {
            if (page < 1)
                return BadRequest(new { StatusCode = 400, message = "page number should be greater than 0" });

            if (limit < 1)
                return BadRequest(new { StatusCode = 400, message = "limit should be greater than 0" });

            int skip = (page - 1) * limit;
            double productCount, result;

            IQueryable<WorkSamples> workSampleList;

            if (groupId == 0)
            {
                workSampleList = _context.workSamples;
                productCount = workSampleList.Count();
            }
            else
            {
                workSampleList = _context.workSamples.Where(ws => ws.groupId == groupId);
                productCount = workSampleList.Count();
            }

            ViewData["page"] = page;
            result = productCount / (double)limit;
            int pageCount = (int)Math.Ceiling(result);
            ViewData["pagesCount"] = pageCount;

            List<WorkSamples> WorkSampleList;

            switch (sort)
            {
                case "Latest":
                    WorkSampleList = workSampleList.OrderByDescending(o => o.createdAt).Skip(skip).Take(limit).ToList();
                    break;

                case "Oldest":
                    WorkSampleList = workSampleList.OrderBy(o => o.createdAt).Skip(skip).Take(limit).ToList();
                    break;

                case "LatestByEdit":
                    WorkSampleList = workSampleList.OrderByDescending(o => o.editedAt).Skip(skip).Take(limit).ToList();
                    break;

                case "OldestByEdit":
                    WorkSampleList = workSampleList.OrderBy(o => o.editedAt).Skip(skip).Take(limit).ToList();
                    break;

                default:
                    WorkSampleList = workSampleList.Skip(skip).Take(limit).ToList();
                    break;
            }

            List<Group> groups = _context.groups.ToList();
            WorkSampleListViewModel workSampleListViewModel = new WorkSampleListViewModel()
            {
                groups = groups,
                Samples = WorkSampleList
            };
            return View(workSampleListViewModel);
        }
        [HttpGet]
        public IActionResult DeleteSample(int sampleId)
        {
            var sample = _context.workSamples.SingleOrDefault(s => s.Id == sampleId);
            if (sample == null)
                return NotFound();
            _context.workSamples.Remove(sample);
            _context.SaveChanges();
            return Redirect("/Admin/WorkSampleList");
        }
        #endregion
        #region Group
        public IActionResult addGroup()
        {
            try
            {
                var groupsWithSubgroups = _context.groups
                    .Include(sg => sg.subGroups).Include(p => p.product)
                    .Select(g => new GroupWithSubGroupsWithProductsViewModel
                    {
                        groupId = g.id,
                        groupName = g.Name,
                        productNumber = g.product.Count(),
                        subGroups = g.subGroups.ToList()
                    }).ToList();
                return View(groupsWithSubgroups);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public IActionResult addGroup(string name)
        {
            try
            {
                if (name != null)
                {
                    Group newGroup = new Group
                    {
                        Name = name,
                    };
                    _context.groups.Add(newGroup);
                    _context.SaveChanges();

                }
                return RedirectToAction("addGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                var group = _context.groups.SingleOrDefault(g => g.id == groupId);
                if (group == null) { return NotFound(); }
                group.IsDelete = true;
                _context.groups.Update(group);
                _context.SaveChanges();
                return RedirectToAction("addGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult EditGroup(int groupId)
        {
            try
            {
                var group = _context.groups.SingleOrDefault(g => g.id == groupId);
                if (group == null) { return NotFound(); }
                return View(group);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public IActionResult EditGroup([FromForm] Group group)
        {
            try
            {
                if (group.Name == null)
                {
                    ModelState.AddModelError("", "لطفا نام گروه را وارد کنید");
                    return View(group);
                }
                _context.groups.Update(group);
                _context.SaveChanges();
                return RedirectToAction("addGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #endregion
        #region SubGroup
        public IActionResult addSubGroup()
        {
            try
            {
                var groups = _context.groups.ToList();
                var subGroups = _context.subGroups
                    .Include(g => g.parentGroup).Include(p => p.product)
                    .Select(sg => new SubGroup_With_It_ProductsNumber_ViewModel
                    {
                        subgroupId = sg.id,
                        subgroupName = sg.Name,
                        productNumber = sg.product.Count(),
                        parentGroupName = sg.parentGroup.Name
                    }).ToList();
                addSubgroupViewModel addSubgroupView = new addSubgroupViewModel
                {
                    subgroups = subGroups,
                    groups = groups
                };
                return View(addSubgroupView);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public IActionResult addsubGroup(string name, int parentId)
        {
            try
            {
                if (name != null)
                {
                    SubGroup newsubGroup = new SubGroup
                    {
                        Name = name,
                        parentGroupId = parentId

                    };
                    _context.subGroups.Add(newsubGroup);
                    _context.SaveChanges();
                }
                return RedirectToAction("addSubGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult EditSubGroup(int subGroupId)
        {
            try
            {
                var Groups = _context.groups.ToList();
                var subGroup = _context.subGroups.SingleOrDefault(sg => sg.id == subGroupId);
                if (subGroup == null) { return NotFound(); }
                EditSubGroupViewModel model = new EditSubGroupViewModel
                {
                    subGroupId = subGroupId,
                    subgroupName = subGroup.Name,
                    parentGroupId = subGroup.parentGroupId,
                    groups = Groups
                };
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public IActionResult EditSubGroup([FromForm] EditSubGroupViewModel model)
        {
            try
            {
                if (model.subgroupName == null)
                {
                    ModelState.AddModelError("", "لطفا نام گروه را وارد کنید");
                    return View(model);
                }
                var subGroup = _context.subGroups.SingleOrDefault(sg => sg.id == model.subGroupId);
                if (subGroup == null) { return NotFound(); }
                subGroup.Name = model.subgroupName;
                subGroup.parentGroupId = model.parentGroupId;
                _context.subGroups.Update(subGroup);
                _context.SaveChanges();
                return RedirectToAction("addsubGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult DeleteSubGroup(int subGroupId)
        {
            try
            {
                var subgroup = _context.subGroups.SingleOrDefault(sg => sg.id == subGroupId);
                if (subgroup == null) { return NotFound(); }
                subgroup.IsDelete = true;
                _context.subGroups.Update(subgroup);
                _context.SaveChanges();
                return RedirectToAction("addSubGroup", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #endregion
    }
}
