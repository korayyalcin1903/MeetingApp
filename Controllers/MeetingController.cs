using System.Security.Claims;
using MeetingApp.Data.Abstract;
using MeetingApp.Data.Concrete;
using MeetingApp.Data.Entity;
using MeetingApp.Entity;
using MeetingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace MeetingApp.Controllers
{
    public class MeetingController:Controller
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly MeetingContext _context;

        public MeetingController(IMeetingRepository meeting, MeetingContext context)
        {
            _meetingRepository = meeting;
            _context = context;
        }

        public IActionResult Index()
        {
            var meetings = _meetingRepository.Meetings.Where(x => x.StartDate > DateTime.Now).ToList();
            return View(meetings);
        }

        public async Task<string> ImageAdd(IFormFile imageFile)
        {
            var extension = "";

            if (imageFile != null)
            {
                var allowExtensions = new[] { ".jpg", ".jpeg", ".png" };
                extension = Path.GetExtension(imageFile.FileName);

                if (!allowExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
                }

                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/meeting", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return randomFileName;
            }

            return null;
        }


        public async Task<IActionResult> Details(int id)
        {
            if(id.ToString() != null){
                var meeting = await _meetingRepository.Meetings.Include(m => m.MeetingUsers).ThenInclude(x => x.User).FirstOrDefaultAsync(m => m.MeetingId == id);
                return View(meeting);
            } else {
                return NotFound();
            }
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();           
        }

        [HttpPost]
        public async Task<IActionResult> Register(int id)
        {
            var meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.MeetingId == id);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if(meeting != null){
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

                var isExist = await _context.MeetingUsers.FirstOrDefaultAsync(x => x.MeetingId == id && x.UserId == user.Id); 

                if(isExist == null){
                    _context.Add( new MeetingUser {
                        MeetingId = meeting.MeetingId,
                        UserId = user.Id
                    });
                } else {
                    return RedirectToAction("RegisterError","Error");
                }
                
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Meeting", new { id = id});
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(MeetingViewModel model, IFormFile imageFile)
        {
            var imageFileName = await ImageAdd(imageFile);

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    _context.Meetings.Add(new Meeting {
                        MeetingName = model.MeetingName,
                        Description = model.Description,
                        Location = model.Location,
                        MeetingPhoto = imageFileName,
                        Subject = model.Subject,
                        StartDate = model.StartDate
                    });

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index","Meeting");
                }
                else
                {
                    ModelState.AddModelError("", "E posta kullanılmaktadır");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null){
                return NotFound();
            }

            var meet = await _meetingRepository.Meetings.FirstAsync(x => x.MeetingId == id);

            if(meet == null){
                return NotFound();
            }
            return View(meet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null){
                return NotFound();    
            }

            var meet = await _meetingRepository.Meetings.FirstAsync(x => x.MeetingId == id);

            if(meet != null){
                _meetingRepository.DeleteMeeting(meet);
                return RedirectToAction("Index");
            } else {
                return NotFound();
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null){
                return NotFound();
            }

            var meet = _meetingRepository.Meetings.FirstOrDefault(x => x.MeetingId == id);

            if(meet != null){
                return View( new MeetingViewModel {
                    MeetingName = meet.MeetingName,
                    Subject = meet.Subject,
                    Description = meet.Description,
                    Location = meet.Location,
                    MeetingPhoto = meet.MeetingPhoto,
                    StartDate = meet.StartDate
                });
            } else {
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(MeetingViewModel model, int id, IFormFile imageFile)
        {
            if(id == null){
                return NotFound();
            }

            var meet = _meetingRepository.Meetings.FirstAsync(x => x.MeetingId == id);

            var imageFileName = await ImageAdd(imageFile);

            if(meet != null && ModelState.IsValid){
                var entityToUpdate = new Meeting{
                    MeetingId = id,
                    MeetingName = model.MeetingName,
                    Description = model.Description,
                    Subject = model.Subject,
                    Location = model.Location,
                    MeetingPhoto = imageFileName,
                    StartDate = model.StartDate
                };

                _meetingRepository.EditMeeting(entityToUpdate);
                return RedirectToAction("Index","Meeting");
            }

            return View(model);
        }
    }

}