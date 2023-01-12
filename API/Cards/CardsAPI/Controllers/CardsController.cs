using CardsAPI.Data;
using CardsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get Single Card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetSingleCard")]
        public async Task<IActionResult> GetSingleCard([FromRoute] Guid id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
                return Ok(card);
            else
                return NotFound("Card not found!");
        }
        
        //Post Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingleCard), new { id = card.Id }, card);
        }

        //Update Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCard != null)
            {
                existingCard.CardHolderName = card.CardHolderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC = card.CVC;

                await cardsDbContext.SaveChangesAsync();

                return Ok(existingCard);
            }
            else
                return NotFound("Card not found!");
        }

        //Delete Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCard != null)
            {
                cardsDbContext.Remove(existingCard);

                await cardsDbContext.SaveChangesAsync();

                return Ok(existingCard);
            }
            else
                return NotFound("Card not found!");
        }
    }
}
