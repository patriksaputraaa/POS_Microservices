using Polly;
using Microsoft.AspNetCore.Mvc;
using Pos.Common;
using Pos.Transaction.Service.Clients;
using Pos.Transaction.Service.Entities;
using Polly.Retry;
using RabbitMQ.Client;
using System.Text;

namespace Pos.Transaction.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleItemsController : ControllerBase
    {
        private readonly IRepository<SaleItems> saleItemsRepository;
        private readonly IRepository<Sales> salesRepository;
        private readonly ProductClient productClient;
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly ILogger<SaleItemsController> _logger;
        public SaleItemsController(IRepository<SaleItems> saleItemsRepository, IRepository<Sales> salesRepository, ProductClient productClient, ILogger<SaleItemsController> logger)
        {
            this.saleItemsRepository = saleItemsRepository;
            this.salesRepository = salesRepository;
            this.productClient = productClient;
            _logger = logger;

            retryPolicy = Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
        {
            Console.WriteLine($"Retry {retryCount} failed. Waiting {timeSpan} before next retry.");
        });

            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://localhost:5672"),
            };
        }

        [HttpGet]
        public async Task<IEnumerable<SaleItemsDto>> GetAll()
        {
            var saleItems = (await saleItemsRepository.GetAllAsync()).Select(saleItems => saleItems.AsDto());
            return saleItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleItemsDto>> GetItem(Guid id)
        {
            var saleItems = await saleItemsRepository.GetByIdAsync(id);
            if (saleItems is null)
            {
                return NotFound();
            }
            return saleItems.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<SaleItemsDto>> Post(CreateSaleItemsDto createSaleItemsDto)
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var saleItems = new SaleItems
                {
                    SaleId = createSaleItemsDto.SaleId,
                    ProductId = createSaleItemsDto.ProductId,
                    Quantity = createSaleItemsDto.Quantity,
                    Price = createSaleItemsDto.Price
                };
                await saleItemsRepository.CreateAsync(saleItems);

                var sales = await salesRepository.GetByIdAsync(saleItems.SaleId);
                if (sales != null)
                {
                    sales.TotalAmount += saleItems.Price * saleItems.Quantity;
                    await salesRepository.UpdateAsync(sales);
                }

                var product = await productClient.GetProductsAsync();
                var existingProduct = product.SingleOrDefault(p => p.Id == saleItems.ProductId);
                if (existingProduct != null)
                {
                    var updatedProduct = new ProductDto(
                        existingProduct.Id,
                        existingProduct.Name,
                        existingProduct.CategoryId,
                        existingProduct.Price,
                        existingProduct.Stock - saleItems.Quantity,
                        existingProduct.Description
                        );
                    await productClient.UpdateProductAsync(updatedProduct.Id, updatedProduct);
                    await PublishMessageToRabbitMQ(updatedProduct.Name + " terjual sebanyak " + saleItems.Quantity + " pcs dengan harga " + saleItems.Price);
                }

                var saleItemsDto = saleItems.AsDto();
                return CreatedAtAction(nameof(GetItem), new { id = saleItemsDto.Id }, saleItemsDto);
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateSaleItemsDto updateSaleItemsDto)
        {
            var existingSaleItems = await saleItemsRepository.GetByIdAsync(id);
            if (existingSaleItems is null)
            {
                return NotFound();
            }
            existingSaleItems.SaleId = updateSaleItemsDto.SaleId;
            existingSaleItems.ProductId = updateSaleItemsDto.ProductId;
            existingSaleItems.Quantity = updateSaleItemsDto.Quantity;
            existingSaleItems.Price = updateSaleItemsDto.Price;
            await saleItemsRepository.UpdateAsync(existingSaleItems);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingSaleItems = await saleItemsRepository.GetByIdAsync(id);
            if (existingSaleItems is null)
            {
                return NotFound();
            }
            await saleItemsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task PublishMessageToRabbitMQ(string message)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqp://localhost:5672")
                };

                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "task_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "task_queue",
                    body: body
                );

                _logger.LogInformation($"Pesan berhasil dikirim ke RabbitMQ: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saat mengirim pesan ke RabbitMQ: {ex.Message}");
                throw;
            }
        }
    }
}