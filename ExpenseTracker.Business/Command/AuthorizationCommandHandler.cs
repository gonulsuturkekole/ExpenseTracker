﻿using ExpenseTracker.Base;
using ExpenseTracker.Business.Cqrs;
using ExpenseTracker.Business.Services;
using ExpenseTracker.Persistence;
using ExpenseTracker.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Business.Command;

public class AuthorizationCommandHandler
    : IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly JwtConfig _jwtConfig;

    public AuthorizationCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, JwtConfig jwtConfig)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _jwtConfig = jwtConfig;
    }

    public async Task<ApiResponse<AuthorizationResponse>> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.Request.UserName);
        if (user == null)
            return new ApiResponse<AuthorizationResponse>("User name or password is incorrect");

        var hashedPassword = PasswordGenerator.CreateMD5(request.Request.Password, user.Secret);
        if (hashedPassword != user.Password)
            return new ApiResponse<AuthorizationResponse>("User name or password is incorrect");

        var token = _tokenService.GenerateToken(user);
        var entity = new AuthorizationResponse
        {
            UserName = user.UserName,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration)
        };

        return new ApiResponse<AuthorizationResponse>(entity);
    }
}