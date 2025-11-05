param(
    [switch]$Force
)

$ErrorActionPreference = 'Stop'

Write-Host "Ensuring destination folders exist..."
New-Item -ItemType Directory -Force -Path "wwwroot\css" | Out-Null
New-Item -ItemType Directory -Force -Path "wwwroot\js" | Out-Null
New-Item -ItemType Directory -Force -Path "wwwroot\images" | Out-Null

Write-Host "Copying assets from 'website' to 'wwwroot'..."
Copy-Item "website\index-p1x7hlGI.css" "wwwroot\css\index.css" -Force:$Force
Copy-Item "website\index-DOBq2pkS.js" "wwwroot\js\index.js" -Force:$Force
Copy-Item "website\flock.js" "wwwroot\js\flock.js" -Force:$Force
Copy-Item "website\inject.js" "wwwroot\js\inject.js" -Force:$Force
Copy-Item "website\1762253820423-YELHIGH LOHO2.png" "wwwroot\images\yelhigh-logo.png" -Force:$Force

Write-Host "Removing 'website' folder..."
Remove-Item -Recurse -Force "website"

Write-Host "Done. You can now run: dotnet build; dotnet run"

