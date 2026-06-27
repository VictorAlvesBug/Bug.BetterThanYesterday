# Bug.BetterThanYesterday

## Running API

Prerequisites: .NET 8 SDK and MongoDB at `mongodb://localhost:27017`.

```sh
dotnet run --project .\Bug.BetterThanYesterday.API\Bug.BetterThanYesterday.API.csproj --urls "http://0.0.0.0:5018"
```

Swagger: `http://localhost:5018/swagger`

## AWS configuration (presigned uploads)

Check-in photo uploads use `POST /api/Uploads/PresignedUrl`. Configure AWS credentials via environment variables — do not commit secrets.

**Required environment variables** (any of these names work):

| Purpose | Preferred | AWS standard fallback |
|---------|-----------|------------------------|
| Access key | `AWS_ACCESS_KEY` | `AWS_ACCESS_KEY_ID` |
| Secret key | `AWS_SECRET_KEY` | `AWS_SECRET_ACCESS_KEY` |

```powershell
$env:AWS_ACCESS_KEY = "your-access-key"
$env:AWS_SECRET_KEY = "your-secret-key"
```

If you set variables in Windows Settings, restart Cursor/your terminal so the API process can see them. The API also reads User and Machine scope without requiring a restart in some cases.

**Non-secret settings** (`Region`, `BucketName`) live in `appsettings.Development.json` under `AwsConfig`.

**Optional fallback — User Secrets** (overridden by env vars when both are set):

```sh
cd Bug.BetterThanYesterday.API
dotnet user-secrets set "AwsConfig:AccessKey" "YOUR_ACCESS_KEY"
dotnet user-secrets set "AwsConfig:SecretKey" "YOUR_SECRET_KEY"
```

## New endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/Uploads/PresignedUrl` | Returns `{ uploadUrl, fileUrl }` for S3 PUT upload |
| POST | `/api/CheckIns/{checkInId}/Reviews` | Add a review to a check-in |

## CORS (Development)

Allows Expo dev server (`localhost`, `127.0.0.1`, and LAN `192.168.*` origins).
