# Bug.BetterThanYesterday

API .NET 8 do Better Than Yesterday. O passo a passo completo de front e back está em `better-than-yesterday-app/README.md`.

## Pré-requisitos

.NET 8 SDK e um cluster MongoDB Atlas.

## Variáveis de ambiente

A API só sobe com estas variáveis no processo. Defina-as no PowerShell, na mesma sessão do `dotnet run`. Se você gravar as variáveis nas configurações de usuário do Windows, abra um terminal novo antes de iniciar a API.

```powershell
$env:MONGODB_CONNECTIONSTRING = "mongodb+srv://USUARIO:SENHA@CLUSTER/..."
$env:AWS_BTY_ACCESS_KEY = "sua-access-key"
$env:AWS_BTY_SECRET_KEY = "sua-secret-key"
$env:AWS_BTY_S3_BUCKET = "better-than-yesterday"
```

Região e nomes dos bancos já estão em `Bug.BetterThanYesterday.API/appsettings.Development.json`:

- `AwsConfig.Region`: `sa-east-1`
- `DatabaseConfig.DatabaseName`: `better-than-yesterday`
- `DatabaseConfig.TestDatabaseName`: `better-than-yesterday-test`

Não commite connection string nem chaves. Uploads de foto usam `POST /api/Uploads/PresignedUrl`.

## Subir a API

Na pasta deste projeto:

```powershell
dotnet run --project .\Bug.BetterThanYesterday.API\Bug.BetterThanYesterday.API.csproj --urls "http://0.0.0.0:5018"
```

Swagger: `http://localhost:5018/swagger`

## Endpoints

| Método | Caminho | Descrição |
|--------|---------|-----------|
| POST | `/api/Uploads/PresignedUrl` | Devolve `{ uploadUrl, fileUrl }` para upload PUT no S3 |
| POST | `/api/CheckIns/{checkInId}/Reviews` | Adiciona uma review a um check-in |

## CORS (Development)

Libera o servidor de desenvolvimento do Expo (`localhost`, `127.0.0.1` e origens na rede `192.168.*`).
