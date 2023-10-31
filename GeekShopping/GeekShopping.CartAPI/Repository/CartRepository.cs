using AutoMapper;
using GeekShopping.CartAPI.Data.DTO;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Context;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationContext _context;
        private IMapper _mapper;

        public CartRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.UserId == userId);
            
            if (cartHeader != null)
            {
                _context.CartDetails.RemoveRange(_context.CartDetails
                    .Where(c => c.CartHeaderId == cartHeader.Id));

                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<CartDTO> FindCartByUserId(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

            Cart cart = new()
            { 
                CartHeader = _mapper.Map<CartHeaderDTO>(cartHeader)
            };

            var cartDetail = _context.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.Id);

            var novoCartDetail = _mapper.Map<List<CartDetailDTO>>(cartDetail);

            foreach (var item in novoCartDetail)
            {
                var product = _context.Products.Where(x => x.Id == cart.CartHeader.Id);
                item.Product = _mapper.Map<ProductDTO>(product.FirstOrDefault());
            }

            cart.CartDetails = _mapper.Map<List<CartDetailDTO>>(novoCartDetail);

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            try
            {
                //CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == ca)

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromCart(long cartDetailsId)
        {
            try
            {
                CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == cartDetailsId);

                int total = _context.CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetail);

                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CartDTO> SaveOrUpdateCart(CartDTO dto)
        {
            Cart cart = _mapper.Map<Cart>(dto);

            var productDB = await _context.Products.FirstOrDefaultAsync(p => 
                p.Id == dto.CartDetails.FirstOrDefault().ProductId);

            if (productDB == null)
            {
                
                Product newProduct = _mapper.Map<Product>(cart.CartDetails.FirstOrDefault().Product);
                
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
            }

            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c =>
                c.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                CartHeader newCartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                _context.CartHeaders.Add(newCartHeader);
                await _context.SaveChangesAsync();

                cart.CartDetails.FirstOrDefault().CartHeaderId = newCartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;

                CartDetail newCartDetail = _mapper.Map<CartDetail>(cart.CartDetails.FirstOrDefault());
                _context.CartDetails.Add(newCartDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        p => p.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                        p.CartHeaderId == cartHeader.Id);

                if (cartDetail == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    CartDetail newCartDetail = _mapper.Map<CartDetail>(cart.CartDetails.FirstOrDefault());

                    _context.CartDetails.Add(newCartDetail);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    CartDetail updateCartDetail = _mapper.Map<CartDetail>(cart.CartDetails.FirstOrDefault());

                    _context.CartDetails.Update(updateCartDetail);
                    await _context.SaveChangesAsync();

                }
            }

            
            return _mapper.Map<CartDTO>(cart);
        }
    }
}
