using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Reponsitories.IReponsitories;
using System;
using System.Security.Claims;

namespace RentCarSystem.Reponsitories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public ReviewRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }


        public async Task<Review?> CreateAsync(Review review)
        {
            await dbcontext.Reviews.AddAsync(review);
            await dbcontext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> DeleteByIdAsync(string id)
        {
            var existingReview = await dbcontext.Reviews.FirstOrDefaultAsync(x => x.ReviewId == id);
            if (existingReview == null)
            {
                return null;
            }
            dbcontext.Reviews.Remove(existingReview);
            await dbcontext.SaveChangesAsync();
            return existingReview;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            // Lọc danh sách review theo rating người dùng chọn và sắp xếp theo rating
            var reviews = await dbcontext.Reviews
                                         .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetPagedReviewAsync(int pageNumber, int pageSize)
        {
            return await dbcontext.Reviews.Skip((pageNumber - 1) * pageSize) // Ví dụ nếu ở trong 3, với size là 10, thì bỏ qua 20 datas trước đó
                                           .Take(pageSize)
                                           .ToListAsync();
        }

        public async Task<Review?> GetReviewById(string id)
        {
            return await dbcontext.Reviews.FirstOrDefaultAsync(x=>x.ReviewId == id);
        }

        public async Task<List<Review>> GetReviewsByRatingAsync(List<int> ratings)
        {
            // Lọc các đánh giá có rating trong danh sách ratings
            var reviews = await dbcontext.Reviews
                                         .Where(r => ratings.Contains(r.Rating))
                                         .ToListAsync();

            return reviews;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await dbcontext.Reviews.CountAsync();
        }

        public async Task<Review?> UpdateAsync(string id, Review review, string cusid)
        {
            var existingReview = await dbcontext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id && r.CusId == cusid);
            if (existingReview == null)
            {
                return null;
            }
            // Cập nhật thông tin
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            await dbcontext.SaveChangesAsync();
            return existingReview;
        }
    }
}
