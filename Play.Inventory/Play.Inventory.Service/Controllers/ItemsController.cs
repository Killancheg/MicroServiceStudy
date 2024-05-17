using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryItemRepository;
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public ItemsController(IRepository<InventoryItem> inventoryItemRepository, IRepository<CatalogItem> catalogItemRepository)
        { 
            _inventoryItemRepository = inventoryItemRepository;
            _catalogItemRepository = catalogItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var InventoryItemEntities = await _inventoryItemRepository.GetAllAsync(item => item.UsedId == userId);
            var itemIds = InventoryItemEntities.Select(item => item.CatalogItemId);
            var catalogItemEntities = await _catalogItemRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var inventoryItemDtos = InventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItemEntities.Single(item => item.Id == inventoryItem.CatalogItemId);

                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var inventoryItem = await _inventoryItemRepository.GetAsync(item =>
            item.UsedId == grantItemDto.UserId &&
            item.CatalogItemId == grantItemDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemDto.CatalogItemId,
                    UsedId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _inventoryItemRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await _inventoryItemRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}
