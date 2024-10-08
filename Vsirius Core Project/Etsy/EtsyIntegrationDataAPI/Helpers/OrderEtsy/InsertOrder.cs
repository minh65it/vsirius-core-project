using AutoMapper;
using EFCore.BulkExtensions;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.OrderEtsy;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI.Helpers.OrderEtsy
{
    public interface IInsertOrder
    {
        public Task<OperationResult> InsertUpdateOrder(List<OrderDto> input);
        public Task<OperationResult> InsertUpdateShipment(List<OrderDto> input);
        public Task<OperationResult> InsertUpdateTransaction(List<OrderDto> input);
    }
    public class InsertOrder : IInsertOrder
    {
        private readonly DataContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IMapper _mapper;
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _config;

        public InsertOrder(DataContext context, IOrderRepository orderRepository,
            IAuthenticationEtsyService authenticationEtsyService,
            IMapper mapper,
            ILoggerCustom logService, IConfiguration config)
        {
            _context = context;
            _orderRepository = orderRepository;
            _authenticationEtsyService = authenticationEtsyService;
            _mapper = mapper;
            _logService = logService;
            _config = config;
        }

        public async Task<OperationResult> InsertUpdateOrder(List<OrderDto> input)
        {
            List<Order> result = new List<Order>();
            if (input != null)
            {
                foreach (var item in input)
                {
                    var order = new Order()
                    {
                        receipt_id = item.receipt_id,
                        receipt_type = item.receipt_type,
                        seller_user_id = item.seller_user_id,
                        seller_email = item.seller_email,
                        buyer_user_id = item.buyer_user_id,
                        buyer_email = item.buyer_email,
                        name = item.name,
                        first_line = item.first_line,
                        second_line = item.second_line,
                        city = item.city,
                        state = item.state,
                        zip = item.zip,
                        status = item.status,
                        formatted_address = item.formatted_address,
                        country_iso = item.country_iso,
                        payment_method = item.payment_method,
                        payment_email = item.payment_email,
                        message_from_seller = item.message_from_seller,
                        message_from_buyer = item.message_from_buyer,
                        message_from_payment = item.message_from_payment,
                        is_paid = item.is_paid,
                        is_shipped = item.is_shipped,
                        create_timestamp = item.create_timestamp,
                        created_timestamp = item.created_timestamp,
                        update_timestamp = item.update_timestamp,
                        updated_timestamp = item.updated_timestamp,
                        is_gift = item.is_gift,
                        gift_message = item.gift_message,
                        gift_sender = item.gift_sender,
                        grandtotal_amount = item.grandtotal?.amount,
                        grandtotal_divisor = item.grandtotal?.divisor,
                        grandtotal_currency_code = item.grandtotal?.currency_code,
                        subtotal_amount = item.subtotal?.amount,
                        subtotal_divisor = item.subtotal?.divisor,
                        subtotal_currency_code = item.subtotal?.currency_code,
                        total_price_amount = item.total_price?.amount,
                        total_price_divisor = item.total_price?.divisor,
                        total_price_currency_code = item.total_price?.currency_code,
                        total_shipping_cost_amount = item.total_shipping_cost?.amount,
                        total_shipping_cost_divisor = item.total_shipping_cost?.divisor,
                        total_shipping_cost_currency_code = item.total_shipping_cost?.currency_code,
                        total_tax_cost_amount = item.total_tax_cost?.amount,
                        total_tax_cost_divisor = item.total_tax_cost?.divisor,
                        total_tax_cost_currency_code = item.total_tax_cost?.currency_code,
                        total_vat_cost_amount = item.total_vat_cost?.amount,
                        total_vat_cost_divisor = item.total_vat_cost?.divisor,
                        total_vat_cost_currency_code = item.total_vat_cost?.currency_code,
                        discount_amt_amount = item.discount_amt?.amount,
                        discount_amt_divisor = item.discount_amt?.divisor,
                        discount_amt_currency_code = item.discount_amt?.currency_code,
                        gift_wrap_price_amount = item.gift_wrap_price?.amount,
                        gift_wrap_price_divisor = item.gift_wrap_price?.divisor,
                        gift_wrap_price_currency_code = item.gift_wrap_price?.currency_code
                    };
                    result.Add(order);
                }
            }

            try
            {
                Order order = new Order();
                var bulkConfig = new BulkConfig()
                {

                    PropertiesToExcludeOnUpdate = new List<string> { nameof(order.is_submit) }

                };

                //var del = await _context.Order.Where(s => result.Select(x => x.receipt_id).Contains(s.receipt_id) && s.is_submit == null).ToListAsync();
                //if (del != null)
                //{
                //    _context.RemoveRange(del);
                //}
                //await _context.SaveChangesAsync();

                await _context.BulkInsertOrUpdateAsync(result, bulkConfig);
                return new OperationResult() { Success = true };
            }
            catch (Exception ex)
            {

                await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Etsy");
                return new OperationResult() { Success = false, Message = ex.ToString() };
            }

        }
        public async Task<OperationResult> InsertUpdateShipment(List<OrderDto> input)
        {
            try
            {
                List<Shipment> resultAdd = new List<Shipment>();
                var listShipment = await _context.Shipment
                    .Where(x => input.Select(s => s.receipt_id).ToList().Contains(x.receipt_id)).ToListAsync();
                await _context.BulkDeleteAsync(listShipment);
                if (input != null)
                {
                    foreach (var item in input)
                    {
                        foreach (var i in item.shipments)
                        {
                            Shipment shipment = new Shipment()
                            {
                                receipt_shipping_id = i.receipt_shipping_id,
                                shipment_notification_timestamp = i.shipment_notification_timestamp,
                                carrier_name = i.carrier_name,
                                tracking_code = i.tracking_code,
                                receipt_id = item.receipt_id
                            };
                            resultAdd.Add(shipment);
                        }
                    }
                }



                await _context.BulkInsertAsync(resultAdd);
                return new OperationResult() { Success = true };
            }
            catch (Exception ex)
            {

                await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Etsy");
                return new OperationResult() { Success = false, Message = ex.ToString() };
            }
        }
        public async Task<OperationResult> InsertUpdateTransaction(List<OrderDto> input)
        {
            List<Transaction> resultAdd = new List<Transaction>();
            var listTransaction = await _context.Transaction
                .Where(x => input.Select(s => s.receipt_id).ToList().Contains(x.receipt_id)).ToListAsync();
            await _context.BulkDeleteAsync(listTransaction);
            if (input != null)
            {
                foreach (var item in input)
                {
                    foreach (var i in item.transactions)
                    {
                        Transaction transaction = new Transaction()
                        {
                            receipt_id = item.receipt_id,
                            transaction_id = i.transaction_id,
                            title = i.title,
                            description = i.description,
                            seller_user_id = i.seller_user_id,
                            buyer_user_id = i.buyer_user_id,
                            create_timestamp = i.create_timestamp,
                            created_timestamp = i.created_timestamp,
                            paid_timestamp = i.paid_timestamp,
                            shipped_timestamp = i.shipped_timestamp,
                            quantity = i.quantity,
                            listing_image_id = i.listing_image_id,
                            is_digital = i.is_digital,
                            file_data = i.file_data,
                            listing_id = i.listing_id,
                            transaction_type = i.transaction_type,
                            product_id = i.product_id,
                            sku = i.sku,
                            price_amount = i.price.amount,
                            price_divisor = i.price.divisor,
                            price_currency_code = i.price.currency_code,
                            shipping_cost_amount = i.shipping_cost.amount,
                            shipping_cost_divisor = i.shipping_cost.divisor,
                            shipping_cost_currency_code = i.shipping_cost.currency_code,
                            shipping_profile_id = i.shipping_profile_id,
                            min_processing_days = i.min_processing_days,
                            max_processing_days = i.max_processing_days,
                            shipping_method = i.shipping_method,
                            shipping_upgrade = i.shipping_upgrade,
                            expected_ship_date = i.expected_ship_date,
                            buyer_coupon = i.buyer_coupon,
                            shop_coupon = i.shop_coupon,
                        };
                        resultAdd.Add(transaction);
                    }
                }
            }

            try
            {

                await _context.BulkInsertAsync(resultAdd);
                return new OperationResult() { Success = true };
            }
            catch (Exception ex)
            {

                await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Etsy");
                return new OperationResult() { Success = false, Message = ex.ToString() };
            }
        }
    }
}
