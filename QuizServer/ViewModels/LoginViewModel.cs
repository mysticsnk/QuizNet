using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.Exceptions;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.UserRelevant;

namespace QuizServer.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IUserRegistrationService _registrationService;
    private readonly IUserValidationService _validationService;
    private readonly INavigationService _navigationService; // Your existing window switcher

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private bool _isRegisterMode;

    public string TitleText => IsRegisterMode ? "Create Teacher Account" : "Teacher Sign In";
    public string ActionButtonText => IsRegisterMode ? "Register" : "Sign In";
    public string ToggleButtonText => IsRegisterMode ? "Already have an account? Log In" : "Don't have an account? Register";

    public LoginViewModel(
        IUserRegistrationService registrationService, 
        IUserValidationService validationService,
        INavigationService navigationService)
    {
        _registrationService = registrationService;
        _validationService = validationService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = string.Empty;
        HasError = false;
        
        // Notify Avalonia UI properties dependent on this state changed
        OnPropertyChanged(nameof(TitleText));
        OnPropertyChanged(nameof(ActionButtonText));
        OnPropertyChanged(nameof(ToggleButtonText));
    }

    [RelayCommand]
    private async Task ExecuteSubmit()
    {
        ErrorMessage = string.Empty;
        HasError = false;

        try
        {
            // 1. Run Inputs through the provided UserValidationService
            if (!_validationService.IsValidEmail(Email))
            {
                SetError("Invalid email address");
                return;
            }

            if (!_validationService.IsValidPassword(Password))
            {
                // Note: IsValidPassword throws ValidationException based on your source code
                return; 
            }

            if (IsRegisterMode && string.IsNullOrWhiteSpace(Username))
            {
                SetError("Username field is required for registration.");
                return;
            }

            // For presentation validation consistency, we pass the text straight to your backend verification handlers
            if (IsRegisterMode)
            {
                IPasswordHashingService hasher = Program.AppHost.Services.GetRequiredService<IPasswordHashingService>();
                string hash = hasher.Hash(Password);
                await _registrationService.RegisterAsync(Username, Email, hash);
                // Auto switch back to login mode on successful creation
                ToggleMode();
                ErrorMessage = "Registration successful! Please login.";
                HasError = true;
            }
            else
            {
                IPasswordHashingService hasher = Program.AppHost.Services.GetRequiredService<IPasswordHashingService>();
                string hash = hasher.Hash(Password);
                await _registrationService.VerifyAsync(Email, hash);
                
                // Success! Progress cleanly over to the Teacher Dashboard View
                
                _navigationService.NavigateTo<DashboardWindowViewModel>();
            }
        }
        catch (UserNotFoundException)
        {
            SetError("No account found matching those credentials.");
        }
        catch (InvalidPasswordException)
        {
            SetError("Incorrect password. Please try again.");
        }
        catch (ValidationException valEx)
        {
            SetError(valEx.Message);
        }
        catch (Exception ex)
        {
            SetError($"An unexpected error occurred: {ex.Message}");
        }
    }

    private void SetError(string message)
    {
        ErrorMessage = message;
        HasError = true;
    }
}