using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories.IReponsitories;
using System.Security.Claims;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly RentCarSystemContext dbcontext;
        private readonly IReviewRepository reviewRepository;

        public ReviewController(IMapper mapper, RentCarSystemContext dbcontext, IReviewRepository reviewRepository)
        {
            this.mapper = mapper;
            this.dbcontext = dbcontext;
            this.reviewRepository = reviewRepository;
        }

        // POST: /api/Review
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AddReviewDTO addReviewDTO)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("Bạn cần phải đăng nhập để tạo đánh giá.");
            }

            // Lấy UserId từ token
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không thể xác định thông tin người dùng từ token.");
            }

            var userId = userIdClaim.Value;

            // Kiểm tra xem người dùng đã thuê xe và đã thanh toán thành công chưa
            var hasValidRental = await dbcontext.RentalAgreements.FirstOrDefaultAsync(r =>
                r.CusId == userId && r.Status.ToLower() == "completed");

            if (hasValidRental == null)
            {
                return BadRequest("Bạn cần phải thuê và thanh toán thành công trước khi tạo đánh giá.");
            }

            // Map DTO sang domain model
            var review = mapper.Map<Review>(addReviewDTO);
            review.CusId = userId; // Gán ID người dùng vào review

            review.VehicleId = hasValidRental.VehicleId;
            // Tạo review mới
            await reviewRepository.CreateAsync(review);

            // Trả về kết quả thành công
            return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
        }

        // Get: /api/Review/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var review = await reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ReviewDTO>(review));
        }
        // Get: /api/Review
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            // Đảm bảo pageNumber và pageSize hợp lệ
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // lấy tất cả reviews sau khi phân trang
            var reviews = await reviewRepository.GetPagedReviewAsync(pageNumber, pageSize);
            if(reviews == null)
            {
                return NotFound("No reviews found...");
            }
            // map domain to dto
            return Ok(mapper.Map<List<ReviewDTO>>(reviews));
        }

        // Get: /api/Review/filter-by-rating
        [HttpGet("filter-by-rating")]
        public async Task<IActionResult> GetReviewsByRatings([FromQuery] string ratings)
        {
            // Phân tách chuỗi ratings thành mảng các số
            var ratingList = ratings.Split(',').Select(r => int.Parse(r)).ToList();

            // Kiểm tra xem tất cả các rating có hợp lệ (từ 1 đến 5) không
            if (ratingList.Any(r => r < 1 || r > 5))
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            // Lọc các đánh giá có rating trong danh sách
            var reviews = await reviewRepository.GetReviewsByRatingAsync(ratingList);

            if (reviews == null || !reviews.Any())
            {
                return NotFound("Không tìm thấy đánh giá nào với các rating yêu cầu.");
            }

            // Trả về kết quả
            return Ok(mapper.Map<List<ReviewDTO>>(reviews));
        }

        // Delete: 	/api/Review/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(string id)
        {
            // Tìm review theo ID
            var existingReview = await reviewRepository.GetReviewById(id);
            if (existingReview == null)
            {
                return NotFound("Không tìm thấy đánh giá.");
            }

            // Kiểm tra xem người dùng có đăng nhập và lấy UserId từ token (claims)
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không thể xác định thông tin người dùng từ token.");
            }

            var userId = userIdClaim.Value;

            // Xác minh quyền sở hữu: kiểm tra nếu người dùng hiện tại là chủ sở hữu của review
            if (userId != existingReview.CusId)
            {
                return Forbid("Bạn không có quyền xóa đánh giá này.");
            }

            // Xóa review
            await reviewRepository.DeleteByIdAsync(id);

            return NoContent();
        }

        // Put: /api/Review/{id}
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] string id, [FromBody] UpdateReviewDTO updateReviewDTO)
        {
            // Kiểm tra người dùng đã đăng nhập hay chưa
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("Bạn cần phải đăng nhập để cập nhật đánh giá.");
            }

            // Lấy userId từ token (claim)
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("Không thể xác định thông tin người dùng từ token.");
            }
            var userId = userIdClaim.Value;

            // Map UpdateReviewRequest sang domain model Review
            var reviewDomain = mapper.Map<Review>(updateReviewDTO);

            // Lấy đánh giá hiện tại từ repository
            var existingReview = await reviewRepository.GetReviewById(id);

            if (existingReview == null)
            {
                return NotFound("Không tìm thấy đánh giá cần cập nhật.");
            }

            // Kiểm tra xem đánh giá có thuộc về người dùng này không
            if (existingReview.CusId != userId)
            {
                return Unauthorized("Bạn không có quyền sửa đánh giá này.");
            }

            // Cập nhật review thông qua repository
            var updatedReview = await reviewRepository.UpdateAsync(id, reviewDomain, userId);

            if (updatedReview == null)
            {
                return NotFound("Không tìm thấy đánh giá cần cập nhật hoặc bạn không có quyền sửa đánh giá này.");
            }

            // Trả về thông tin đánh giá đã cập nhật
            return Ok(mapper.Map<ReviewDTO>(updatedReview));
        }


    }
}
        