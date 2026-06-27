# Bug.BetterThanYesterday

## Running API

Prerequisites: .NET 8 SDK and MongoDB at `mongodb://localhost:27017`.

```sh
dotnet run --project .\Bug.BetterThanYesterday.API\Bug.BetterThanYesterday.API.csproj --urls "http://0.0.0.0:5018"
```

Swagger: `http://localhost:5018/swagger`

## AWS configuration (presigned uploads)

Check-in photo uploads use `POST /api/Uploads/PresignedUrl`. Configure AWS credentials locally — do not commit secrets.

**Option A — User Secrets (recommended):**

```sh
cd Bug.BetterThanYesterday.API
dotnet user-secrets set "AwsConfig:AccessKey" "YOUR_ACCESS_KEY"
dotnet user-secrets set "AwsConfig:SecretKey" "YOUR_SECRET_KEY"
dotnet user-secrets set "AwsConfig:Region" "sa-east-1"
dotnet user-secrets set "AwsConfig:BucketName" "your-bucket-name"
```

**Option B — `appsettings.Development.json`:** fill the `AwsConfig` section (keep empty strings in git).

## New endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/Uploads/PresignedUrl` | Returns `{ uploadUrl, fileUrl }` for S3 PUT upload |
| POST | `/api/CheckIns/{checkInId}/Reviews` | Add a review to a check-in |

## CORS (Development)

Allows Expo dev server (`localhost`, `127.0.0.1`, and LAN `192.168.*` origins).
