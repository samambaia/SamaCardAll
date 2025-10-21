@echo off
REM Este script automatiza a publicacao do Blazor WASM e garante que
REM os arquivos comprimidos (.gz) tenham o cabecalho Content-Encoding: gzip no S3.
REM PARA RODAR: use .\deploy-frontend.cmd no Git Bash ou PowerShell.

echo.
echo 1. PUBLICANDO O FRONTEND NOVAMENTE...
dotnet publish FrontWeb/FrontWeb.csproj -c Release -o ./deploy/frontend

echo.
echo --- INICIO DA CORRECAO DE METADADOS PARA O S3 (MIME TYPE E COMPRESSAO) ---

echo.
echo 2. SINCRONIZANDO ARQUIVOS DE CONFIGURACAO E ASSEMBLEIAS NAO COMPRIMIDAS (Padrao: application/octet-stream)...
REM Envia arquivos que nao tem compressao pre-aplicada (.dll, .wasm, etc.)
aws s3 sync ./deploy/frontend/wwwroot/_framework s3://samacard-frontend-app-2025/_framework --exclude ".gz" --exclude ".br" --region us-east-1

echo.
echo 3. CORRECAO CRITICA: SINCRONIZANDO ARQUIVOS DE COMPRESSAO (.gz, .br)...
REM Adiciona Content-Encoding: gzip ou brotli aos arquivos ja comprimidos.
aws s3 sync ./deploy/frontend/wwwroot/_framework s3://samacard-frontend-app-2025/_framework --content-encoding gzip --exclude "" --include ".gz" --region us-east-1
aws s3 sync ./deploy/frontend/wwwroot/_framework s3://samacard-frontend-app-2025/_framework --content-encoding br --exclude "" --include ".br" --region us-east-1

echo.
echo 4. CORRECAO MIME TYPE: BLZOR BOOT JSON (.json)...
REM Garante o Content-Type correto para o arquivo de inicializacao principal.
aws s3 cp ./deploy/frontend/wwwroot/_framework/blazor.boot.json s3://samacard-frontend-app-2025/_framework/blazor.boot.json --content-type "application/json" --region us-east-1

echo.
echo 5. PASSO FINAL: Sincronizacao concluida!
echo.
echo =========================================================================
echo AGORA, O PASSO CRITICO NO CELULAR:
echo LIMPE O CACHE DO NAVEGADOR do seu celular COMPLETAMENTE antes de tentar acessar novamente.
echo =========================================================================
echo