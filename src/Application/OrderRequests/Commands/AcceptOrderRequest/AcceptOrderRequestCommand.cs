﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pisheyar.Application.Accounts.Commands.RegisterClient;
using Pisheyar.Application.Common.Interfaces;
using Pisheyar.Domain.Entities;
using Pisheyar.Domain.Enums;

namespace Pisheyar.Application.OrderRequests.Commands.AcceptOrderRequest
{
    public class AcceptOrderRequestCommand : IRequest<AcceptOrderRequestVm>
    {
        public Guid OrderRequestGuid { get; set; }

        public class CreateOrderCommandHandler : IRequestHandler<AcceptOrderRequestCommand, AcceptOrderRequestVm>
        {
            private readonly IPisheyarContext _context;
            private readonly ICurrentUserService _currentUser;
            private readonly ISmsService _sms;

            public CreateOrderCommandHandler(IPisheyarContext context, ICurrentUserService currentUserService, ISmsService smsService)
            {
                _context = context;
                _currentUser = currentUserService;
                _sms = smsService;
            }

            public async Task<AcceptOrderRequestVm> Handle(AcceptOrderRequestCommand request, CancellationToken cancellationToken)
            {
                User currentUser = await _context.User
                    .Where(x => x.UserGuid == Guid.Parse(_currentUser.NameIdentifier))
                    .SingleOrDefaultAsync(cancellationToken);

                if (currentUser == null)
                {
                    return new AcceptOrderRequestVm()
                    {
                        Message = "کاربر مورد نظر یافت نشد",
                        State = (int)AcceptOrderRequestState.UserNotFound
                    };
                }

                Client client = await _context.Client
                    .SingleOrDefaultAsync(x => x.UserId == currentUser.UserId && !x.IsDelete, cancellationToken);

                if (client == null) return new AcceptOrderRequestVm
                {
                    Message = "سرویس گیرنده مورد نظر یافت نشد",
                    State = (int)AcceptOrderRequestState.ClientNotFound
                };

                OrderRequest orderRequest = await _context.OrderRequest
                    .SingleOrDefaultAsync(x => x.OrderRequestGuid == request.OrderRequestGuid && !x.IsDelete, cancellationToken);

                if (orderRequest == null) return new AcceptOrderRequestVm
                {
                    Message = "درخواست سفارش مورد نظر یافت نشد",
                    State = (int)AcceptOrderRequestState.OrderRequestNotFound
                };

                orderRequest.IsAccept = true;
                //orderRequest.Order.StateCodeId = 10;
                //orderRequest.Order.ContractorId = orderRequest.ContractorId;

                await _context.SaveChangesAsync(cancellationToken);

                return new AcceptOrderRequestVm
                {
                    Message = "عملیات موفق آمیز",
                    State = (int)AcceptOrderRequestState.Success
                };
            }
        }
    }
}
