using AutoMapper;
using Etkinlik.Dtos;
using EtkinlikYönetimProjesi.Dtos;
using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using EtkinlikYönetimProjesi.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Dtos;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EtkinlikYönetimProjesi.Controllers
{
    [Authorize( Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfwork;
        private readonly JwtSettings tokenOption;
        public UserController( IMapper mapper, IUnitOfWork unitOfwork , IOptions<JwtSettings> options)

        {
          
            _mapper = mapper;
            _unitOfwork = unitOfwork;
            tokenOption = options.Value;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginViewModel userLoginDto)
        {
            
            string hashedPassword = _unitOfwork.User.HashPassword(userLoginDto.Password);
            var admin = _unitOfwork.User.GetFirstOrDefault(x => x.Email == userLoginDto.Email);
            //database de sistem ayağa kalkarken oluşan admin password ile hashpassword aynı olmadığından password güncelleme işlemi yapılıp token oluşması için.
            if (admin.Password != hashedPassword)
            {
                _unitOfwork.User.AdminControl(admin, hashedPassword);
            }

            // Kullanıcı girişi ve token oluşturma işlemi
            var user = _unitOfwork.User.GetFirstOrDefault(x => x.Email == userLoginDto.Email && x.Password == hashedPassword);
            if (user == null)
            {
                return Unauthorized("Hatalı giriş bilgileri.");
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim("userID", user.ID.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole)
                  };

            JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: tokenOption.Issuer,
            audience: tokenOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(tokenOption.Expiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey)), 
            SecurityAlgorithms.HmacSha256));
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string userToken = tokenHandler.WriteToken(securityToken);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Sadece sunucu tarafından erişilebilir
                Expires = DateTime.Now.AddHours(1) // Geçerlilik süresi
            };

            Response.Cookies.Append("userToken", userToken, cookieOptions);
            return Ok(userToken);
          
        }
      
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]

        public IActionResult UserRegister([FromBody] UserRegisterViewModel model)
        {

           if (model.Password != model.PasswordRepeat)
            {
                return BadRequest("Passwords are not equal each other");
            }
           if (ModelState.IsValid)
            {
                var existingUser = _unitOfwork.User.GetFirstOrDefault(x => x.Email == model.EMail);
                if (existingUser != null)
                {
                    return BadRequest("Email is used by someone");
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.EMail,
                    Password = _unitOfwork.User.HashPassword(model.Password),
                    PasswordRepeat = _unitOfwork.User.HashPassword(model.Password),
                    UserRole = model.Role
                };

                // Kullanıcının rollerini eklemek    
                _unitOfwork.User.Add(user);
                _unitOfwork.Save();
                return Created("User created Successfuly", user);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetProfileInformation")]
        public IActionResult GetProfileInformation()
        {
            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            var getProfileUser = _unitOfwork.User.GetUserById(currentUserId);
            if (getProfileUser == null)
            {
                return NotFound("User is not found");
            }
            
            return Ok(getProfileUser);
        
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            var getProfileUser = _unitOfwork.User.GetUserById(currentUserId);
            var roles = _unitOfwork.User.GetUserByRole(getProfileUser.UserRole);
            return Ok(roles.UserRole);
        }

        [HttpPut]
        [Route("UpdateUserInformation")]
        // Bu Actiona giderse kullanıcı şifre yenile gibi bir butonu basmıştır.
        public IActionResult UpdateUserInformation([FromBody] UserUpdateViewModel userUpdateViewModel)
        {
            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            var updatedUser = _unitOfwork.User.GetUserById(currentUserId);
            updatedUser.Password = _unitOfwork.User.HashPassword(userUpdateViewModel.NewPassword);
            updatedUser.PasswordRepeat = _unitOfwork.User.HashPassword(userUpdateViewModel.NewPassword);
            if (updatedUser.Password != null)
            {

                _unitOfwork.User.Update(updatedUser);
                _unitOfwork.Save();
                return Ok("User password is chanced succesfully");
            }
            return BadRequest("User password is not chanced succesfully");
        }

        [HttpPost]
        [Route("AddEvent")]
        public IActionResult AddEvent([FromBody] EventDto eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            Events eventAdd = _mapper.Map<Events>(eventDto);
            _unitOfwork.Event.Add(eventAdd);
            _unitOfwork.Save();
            return Ok(eventAdd);
        }
        [HttpGet]
        [Route("GetCity")]
        [AllowAnonymous]
        public IActionResult GetCity()
        {
            var city = _unitOfwork.City.Getlist();
            return Ok(city);
        }

        [HttpGet]
        [Route("GetCategory")]
        [AllowAnonymous]
        public IActionResult GetCategory()
        {
            var category = _unitOfwork.Category.Getlist();
            return Ok(category);
        }

        [HttpPost]
        [Route("CreateEvent")]
        public IActionResult CreateEvent([FromBody] EventDto model)
        {
            try
            {
                int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
                var category = _unitOfwork.Category.GetCategoryByName(model.CategoryName);
                var city = _unitOfwork.City.GetCityByName(model.CityName);
                var createEvent = new Events
                {

                    Name = model.Name,
                    EventDate = model.EventDate,
                    ApplicationDeadline = model.ApplicationDeadline,
                    Description = model.Description,
                    CityName = model.CityName,
                    Address = model.Address,
                    Capacity = model.Capacity,
                    IsTicketed = model.IsTicketed,
                    CategoryName = model.CategoryName,
                    UserId = currentUserId,
                    CategoryId = category.Id,
                    CityId = city.Id

    
                };
                if (createEvent == null)
                {
                    return BadRequest();
                }
                if (model.IsTicketed)
                {
                    createEvent.EventPayment = model.EventPayment;
                }
                var nameControl = _unitOfwork.Event.GetFirstOrDefault(x=> x.Name ==  model.Name); 
                if(nameControl != null)
                {
                    return BadRequest("Events name is exist please you can add other name");
                }
                _unitOfwork.Event.Add(createEvent);
                _unitOfwork.Save();
                return Created("Events created Successfully", createEvent);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
          }

        }
        [HttpPut]
        [Route("CancelEvent")]
        public IActionResult CancelEvent( [FromBody] EventUpdateViewModel eventUpdateViewModel)
        {
            var cancelEventId = _unitOfwork.Event.GetEventById(eventUpdateViewModel.Id);
            if (cancelEventId == null)
            {
              return NotFound("Event is not found");
            }
            var currentDate = DateTime.Now;
            var calculateDate = (cancelEventId.ApplicationDeadline - currentDate).Days;
            if (calculateDate >= 5)        
             {               
                if (eventUpdateViewModel.IsCancelled)
                {
                    _unitOfwork.Event.Delete(cancelEventId);
                    _unitOfwork.Save();
                    return BadRequest("Event cancelled from the system.");
                }
                else
                {
                    cancelEventId.Capacity = eventUpdateViewModel.Capacity;
                    cancelEventId.Address = eventUpdateViewModel.Address;
                    _unitOfwork.Save();
                    return Ok("Events updated Succesfully");
                }        
             }
            return Ok();
            }
   
        [HttpGet]
        [Route("EventList")]
        public IActionResult EventList()
        {
            //Organiztor admin tarafından onaylanmış olan eventleri listeler.

            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            // gelen id yi events tablosundaki userid ye göre listelemek için.
            var userExistEvents = _unitOfwork.Event.Getlist(x=>x.IsApproved);
            if (userExistEvents == null)
            {
                return NotFound("Events are not found or Admin can be rejected event");
            }
            var model = new EventListViewModel()
            {
                EventList = userExistEvents
            };
            return Ok(userExistEvents);
        }
        [HttpGet]
        [Route("GetEvents")]
        // category ve şehire göre ayrı ayrı yada her ikisi için event listesi içerisinden filtreleme yapılabilir.
        //isterlerse seçeneği oldugundan ayrı ayrı actionlarda yazmayı tercih ettim.
        public IActionResult GetEvents([FromQuery(Name = "category")] string? category, [FromQuery(Name = "city")] string? city)
        {
            List<Events> query = _unitOfwork.Event.GetEvents();
          
            if (category != null)
            {
                query = query.Where(e => e.CategoryName == category).ToList();
            }

            if (city != null)
            {
                query = query.Where(e => e.CityName == city).ToList();
            }

            var events = query.ToList();

            return Ok(events);
        }

    
        [HttpPost]
        [Route("JoinEventById")]
        public IActionResult JoinEventById(JoinEventViewModel joinEventViewModel)
        {
            joinEventViewModel.IsChecked = true;
            var selectedEvent = _unitOfwork.Event.GetEventById(joinEventViewModel.Id);
            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            var events = _unitOfwork.Event.GetEventById(joinEventViewModel.Id);
            var companies = _unitOfwork.Company.GetCompanies();
            var selectedCompanies = companies
            .Where(c => c.EventId == events.Id || c.EventId == null) // bu listeden şirket ad ları ve url gelmektedir.
                 .Select(c => new
                 {
                     c.Name,
                     c.WebsiteUrl
                 }).ToList();

            if (selectedEvent.Capacity > selectedEvent.Participants)
            {
                selectedEvent.Id = joinEventViewModel.Id;
                selectedEvent.Participants++;
                selectedEvent.UserId = currentUserId;
                selectedEvent.IsJoin = joinEventViewModel.IsChecked;
                _unitOfwork.Save();
                return Ok(selectedCompanies);
            }
            return BadRequest("Event capacity is full pls can you select other events ?");

        }

        [HttpGet]
        [Route("JoinEventList")]
        public IActionResult JoinEventList()
        {
            // Katıldığı event Listesi
            int currentUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
            var eventList = _unitOfwork.Event.Getlist(x => x.UserId == currentUserId && x.IsJoin == true);
            var companies = _unitOfwork.Company.GetCompanies();
            if (eventList == null)
            {
                return BadRequest("Event list is null");
            }
            foreach (var item in eventList)
            {
                item.Companies = companies;
            }
            return Ok(eventList);
        }

    
        [HttpGet]
        [Route("TicketJoinEvent/{eventId}")]
        public IActionResult GetTicketFirms(int eventId)
        {
            //Biletli evente katılmak isterse          
            var events = _unitOfwork.Event.GetEventById(eventId);
            var companies = _unitOfwork.Company.GetCompanies();
            var selectedCompanies = companies
            .Where(c => c.EventId == eventId || c.EventId == null) 
                 .Select(c => new
                 {
                   c.Name,
                   c.WebsiteUrl
                 }).ToList();
            return Ok(selectedCompanies);
     
        }

       
    }
}
