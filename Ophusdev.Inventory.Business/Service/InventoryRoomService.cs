using Confluent.Kafka;
using Inventory.Repository.Abstraction;
using Inventory.Repository.Model;
using Microsoft.Extensions.Logging;
using Ophusdev.Inventory.Business.Abstraction;
using Ophusdev.Inventory.Repository.Model;
using Ophusdev.Inventory.Shared;
using Ophusdev.Kafka.Abstraction;

namespace Ophusdev.Inventory.Business.Service
{
    public class InventoryRoomService : IInventoryRoomService
    {
        private readonly IRepository _inventoryRepository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger _logger;

        public InventoryRoomService(IRepository inventoryRepository, IKafkaProducer kafkaProducer, ILogger<InventoryRoomService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task ProcessInventoryRequestAsync(InventoryRequest request)
        {
            var response = new InventoryResponse
            {
                SagaId = request.SagaId,
                BookingId = request.BookingId,
                RoomId = request.RoomId,
                Success = false,
            };

            _logger.LogInformation("Search availability for room={roomId}, checkInDate={}, checkOutdate={}",
                    request.RoomId,
                    request.CheckInDate,
                    request.CheckOutDate
            );

            try
            {
                Room _room = await _inventoryRepository.ReadRoomAsync(request.RoomId);

                if (_room == null)
                {
                    throw new Exception("Room not found");
                }

                var isAvailable = await _inventoryRepository.IsRoomAvailableAsync(request.RoomId, request.CheckInDate, request.CheckOutDate);

                response = new InventoryResponse
                {
                    SagaId = request.SagaId,
                    BookingId = request.BookingId,
                    RoomId = request.RoomId,
                    Success = isAvailable,
                };

                _logger.LogInformation("Find availability for room={roomId}, checkInDate={}, checkOutdate={}, result={}",
                    request.RoomId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    isAvailable
                );

                if (isAvailable)
                {
                    await _inventoryRepository.ReserveRoomAsync(request.BookingId, request.SagaId, request.RoomId, request.CheckInDate, request.CheckOutDate, request.GuestId, ReservationStatus.Reserved);
                    await _inventoryRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to reserve room. ex={ex}", ex.InnerException);

                response = new InventoryResponse
                {
                    SagaId = request.SagaId,
                    BookingId = request.BookingId,
                    RoomId = request.RoomId,
                    Success = false,
                };
            }

            await _kafkaProducer.ProduceAsync(Topic.TOPIC_INVENTORY_RESPONSE, response);
        }

        public async Task CompensateInventoryAsync(string bookingId)
        {
            _logger.LogInformation("Release reservation for room={bookingId}", bookingId);

            await _inventoryRepository.ReleaseRoomAsync(bookingId);
        }
    }
}
