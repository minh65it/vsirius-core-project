
# VSiriusSystemLog
Gồm:
CorrealationId
    - Thêm builder.Services.AddCorrelationIdGeneratorService(); để tạo  CorrelationId tự động trong các request trong Program.cs
    - Thêm app.AddCorrelationIdMiddleware(); để tự add các CorrealationId vào Headers trong Program.cs
WriteLog
    - Thêm builder.Services.AddWriteLog(); để viết log trong Program.cs
    - CreateLogInformation (string message);
    - CreateLogWarning (string message);
    - CreateLogError (string? exception, string? message = "");
Setup trong file appsetting:
      "ApiVsiriusCredentials": {
        "ClientId": "ProductApi",
        "ClientSecret": "ClientSecret in VSiriusSystem"
      },
      "VsiriusSystemApi": "http://ip:5102/system/api/",
