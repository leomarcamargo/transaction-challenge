using AutoMapper;
using Transactions.Application.Commands.CreateTransaction;
using Transactions.Application.DTOs;
using Transactions.Application.Events.TransactionCreated;
using Transactions.Application.Queries.GetTransactionById;
using Transactions.Domain.Entities;

namespace Transactions.Application.Mappings;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<CreateTransactionCommand, Transaction>();
        
        CreateMap<Transaction, CreateTransactionResult>();
        
        CreateMap<Transaction, TransactionCreatedEvent>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        
        CreateMap<Transaction, GetTransactionByIdResult>();
       
        CreateMap<Transaction, TransactionDto>();
    }
}